using MediatR;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.Queries
{
    //public class GetBuildingPlanByIdQueryHandler : IRequestHandler<GetBuildingPlanByIdQuery, Result<BuildingPlanResponse>>
    //{
    //    private readonly IBuildingPlanRepository _repo;
    //    private readonly IMapper _mapper;

    //    public GetBuildingPlanByIdQueryHandler(IBuildingPlanRepository repo, IMapper mapper)
    //    {
    //        _repo = repo; _mapper = mapper;
    //    }

    //    public async Task<Result<BuildingPlanResponse>> Handle(GetBuildingPlanByIdQuery request, CancellationToken ct)
    //    {
    //        var entity = await _repo.GetByIdAsync(request.Id, ct, includeDocuments: true, includeWorkflow: true);
    //        if (entity == null) return Result.Failure<BuildingPlanResponse>("Not found");
    //        var dto = _mapper.Map<BuildingPlanResponse>(entity);
    //        dto.Documents = entity.Documents.Select(_mapper.Map<ApplicationDocumentResponse>).ToList();
    //        dto.Workflow = entity.WorkflowLogs.OrderBy(x => x.PerformedOn).Select(_mapper.Map<WorkflowLogResponse>).ToList();
    //        return Result.Success(dto);
    //    }
    //}

    //public class SearchBuildingPlansQueryHandler : IRequestHandler<SearchBuildingPlansQuery, Result<PagedResult<BuildingPlanResponse>>>
    //{
    //    private readonly IBuildingPlanRepository _repo;
    //    private readonly IMapper _mapper;

    //    public SearchBuildingPlansQueryHandler(IBuildingPlanRepository repo, IMapper mapper)
    //    {
    //        _repo = repo; _mapper = mapper;
    //    }

    //    public async Task<Result<PagedResult<BuildingPlanResponse>>> Handle(SearchBuildingPlansQuery request, CancellationToken ct)
    //    {
    //        var (items, total) = await _repo.SearchAsync(request.Status, request.Page, request.PageSize, ct);
    //        var dtos = items.Select(_mapper.Map<BuildingPlanResponse>).ToList();
    //        return Result.Success(new PagedResult<BuildingPlanResponse>(dtos, total, request.Page, request.PageSize));
    //    }
    //}

    //public class GetBuildingPlanWorkflowQueryHandler : IRequestHandler<GetBuildingPlanWorkflowQuery, Result<List<WorkflowLogResponse>>>
    //{
    //    private readonly IBuildingPlanRepository _repo;
    //    private readonly IMapper _mapper;

    //    public GetBuildingPlanWorkflowQueryHandler(IBuildingPlanRepository repo, IMapper mapper)
    //    {
    //        _repo = repo; _mapper = mapper;
    //    }

    //    public async Task<Result<List<WorkflowLogResponse>>> Handle(GetBuildingPlanWorkflowQuery request, CancellationToken ct)
    //    {
    //        var entity = await _repo.GetByIdAsync(request.Id, ct, includeWorkflow: true);
    //        if (entity == null) return Result.Failure<List<WorkflowLogResponse>>("Not found");
    //        var logs = entity.WorkflowLogs.OrderBy(x => x.PerformedOn).Select(_mapper.Map<WorkflowLogResponse>).ToList();
    //        return Result.Success(logs);
    //    }
    //}
}
