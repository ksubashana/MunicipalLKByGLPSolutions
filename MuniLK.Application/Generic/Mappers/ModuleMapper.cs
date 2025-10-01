using AutoMapper;
using MuniLK.Application.Generic.DTOs;
using MuniLK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MuniLK.Application.Generic.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Module Mappings
            CreateMap<ModuleCreateDto, Module>();
            CreateMap<ModuleUpdateDto, Module>();
            CreateMap<Module, ModuleDto>()
                .ForMember(dest => dest.ParentModuleName, opt => opt.MapFrom(src => src.ParentModule != null ? src.ParentModule.Name : null));
        }
    }
}
