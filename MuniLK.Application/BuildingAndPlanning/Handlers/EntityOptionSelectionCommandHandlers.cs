using MediatR;
using MuniLK.Application.BuildingAndPlanning.Commands;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using MuniLK.Application.BuildingAndPlanning.Interfaces;
using MuniLK.Application.Generic.Result;
using MuniLK.Domain.Entities;
using MuniLK.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.Handlers
{
    public class SaveEntityOptionSelectionsCommandHandler 
        : IRequestHandler<SaveEntityOptionSelectionsCommand, Result<EntityOptionSelectionsResponse>>
    {
        private readonly IEntityOptionSelectionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public SaveEntityOptionSelectionsCommandHandler(
            IEntityOptionSelectionRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<EntityOptionSelectionsResponse>> Handle(
            SaveEntityOptionSelectionsCommand request, 
            CancellationToken ct)
        {
            // Validate input
            if (request.EntityId == Guid.Empty)
                return Result<EntityOptionSelectionsResponse>.Failure("EntityId is required");

            if (string.IsNullOrWhiteSpace(request.EntityType))
                return Result<EntityOptionSelectionsResponse>.Failure("EntityType is required");

            if (request.ModuleId == Guid.Empty)
                return Result<EntityOptionSelectionsResponse>.Failure("ModuleId is required");

            // (Legacy OptionItemIds validation skipped - moving to LookupIds)

            try
            {
                // Start transaction
                using var transaction = await _unitOfWork.BeginTransactionAsync(ct);

                try
                {
                    // Delete existing selections
                    await _repository.DeleteSelectionsAsync(
                        request.EntityId, 
                        request.EntityType, 
                        request.ModuleId, 
                        ct);

                    // Add new selections
                    if (request.OptionItemIds != null && request.OptionItemIds.Any())
                    {
                        var newSelections = request.OptionItemIds.Select(optionItemId => new EntityOptionSelection
                        {
                            Id = Guid.NewGuid(),
                            EntityId = request.EntityId,
                            EntityType = request.EntityType,
                            ModuleId = request.ModuleId,
                            OptionItemId = optionItemId,
                            LookupId = optionItemId // Treat incoming ids as lookup ids directly in transition phase
                        }).ToList();

                        await _repository.AddSelectionsAsync(newSelections, ct);
                    }

                    // Save changes
                    await _unitOfWork.SaveChangesAsync(ct);
                    await _unitOfWork.CommitTransactionAsync(ct);

                    var response = new EntityOptionSelectionsResponse
                    {
                        EntityId = request.EntityId,
                        EntityType = request.EntityType,
                        ModuleId = request.ModuleId,
                        SelectedOptionItemIds = request.OptionItemIds ?? new List<Guid>(),
                        Success = true,
                        Message = "Selections saved successfully"
                    };

                    return Result<EntityOptionSelectionsResponse>.Success(response);
                }
                catch (Exception)
                {
                    await _unitOfWork.RollbackTransactionAsync(ct);
                    throw;
                }
            }
            catch (Exception ex)
            {
                return Result<EntityOptionSelectionsResponse>.Failure($"Error saving selections: {ex.Message}");
            }
        }
    }

    public class DeleteEntityOptionSelectionsCommandHandler 
        : IRequestHandler<DeleteEntityOptionSelectionsCommand, Result>
    {
        private readonly IEntityOptionSelectionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEntityOptionSelectionsCommandHandler(
            IEntityOptionSelectionRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeleteEntityOptionSelectionsCommand request, 
            CancellationToken ct)
        {
            try
            {
                await _repository.DeleteSelectionsAsync(
                    request.EntityId, 
                    request.EntityType, 
                    request.ModuleId, 
                    ct);

                await _unitOfWork.SaveChangesAsync(ct);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error deleting selections: {ex.Message}");
            }
        }
    }
}
