using FluentValidation;

namespace Studweb.Application.Features.Notes.Commands.UpdateNote;

public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
{
    public UpdateNoteCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be grater than 0")
            .NotNull().WithMessage("Id is required");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required");
    }
}