using FluentValidation;

namespace Studweb.Application.Features.Users.Commands;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("LastName is required");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Password and Confirm Password must be the same");

        RuleFor(x => x.Birthday)
            .NotEmpty().WithMessage("Birthday is required")
            .Must(x => x.Value.AddYears(13).Date <= DateTime.Now.Date)
            .WithMessage("You must be at least 13 years old");

    }
    
}