using FluentValidation;

namespace HomeHub.Application.Homes.Validators
{
    public class CreateHomeRequestValidator : AbstractValidator<CreateHomeRequest>
    {
        public CreateHomeRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            // Assuming you map 'Price' (DTO) to 'BasePrice' (Entity)
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.")
                .LessThan(100_000_000).WithMessage("Price is unreasonably high.");

            RuleFor(x => x.Bedrooms)
                .GreaterThanOrEqualTo(0);

            // FIX: Validating a GUID
            // We use .NotEmpty() instead of .GreaterThan(0)
            RuleFor(x => x.CommunityId)
                .NotEmpty().WithMessage("Community ID is required.");
        }
    }
}