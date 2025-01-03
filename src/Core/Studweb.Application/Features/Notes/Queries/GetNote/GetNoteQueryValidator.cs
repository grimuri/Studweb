using FluentValidation;

namespace Studweb.Application.Features.Notes.Queries.GetNote;

public class GetNoteQueryValidator : AbstractValidator<GetNoteQuery>
{
    public GetNoteQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .GreaterThan(0).WithMessage("Id must be grater than 0");
    }
}