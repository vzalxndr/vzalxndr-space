using FluentValidation;
using VzalxndrSpace.Api.DTOs;

namespace VzalxndrSpace.Api.Validators;

public class StartSessionRequestValidator : AbstractValidator<StartSessionRequest>
{
    public StartSessionRequestValidator()
    {
        RuleFor(x => x.GoalId)
            .NotEmpty().WithMessage("Goal ID is required.");

        RuleFor(x => x.TargetDurationMinutes)
            .GreaterThan(3).WithMessage("Target duration must be at least 4 minutes.");
    }
}