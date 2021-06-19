using System;
using System.Net;
using Arcaim.Assertor.Exceptions;
using Arcaim.Exception;
using Microsoft.Extensions.Logging;
using OF.Meal.Core.Exceptions;
using ApplicationException = OF.Meal.Core.Exceptions.ApplicationException;

namespace OF.Meal.Infrastructure.Mappers
{
    public class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        private ILogger<ExceptionToResponseMapper> _logger;

        public ExceptionToResponseMapper(ILogger<ExceptionToResponseMapper> logger)
            => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public ExceptionResponse Map(Exception exception)
        {
            var exceptionResponse = exception switch
            {
                ApplicationException ex => new ExceptionResponse(new { code = ex.Code, message = ex.Message }, (HttpStatusCode)ex.StatusCode, ex.Source),
                InfrastructureException ex => new ExceptionResponse(new { code = ex.Code, message = ex.Message }, (HttpStatusCode)ex.StatusCode, ex.Source),
                ValidationException ex => new ExceptionResponse(new { code = ex.Code, message = ex.Message }, (HttpStatusCode)ex.StatusCode, "Assertor"),
                _ => new ExceptionResponse(new { code = "error", message = "There was an error." }, HttpStatusCode.InternalServerError, "Undefined")
            };

            _logger.LogInformation($"[{DateTime.UtcNow}] [{exceptionResponse.Source}] [{exceptionResponse.StatusCode}]\nMessage: [{exception.Message}]\nResponse: {exceptionResponse.Response}");

            return exceptionResponse;
        }
    }
}