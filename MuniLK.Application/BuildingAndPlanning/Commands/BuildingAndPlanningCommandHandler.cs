using MediatR;
using MuniLK.Application.Assignments.Commands;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.BuildingAndPlanning.Mappings;
using MuniLK.Application.Documents.Commands.UploadDocument;
using MuniLK.Application.FeatureId.Interfaces;
using MuniLK.Application.Generic.Interfaces;
using MuniLK.Application.Generic.Result;
using MuniLK.Domain.Constants;
using MuniLK.Domain.Entities;
using MuniLK.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MuniLK.Application.BuildingAndPlanning.Commands
{
    public class SubmitBuildingPlanCommandHandler : IRequestHandler<SubmitBuildingPlanCommand, Result<string>>
    {
        private readonly IBuildingPlanRepository _repository;
        private readonly ICurrentTenantService _currentTenantService;
        private readonly ICurrentUserService _currentUser;
        private readonly IMediator _mediator;
        private readonly IFeatureIdService _featureIdService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkflowService _workflowService;

        public SubmitBuildingPlanCommandHandler(
            IBuildingPlanRepository repository,
            ICurrentTenantService currentTenantService,
            ICurrentUserService currentUser,
            IMediator mediator,
            IFeatureIdService featureIdService, IUnitOfWork unitOfWork, IWorkflowService workflowService)
        {
            _repository = repository;
            _currentTenantService = currentTenantService;
            _currentUser = currentUser;
            _mediator = mediator;
            _featureIdService = featureIdService;
            _unitOfWork = unitOfWork;
            _workflowService = workflowService;
        }

        public async Task<Result<string>> Handle(SubmitBuildingPlanCommand command, CancellationToken ct)
        {
            Guid? tenantId = _currentTenantService.GetTenantId();

            // Upload documents and collect their IDs and metadata
            var uploadedDocuments = new List<(Guid DocumentId, string? LinkContext, bool IsPrimary)>();
            foreach (var doc in command.Request.Documents)
            {
                var uploadResult = await _mediator.Send(new UploadDocumentCommand(doc), ct);
                if (!uploadResult.HasValue)
                    return Result<string>.Failure($"Document upload failed: {uploadResult.Value}");

                uploadedDocuments.Add((uploadResult.Value, "BuildingAndPlanning", true));
            }

            var buildingPlanApplication = command.Request.ToEntity(tenantId, _currentUser.UserName, uploadedDocuments);

            buildingPlanApplication.ApplicationNumber = await _featureIdService.GenerateFeatureIdAsync(ClientConfigurationKeys.BuildingAndPlanFeatureId.ToString(),ct);

            await _repository.AddAsync(buildingPlanApplication, ct);

            var roles = _currentUser.GetRoles() ; // IEnumerable<string> of roles
            var rolesString = string.Join(",", roles);

            await _workflowService.AddLogAsync(
            buildingPlanApplication.Id,
            actionTaken: "Application Submitted",
            previousStatus: null,
            newStatus: "Pending",
            remarks: null,
            performedByUserId: _currentUser.UserId,
            performedByRole: rolesString,
            assignedToUserId: null,
            isSystemGenerated: false,
            ct);

            await _unitOfWork.SaveChangesAsync(ct);
            return Result<string>.Success(buildingPlanApplication.ApplicationNumber);
        }
    }

    //public class VerifyBuildingPlanCommandHandler : IRequestHandler<VerifyBuildingPlanCommand, Result>
    //{
    //    private readonly IBuildingPlanRepository _repo;
    //    private readonly ICurrentUserService _currentUser;

    //    public VerifyBuildingPlanCommandHandler(IBuildingPlanRepository repo, ICurrentUserService currentUser)
    //    {
    //        _repo = repo; _currentUser = currentUser;
    //    }

    //    public async Task<Result> Handle(VerifyBuildingPlanCommand request, CancellationToken ct)
    //    {
    //        var entity = await _repo.GetByIdAsync(request.Id, ct);
    //        if (entity == null) return Result.Failure("Not found");
    //        if (entity.Status != BuildingPlanStatus.Submitted) return Result.Failure("Invalid state transition");

    //        var from = entity.Status;
    //        entity.Status = BuildingPlanStatus.VerifiedByClerk;
    //        entity.WorkflowLogs.Add(new WorkflowLog
    //        {
    //            Id = Guid.NewGuid(),
    //            ApplicationId = entity.Id,
    //            From = from,
    //            To = entity.Status,
    //            Action = "Verify",
    //            Remarks = request.Remarks,
    //            PerformedByUserId = _currentUser.UserId,
    //            PerformedOn = DateTime.UtcNow
    //        });

    //        await _repo.UnitOfWork.SaveChangesAsync(ct);
    //        return Result.Success();
    //    }
    //}

    //public class AssignInspectionCommandHandler : IRequestHandler<AssignInspectionCommand, Result>
    //{
    //    private readonly IBuildingPlanRepository _repo;
    //    private readonly ICurrentUserService _currentUser;

    //    public AssignInspectionCommandHandler(IBuildingPlanRepository repo, ICurrentUserService currentUser)
    //    {
    //        _repo = repo; _currentUser = currentUser;
    //    }

    //    public async Task<Result> Handle(AssignInspectionCommand request, CancellationToken ct)
    //    {
    //        var entity = await _repo.GetByIdAsync(request.Id, ct);
    //        if (entity == null) return Result.Failure("Not found");
    //        if (entity.Status != BuildingPlanStatus.VerifiedByClerk) return Result.Failure("Invalid state transition");

    //        var inspection = new Inspection
    //        {
    //            Id = Guid.NewGuid(),
    //            ApplicationId = entity.Id,
    //            ScheduledOn = request.ScheduledOn,
    //            InspectorUserId = request.InspectorUserId
    //        };

    //        entity.Inspections.Add(inspection);

    //        var from = entity.Status;
    //        entity.Status = BuildingPlanStatus.InspectionAssigned;
    //        entity.WorkflowLogs.Add(new WorkflowLog
    //        {
    //            Id = Guid.NewGuid(),
    //            ApplicationId = entity.Id,
    //            From = from,
    //            To = entity.Status,
    //            Action = "AssignInspection",
    //            Remarks = request.Remarks,
    //            PerformedByUserId = _currentUser.UserId,
    //            PerformedOn = DateTime.UtcNow
    //        });

    //        await _repo.UnitOfWork.SaveChangesAsync(ct);
    //        return Result.Success();
    //    }
    //}

    //public class CompleteInspectionCommandHandler : IRequestHandler<CompleteInspectionCommand, Result>
    //{
    //    private readonly IBuildingPlanRepository _repo;
    //    private readonly ICurrentUserService _currentUser;
    //    public CompleteInspectionCommandHandler(IBuildingPlanRepository repo, ICurrentUserService currentUser)
    //    {
    //        _repo = repo; _currentUser = currentUser;
    //    }

    //    public async Task<Result> Handle(CompleteInspectionCommand request, CancellationToken ct)
    //    {
    //        var entity = await _repo.GetByIdAsync(request.Id, ct);
    //        if (entity == null) return Result.Failure("Not found");
    //        if (entity.Status != BuildingPlanStatus.InspectionAssigned) return Result.Failure("Invalid state transition");

    //        var inspection = entity.Inspections.FirstOrDefault(x => x.Id == request.InspectionId);
    //        if (inspection == null) return Result.Failure("Inspection not found");
    //        inspection.Report = request.Report;
    //        inspection.CompletedOn = DateTime.UtcNow;

    //        var from = entity.Status;
    //        entity.Status = BuildingPlanStatus.Inspected;
    //        entity.WorkflowLogs.Add(new WorkflowLog
    //        {
    //            Id = Guid.NewGuid(),
    //            ApplicationId = entity.Id,
    //            From = from,
    //            To = entity.Status,
    //            Action = "CompleteInspection",
    //            Remarks = null,
    //            PerformedByUserId = _currentUser.UserId,
    //            PerformedOn = DateTime.UtcNow
    //        });

    //        await _repo.UnitOfWork.SaveChangesAsync(ct);
    //        return Result.Success();
    //    }
    //}

    //public class EngineerApproveBuildingPlanCommandHandler : IRequestHandler<EngineerApproveBuildingPlanCommand, Result>
    //{
    //    private readonly IBuildingPlanRepository _repo;
    //    private readonly ICurrentUserService _currentUser;

    //    public EngineerApproveBuildingPlanCommandHandler(IBuildingPlanRepository repo, ICurrentUserService currentUser)
    //    {
    //        _repo = repo; _currentUser = currentUser;
    //    }

    //    public async Task<Result> Handle(EngineerApproveBuildingPlanCommand request, CancellationToken ct)
    //    {
    //        var entity = await _repo.GetByIdAsync(request.Id, ct);
    //        if (entity == null) return Result.Failure("Not found");
    //        if (entity.Status != BuildingPlanStatus.Inspected) return Result.Failure("Invalid state transition");

    //        var from = entity.Status;
    //        entity.Status = BuildingPlanStatus.EngineerApproved;
    //        entity.WorkflowLogs.Add(new WorkflowLog
    //        {
    //            Id = Guid.NewGuid(),
    //            ApplicationId = entity.Id,
    //            From = from,
    //            To = entity.Status,
    //            Action = "EngineerApprove",
    //            Remarks = request.Remarks,
    //            PerformedByUserId = _currentUser.UserId,
    //            PerformedOn = DateTime.UtcNow
    //        });

    //        await _repo.UnitOfWork.SaveChangesAsync(ct);
    //        return Result.Success();
    //    }
    //}

    //public class FinalApproveBuildingPlanCommandHandler : IRequestHandler<FinalApproveBuildingPlanCommand, Result>
    //{
    //    private readonly IBuildingPlanRepository _repo;
    //    private readonly ICurrentUserService _currentUser;

    //    public FinalApproveBuildingPlanCommandHandler(IBuildingPlanRepository repo, ICurrentUserService currentUser)
    //    {
    //        _repo = repo; _currentUser = currentUser;
    //    }

    //    public async Task<Result> Handle(FinalApproveBuildingPlanCommand request, CancellationToken ct)
    //    {
    //        var entity = await _repo.GetByIdAsync(request.Id, ct);
    //        if (entity == null) return Result.Failure("Not found");
    //        if (entity.Status != BuildingPlanStatus.EngineerApproved) return Result.Failure("Invalid state transition");

    //        var from = entity.Status;
    //        entity.Status = BuildingPlanStatus.Approved;
    //        entity.ApprovedOn = DateTime.UtcNow;
    //        entity.ApprovedByUserId = _currentUser.UserId;

    //        entity.WorkflowLogs.Add(new WorkflowLog
    //        {
    //            Id = Guid.NewGuid(),
    //            ApplicationId = entity.Id,
    //            From = from,
    //            To = entity.Status,
    //            Action = "FinalApprove",
    //            Remarks = request.Remarks,
    //            PerformedByUserId = _currentUser.UserId,
    //            PerformedOn = DateTime.UtcNow
    //        });

    //        await _repo.UnitOfWork.SaveChangesAsync(ct);
    //        return Result.Success();
    //    }
    //}

    //public class RejectBuildingPlanCommandHandler : IRequestHandler<RejectBuildingPlanCommand, Result>
    //{
    //    private readonly IBuildingPlanRepository _repo;
    //    private readonly ICurrentUserService _currentUser;

    //    public RejectBuildingPlanCommandHandler(IBuildingPlanRepository repo, ICurrentUserService currentUser)
    //    {
    //        _repo = repo; _currentUser = currentUser;
    //    }

    //    public async Task<Result> Handle(RejectBuildingPlanCommand request, CancellationToken ct)
    //    {
    //        var entity = await _repo.GetByIdAsync(request.Id, ct);
    //        if (entity == null) return Result.Failure("Not found");
    //        if (entity.Status == BuildingPlanStatus.Approved || entity.Status == BuildingPlanStatus.CertificateIssued)
    //            return Result.Failure("Cannot reject at this stage");

    //        var from = entity.Status;
    //        entity.Status = BuildingPlanStatus.Rejected;
    //        entity.WorkflowLogs.Add(new WorkflowLog
    //        {
    //            Id = Guid.NewGuid(),
    //            ApplicationId = entity.Id,
    //            From = from,
    //            To = entity.Status,
    //            Action = "Reject",
    //            Remarks = request.Reason,
    //            PerformedByUserId = _currentUser.UserId,
    //            PerformedOn = DateTime.UtcNow
    //        });

    //        await _repo.UnitOfWork.SaveChangesAsync(ct);
    //        return Result.Success();
    //    }
    //}


    public class CompleteSiteInspectionCommandHandler : IRequestHandler<CompleteSiteInspectionCommand, Result>
    {
        private readonly IBuildingPlanRepository _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly ICurrentTenantService _currentTenant;
        private readonly ISiteInspectionRepository _siteInspectionRepo;
        private readonly IMediator _mediator;

        public CompleteSiteInspectionCommandHandler(
            IBuildingPlanRepository repo, 
            ICurrentUserService currentUser, 
            ICurrentTenantService currentTenant,
            ISiteInspectionRepository siteInspectionRepo,
            IMediator mediator)
        {
            _repo = repo;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _siteInspectionRepo = siteInspectionRepo;
            _mediator = mediator;
        }

        public async Task<Result> Handle(CompleteSiteInspectionCommand request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            if (entity == null) return Result.Failure("Building plan application not found");
            Guid SiteInspectionId = Guid.NewGuid();
            // Create the site inspection record
            var siteInspection = new SiteInspection
            {
                Id = SiteInspectionId,
                TenantId = _currentTenant.GetTenantId(),
                ApplicationId = request.Id,
                InspectionId = SiteInspectionId,
                
                // Core fields
                Status = request.Request.Status,
                Remarks = request.Request.Remarks,
                
                // Metadata
                InspectionDate = request.Request.InspectionDate,
                OfficersPresent = request.Request.OfficersPresent,
                GpsCoordinates = request.Request.GpsCoordinates,
                PhotosPaths = request.Request.Photos != null ? System.Text.Json.JsonSerializer.Serialize(new List<string>()) : null,
                
                // Site conditions
                AccessRoadWidthCondition = request.Request.AccessRoadWidthCondition,
                AccessRoadWidthNotes = request.Request.AccessRoadWidthNotes,
                BoundaryVerification = request.Request.BoundaryVerification,
                BoundaryVerificationNotes = request.Request.BoundaryVerificationNotes,
                Topography = request.Request.Topography,
                TopographyNotes = request.Request.TopographyNotes,
                ExistingStructures = request.Request.ExistingStructures,
                ExistingStructuresNotes = request.Request.ExistingStructuresNotes,
                EncroachmentsReservations = request.Request.EncroachmentsReservations,
                EncroachmentsReservationsNotes = request.Request.EncroachmentsReservationsNotes,
                
                // Compliance checks
                MatchesSurveyPlan = request.Request.MatchesSurveyPlan,
                MatchesSurveyPlanNotes = request.Request.MatchesSurveyPlanNotes,
                ZoningCompatible = request.Request.ZoningCompatible,
                ZoningCompatibleNotes = request.Request.ZoningCompatibleNotes,
                SetbacksObserved = request.Request.SetbacksObserved,
                SetbacksObservedNotes = request.Request.SetbacksObservedNotes,
                FrontSetback = request.Request.FrontSetback,
                RearSetback = request.Request.RearSetback,
                SideSetbacks = request.Request.SideSetbacks,
                EnvironmentalConcerns = request.Request.EnvironmentalConcerns,
                EnvironmentalConcernsNotes = request.Request.EnvironmentalConcernsNotes,
                
                // Decision support
                RequiredModifications = request.Request.RequiredModifications,
                ClearancesRequired = request.Request.ClearancesRequired != null ? 
                    System.Text.Json.JsonSerializer.Serialize(request.Request.ClearancesRequired) : null,
                FinalRecommendation = request.Request.FinalRecommendation,
                
                // Timestamps
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _currentUser.UserId
            };

            // Save the detailed site inspection
            await _siteInspectionRepo.AddAsync(siteInspection, ct);
            await _siteInspectionRepo.SaveChangesAsync(ct);

            //// For backward compatibility, also store in the legacy Report field
            //var legacyReport = $"Site Inspection - Status: {request.Request.Status}, " +
            //                 $"Date: {request.Request.InspectionDate:yyyy-MM-dd}, " +
            //                 $"Final Recommendation: {request.Request.FinalRecommendation}";
            
            //// Use the original CompleteInspectionCommand to update the existing Inspection table
            //var legacyResult = await _mediator.Send(new CompleteInspectionCommand(request.Id, legacyReport), ct);
            //if (!legacyResult.Succeeded) return legacyResult;

            return Result.Success();
        }
    }

}
