using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repostories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        //POST api/Auth/Register
        [HttpPost("Register")]

        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username

            };
            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                //Add roles to this user
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was Registered! Please Log in.");
                    }
                }

            }


            return BadRequest("Something went wrong!");

        }

        //POST api/Auth/Login

        [HttpPost("Login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {

            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
                var checkpasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkpasswordResult)
                {

                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        //Create token
                       var jwtToken =  tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDto { 

                            JwtToken  = jwtToken    
                        };

                        return Ok(response);
                    }



                    
                }
            }

            return BadRequest("Username or Pasword incorrect!!");
        }
    }
}
