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

            RuleFor(x => x.MeetingDate)
                .NotEmpty()
                .WithMessage("Meeting date is required")
                .Must(BeValidMeetingDate)
                .WithMessage("Meeting date cannot be in the future");

            RuleFor(x => x.CommitteeType)
                .NotEmpty()
                .IsInEnum()
                .WithMessage("Valid committee type is required");

            RuleFor(x => x.MeetingReferenceNo)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Meeting reference number is required and must be less than 100 characters");

            RuleFor(x => x.ChairpersonName)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Chairperson name is required and must be less than 200 characters");

            RuleFor(x => x.MembersPresent)
                .NotEmpty()
                .WithMessage("At least one committee member must be present")
                .Must(HaveValidMembers)
                .WithMessage("All committee members must have both name and designation");

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

        private bool HaveValidMembers(List<CommitteeMember> members)
        {
            if (members == null || members.Count == 0)
                return false;

            // At least one member must have both name and designation filled
            return members.Any(m => !string.IsNullOrWhiteSpace(m.Name) && 
                                   !string.IsNullOrWhiteSpace(m.Designation));
        }
    }
}