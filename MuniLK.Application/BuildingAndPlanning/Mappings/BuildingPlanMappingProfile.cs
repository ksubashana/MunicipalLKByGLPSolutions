using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Domain.Constants.Flows;
using MuniLK.Domain.Entities;


namespace MuniLK.Application.BuildingAndPlanning.Mappings
{

    public static class BuildingPlanMappingProfile
    {
        public static BuildingPlanApplication ToEntity(
            this SubmitBuildingPlanRequest dto,
            Guid? tenantId,
            string? createdBy,
            List<(Guid DocumentId, string? LinkContext, bool IsPrimary)> uploadedDocuments)
        {

            var entity = new BuildingPlanApplication
            {
                Id = dto.ApplicationId,
                TenantId = tenantId,
                ApplicantContactId = dto.ApplicantContactId,
                PropertyId = dto.PropertyId,
                SubmittedOn = DateTime.UtcNow,
                ApplicationNumber = "",
                BuildingPurpose = dto.BuildingPurpose,
                NoOfFloors = dto.NoOfFloors,
                ArchitectName = dto.ArchitectName,
                EngineerName = dto.EngineerName,
                Remarks = dto.Remarks,
                Status = BuildingAndPlanSteps.Submission,
                Documents = uploadedDocuments.Select(doc => new DocumentLink
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    DocumentId = doc.DocumentId,
                    ModuleId = new Guid("00000000-0000-0000-0000-000000000001"), // Replace with actual ModuleId for BuildingPlan
                    EntityId = dto.ApplicantContactId,
                    LinkContext = doc.LinkContext,
                    IsPrimary = doc.IsPrimary,
                    LinkedBy = createdBy,
                    LinkedDate = DateTime.UtcNow
                }).ToList(),
                Assignments = new List<Assignment>()
            };

            return entity;
        }
    }

}
