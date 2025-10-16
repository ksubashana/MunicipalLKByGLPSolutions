using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.Contact.DTOs;
using MuniLK.Application.PropertiesLK.DTOs;
using MuniLK.Domain.Constants.Flows;

namespace MuniLK.Application.BuildingAndPlanning.Mappings
{
    public static class BuildingPlanDtoMapper
    {
        public static BuildingPlanApplicationDto ToApplicationDto(
            this BuildingPlanResponse summary,
            ContactResponse? applicant = null,
            PropertyResponse? property = null)
        {
            if (summary == null) throw new ArgumentNullException(nameof(summary));

            return new BuildingPlanApplicationDto
            {
                Id = summary.Id,
                ApplicationNumber = summary.ApplicationNumber,
                Status = MapStatus(summary.Status?.Value),

                ApplicantContactId = summary.ApplicantContactId,
                ApplicantSummary = applicant,

                PropertyId = summary.PropertyId,
                PropertySummary = property,

                BuildingPurpose = summary.BuildingPurpose,
                NoOfFloors = summary.NoOfFloors,
                ArchitectName = null,
                EngineerName = null,
                Remarks = null,

                SubmittedOn = summary.SubmittedOn,
                ApprovedOn = null,
                ApprovedByUserId = null,

                PlanningReport = null,
                EngineerReport = null,
                CommissionerDecision = null,

                AssignmentId = summary.AssignmentId,
                // Map already projected assignment response (no cycling)
                Assignment = null, // keep null here; consumer uses ActiveAssignmentId & date

                Documents = summary.Documents ?? new List<ApplicationDocumentResponse>(),
                WorkflowHistory = summary.Workflow ?? new List<WorkflowLogResponse>()
            };
        }

        private static BuildingAndPlanSteps MapStatus(string? statusValue)
        {
            if (string.IsNullOrWhiteSpace(statusValue))
                return BuildingAndPlanSteps.Submission;

            // Try exact enum name (case-insensitive)
            if (Enum.TryParse(statusValue, true, out BuildingAndPlanSteps parsed))
                return parsed;

            // Try matching Display(Name)
            foreach (var value in Enum.GetValues(typeof(BuildingAndPlanSteps)).Cast<BuildingAndPlanSteps>())
            {
                var display = GetDisplayName(value);
                if (!string.IsNullOrWhiteSpace(display) &&
                    string.Equals(display, statusValue, StringComparison.OrdinalIgnoreCase))
                {
                    return value;
                }
            }

            // Try normalized (remove spaces) against Display(Name)
            string Normalize(string s) => new string(s.Where(c => !char.IsWhiteSpace(c)).ToArray());
            var normalized = Normalize(statusValue);
            foreach (var value in Enum.GetValues(typeof(BuildingAndPlanSteps)).Cast<BuildingAndPlanSteps>())
            {
                var display = GetDisplayName(value);
                if (!string.IsNullOrWhiteSpace(display) &&
                    string.Equals(Normalize(display), normalized, StringComparison.OrdinalIgnoreCase))
                {
                    return value;
                }
            }

            return BuildingAndPlanSteps.Submission;
        }

        private static string? GetDisplayName(BuildingAndPlanSteps value)
        {
            var member = typeof(BuildingAndPlanSteps).GetMember(value.ToString()).FirstOrDefault();
            var display = member?.GetCustomAttributes(typeof(DisplayAttribute), false)
                                .Cast<DisplayAttribute>()
                                .FirstOrDefault();
            return display?.Name;
        }
    }
}