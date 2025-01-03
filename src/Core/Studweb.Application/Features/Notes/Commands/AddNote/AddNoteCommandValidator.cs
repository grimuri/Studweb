using FluentValidation;

namespace Studweb.Application.Features.Notes.Commands.AddNote;

public sealed class AddNoteCommandValidator : AbstractValidator<AddNoteCommand>
{
    public AddNoteCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required");
    }
}