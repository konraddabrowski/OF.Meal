using FluentValidation;
using OF.Meal.Application.Commands;

namespace OF.Meal.Application.Validators
{
    public class AddMealValidator : AbstractValidator<AddMeal>
    {
        public AddMealValidator()
        {
            RuleFor(x => x.Title).MaximumLength(50);
            RuleFor(x => x.Description).MaximumLength(150);
            RuleFor(x => x.Recipe).MaximumLength(500);
        }
    }
}