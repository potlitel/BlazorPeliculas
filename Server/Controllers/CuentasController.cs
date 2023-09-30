using BlazorPeliculas.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public CuentasController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration
        )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("crear")]
        public async Task<ActionResult<UserTokenDTO>> CreateUser([FromBody] UserInfo model)
        {
            var usuario = new IdentityUser { UserName = model.Email, Email = model.Email, };
            var resultado = await userManager.CreateAsync(usuario, model.Password);

            if (resultado.Succeeded)
            {
                return await BuildToken(model);
            }
            else
            {
                return BadRequest(resultado.Errors.First());
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserTokenDTO>> Login([FromBody] UserInfo model)
        {
            var resultado = await signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (resultado.Succeeded)
            {
                return await BuildToken(model);
            }
            else
            {
                return BadRequest("Intento de login fallido");
            }
        }

        private async Task<UserTokenDTO> BuildToken(UserInfo userinfo)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userinfo.Email),
                new Claim("mivalor", "lo que sea"),
            };

            var usuario = await userManager.FindByEmailAsync(userinfo.Email);
            var roles = await userManager.GetRolesAsync(usuario!);

            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtkey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new UserTokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
            };
        }
    }
}