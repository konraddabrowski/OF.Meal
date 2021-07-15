using Arcaim.CQRS.WebApi.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OF.Meal.Core.Settings;
using System.Net.Http;
using System.Net;
using System;
using System.Text;
using System.Linq;
using OF.Meal.Infrastructure.Exceptions;
using System.Collections.Generic;
using Arcaim.CQRS.WebApi.Exceptions;
using System.Web;

namespace OF.Meal.Infrastructure.Services
{
    public class Authorization : IAuthorization
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppSettings _appSettings;
        private CookieContainer _cookieContainer;
        private HttpRequestMessage _request;
        private IEnumerable<IEnumerable<string>> _requiredRoles;

        public Authorization(IHttpContextAccessor httpContextAccessor, IAppSettings appSettings)
        {
            _httpContextAccessor = httpContextAccessor
                ?? throw Exceptions.ArgumentNullException.Create(nameof(httpContextAccessor));
            _appSettings = appSettings
                ?? throw Exceptions.ArgumentNullException.Create(nameof(appSettings));
        }

        public void SetRequiredRoles(IEnumerable<IEnumerable<string>> requiredRoles)
            => _requiredRoles = requiredRoles;
        
        public async Task AuthorizeAsync()
        {
            try
            {
                await InternalAuthorizeAsync();
            }
            catch(System.ArgumentNullException ex)
            {
                throw Exceptions.ArgumentNullException.Create(ex);
            }
            //TODO: catch them all
        }

        private async Task InternalAuthorizeAsync()
        {
            SetCookieContainerFromRequest();
            SetAuthorizeHttpRequestMessage();
            using var handler = new HttpClientHandler { CookieContainer = _cookieContainer };
            using var client = new HttpClient(handler);
            using var response = await client.SendAsync(_request);

            if (!response.IsSuccessStatusCode)
            {
                throw UnauthorizedUserException.Create();
            }

            VerifyRequiredRoles();
        }

        private void SetCookieContainerFromRequest()
        {
            var requestCookiesName = "Request.Cookies";
            _cookieContainer = new CookieContainer();
            var cookies = _httpContextAccessor.HttpContext.Request.Cookies;
            if (!cookies.Any())
            {
                throw CollectionEmptyException.Create(requestCookiesName);
            }
            foreach (var cookie in cookies)
            {
                _cookieContainer.Add(new Cookie(cookie.Key, cookie.Value));
            }
        }

        private void SetAuthorizeHttpRequestMessage() => _request = new()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_appSettings.Services.Auth.Address),
            Content = new StringContent("{}", Encoding.UTF8, "application/json"),
        };

        private void VerifyRequiredRoles()
        {
            if (_requiredRoles is null || !_requiredRoles.Any())
            {
                return;
            }

            var userRoles = UserRoles();
            var hasPermissions = _requiredRoles
                .Select(roles => roles
                .Any(requiredRole => userRoles
                .Any(userRole => requiredRole
                .Equals(userRole, StringComparison.OrdinalIgnoreCase))));
            if (hasPermissions.Any(x => !x))
            {
                throw NoPermissionException.Create();
            }
        }

        private IEnumerable<string> UserRoles()
        {
            var rolesKey = "user_cookie";
            var responseCookies = _cookieContainer
                .GetCookies(_request.RequestUri)
                .Cast<Cookie>();
            var rolesCookie = responseCookies.Single(x => x.Name == rolesKey);
            var value = HttpUtility.UrlDecode(rolesCookie.Value);
            var userCookie = Utf8Json.JsonSerializer.Deserialize<UserCookie>(value);

            return userCookie.Roles;
        }

        public class UserCookie //TODO: Move to contract
        {
            public int Id { get; set; }
            public string Value { get; set; }
            public IEnumerable<string> Roles { get; set; }
        }
    }
}