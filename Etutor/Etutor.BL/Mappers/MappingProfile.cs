using AutoMapper;
using Etutor.BL.Dtos;
using Etutor.BL.Mappers.Resolvers;
using System;
using Etutor.DataModel.Entities;

namespace Etutor.BL.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Usuario, UsuarioDto>()
            .ForMember(dto => dto.Contrasena, cfg => cfg.MapFrom<ContrasenaResolver>())
            .ReverseMap()
            .ForMember(entity => entity.PhoneNumberConfirmed, cfg => cfg.Ignore())
            .ForMember(entity => entity.UserName, cfg => cfg.MapFrom(dto => dto.Email.Split("@", StringSplitOptions.RemoveEmptyEntries)[0] ?? null));

        }
    }
}

