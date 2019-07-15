using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace Etutor.Services.Interfaces
{
    public interface ITokenGeneratorService
    {
        object GenerateJwtToken(string givenName, IdentityUser<int> user, List<Claim> userClaims = null);
    }
}
