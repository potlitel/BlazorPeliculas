using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/generos")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenerosController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public GenerosController(ApplicationDBContext context)
        {
            this.context = context;
        }

        [HttpGet("api/generos")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Genero>>> Get()
        {
            return await context.Generos.ToListAsync();
        }

        [HttpGet("api/generos/{id:int}")]
        public async Task<ActionResult<Genero>> Get(int id)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(genero => genero.Id == id);

            if (genero is null)
            {
                return NotFound();
            }

            return genero;
        }

        [HttpPost("api/generos")]
        public async Task<ActionResult<int>> Post(Genero genero)
        {
            context.Add(genero);
            await context.SaveChangesAsync();
            return genero.Id;
        }

        [HttpPut("api/generos")]
        public async Task<ActionResult> Put(Genero genero)
        {
            context.Update(genero);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("api/generos/{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var filasAfectadas = await context.Generos
                .Where(gen => gen.Id == id)
                .ExecuteDeleteAsync();

            if (filasAfectadas == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
