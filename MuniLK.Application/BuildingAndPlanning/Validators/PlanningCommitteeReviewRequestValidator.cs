using FluentValidation;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Domain.Constants.Flows;

namespace MuniLK.Application.BuildingAndPlanning.Validators
{
    public class PlanningCommitteeReviewRequestValidator : AbstractValidator<PlanningCommitteeReviewRequest>
    {
        public PlanningCommitteeReviewRequestValidator()
        {
            RuleFor(x => x.ApplicationId)
                .NotEmpty()
                .WithMessage("Application ID is required");

      
            RuleFor(x => x.CommitteeDecision)
                .NotEmpty()
                .IsInEnum()
                .WithMessage("Committee decision is required");

            // Conditional validation based on committee decision
            When(x => x.CommitteeDecision == CommitteeDecision.ApproveWithConditions, () =>
            {
                RuleFor(x => x.ConditionsImposed)
                    .NotEmpty()
                    .WithMessage("Conditions must be specified when approving with conditions");
            });

            When(x => x.CommitteeDecision == CommitteeDecision.Reject || 
                     x.CommitteeDecision == CommitteeDecision.DeferForClarifications, () =>
            {
                RuleFor(x => x.ReasonForRejectionOrDeferral)
                    .NotEmpty()
                    .WithMessage("Reason must be provided when rejecting or deferring");
            });

            RuleFor(x => x.RecordedByOfficer)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Recording officer name is required");

            // Optional field validations
            RuleFor(x => x.CommitteeDiscussionsSummary)
                .MaximumLength(4000)
                .WithMessage("Committee discussions summary must be less than 4000 characters");

            RuleFor(x => x.ConditionsImposed)
                .MaximumLength(4000)
                .WithMessage("Conditions imposed must be less than 4000 characters");

            RuleFor(x => x.ReasonForRejectionOrDeferral)
                .MaximumLength(4000)
                .WithMessage("Reason for rejection or deferral must be less than 4000 characters");
        }

        private bool BeValidMeetingDate(DateTime meetingDate)
        {
            // Meeting date should not be in the future (allowing today)
            return meetingDate.Date <= DateTime.Today;
        }

    }
}