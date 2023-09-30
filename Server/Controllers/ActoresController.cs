using AutoMapper;
using BlazorPeliculas.Server.Helpers;
using BlazorPeliculas.Shared.DTOs;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/actores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IMapper mapper;
        private readonly string contenedor = "personas";

        public ActoresController(
            ApplicationDBContext context,
            IAlmacenadorArchivos almacenadorArchivos,
            IMapper mapper
        )
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> Get(
            [FromQuery] PaginacionDTO paginacion
        )
        {
            var queryable = context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(
                queryable,
                paginacion.CantidadRegistros
            );

            return await queryable.OrderBy(x => x.Nombre).Paginar(paginacion).ToListAsync();
            //return await context.Actores.ToListAsync();
        }

        [HttpGet("buscar/{textoBusqueda}")]
        public async Task<ActionResult<List<Actor>>> Get(string textoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda))
            {
                return new List<Actor>();
            }
            textoBusqueda = textoBusqueda.ToLower();
            return await context.Actores
                .Where(item => item.Nombre.ToLower().Contains(textoBusqueda))
                .Take(5)
                .ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Actor>> Get(int id)
        {
            var actor = await context.Actores.FirstOrDefaultAsync(actor => actor.Id == id);

            if (actor is null)
            {
                return NotFound();
            }

            return actor;
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

        [HttpPut]
        public async Task<ActionResult> Put(Actor actor)
        {
            var actorBD = await context.Actores.FirstOrDefaultAsync(a => a.Id == actor.Id);

            if (actorBD is null)
            {
                return NotFound();
            }

            //Mapeamos todas las props de actor a actorBD menos la prop Foto
            actorBD = mapper.Map(actor, actorBD);

            if (!string.IsNullOrWhiteSpace(actor.Foto))
            {
                var fotoActor = Convert.FromBase64String(actor.Foto);
                actor.Foto = await almacenadorArchivos.EditarArchivo(
                    fotoActor,
                    ".jpg",
                    contenedor,
                    actorBD.Foto!
                );
            }

            context.Update(actorBD);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var actorBD = await context.Actores.FirstOrDefaultAsync(act => act.Id == id);

            if (actorBD is null)
            {
                return NotFound();
            }

            context.Remove(actorBD);
            await context.SaveChangesAsync();
            await almacenadorArchivos.EliminarArchivo(actorBD.Foto!, contenedor);

            return NoContent();
        }
    }
}
