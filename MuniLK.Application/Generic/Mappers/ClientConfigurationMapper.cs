using AutoMapper;
using MuniLK.Application.Generic.DTOs;
using MuniLK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Generic.Mappers
{
    public class ClientConfigurationMapper : Profile
    {
        public ClientConfigurationMapper()
        {
            CreateMap<ClientConfigurationCreateDto, ClientConfiguration>()
                 .ForMember(dest => dest.ConfigJson,
                            opt => opt.MapFrom(src => src.ConfigJson.GetRawText())); // ✅ Converts JsonElement to string
            CreateMap<ClientConfigurationUpdateDto, ClientConfiguration>();
            CreateMap<ClientConfiguration, ClientConfigurationDto>();
        }
    }
}
