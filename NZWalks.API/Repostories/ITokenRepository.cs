using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace NZWalks.API.Repostories
{
    public interface ITokenRepository
    {

        string CreateJWTToken(IdentityUser user, List<string> roles);
        
    }
}
