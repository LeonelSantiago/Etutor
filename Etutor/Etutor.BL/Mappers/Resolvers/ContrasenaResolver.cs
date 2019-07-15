using AutoMapper;
using Microsoft.Extensions.Configuration;
using Etutor.BL.Dtos;
using Etutor.DataModel.Entities;

namespace Etutor.BL.Mappers.Resolvers
{
    public class ContrasenaResolver : IValueResolver<Usuario, UsuarioDto, string>
    {
        private IConfiguration _configuration;

        public ContrasenaResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Usuario source, UsuarioDto destination, string destMember, ResolutionContext context)
        {
            return _configuration.GetValue<string>("SymbolPasswordRepresentation");
        }
    }
}
