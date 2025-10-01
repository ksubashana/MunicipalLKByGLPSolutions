using FluentValidation;
using MuniLK.Application.BuildingAndPlanning.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.BuildingAndPlanning.Validators
{
    public class SubmitBuildingPlanRequestValidator : AbstractValidator<SubmitBuildingPlanRequest>
    {
        public SubmitBuildingPlanRequestValidator()
        {
            RuleFor(x => x.ApplicantContactId).NotEmpty();
            RuleFor(x => x.PropertyId).NotEmpty();
            RuleFor(x => x.BuildingPurpose).NotEmpty().MaximumLength(200);
            RuleFor(x => x.NoOfFloors).GreaterThan(0);
            RuleForEach(x => x.Documents).ChildRules(d =>
            {
                d.RuleFor(x => x.DocumentTypeId).NotEmpty();
                d.RuleFor(x => x.File).NotEmpty();
            });
        }
    }
}
