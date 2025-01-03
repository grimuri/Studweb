using FluentValidation;

namespace Studweb.Application.Features.Notes.Commands.DeleteNote;

public sealed class DeleteNoteCommandValidator : AbstractValidator<DeleteNoteCommand>
{
    public DeleteNoteCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Id is required")
            .GreaterThan(0).WithMessage("Id must be grater than 0");
    }
}