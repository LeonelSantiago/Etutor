using AutoMapper;
using Microsoft.Extensions.Configuration;
using Etutor.BL.Dtos;
using Etutor.DataModel.Entities;

namespace Etutor.BL.Mappers.Resolvers
{
    public class ContrasenaResolver : IValueResolver<User, UserDto, string>
    {
        private IConfiguration _configuration;

        public ContrasenaResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(User source, UserDto destination, string destMember, ResolutionContext context)
        {
            return _configuration.GetValue<string>("SymbolPasswordRepresentation");
        }
    }
}
