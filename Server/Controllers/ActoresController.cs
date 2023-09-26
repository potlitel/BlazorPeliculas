using BlazorPeliculas.Server.Helpers;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "personas";

        public ActoresController(
            ApplicationDBContext context,
            IAlmacenadorArchivos almacenadorArchivos
        )
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> Get()
        {
            return await context.Actores.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Actor actor)
        {
            if (!string.IsNullOrWhiteSpace(actor.Foto))
            {
                var fotoActor = Convert.FromBase64String(actor.Foto);
                actor.Foto = await almacenadorArchivos.GuardarArchivo(
                    fotoActor,
                    ".jpg",
                    contenedor
                );
            }
            context.Add(actor);
            await context.SaveChangesAsync();
            return actor.Id;
        }
    }
}