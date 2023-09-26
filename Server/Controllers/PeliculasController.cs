using BlazorPeliculas.Server.Helpers;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas";

        public PeliculasController(
            ApplicationDBContext context,
            IAlmacenadorArchivos almacenadorArchivos
        )
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Pelicula peli)
        {
            if (!string.IsNullOrWhiteSpace(peli.Poster))
            {
                var peliPoster = Convert.FromBase64String(peli.Poster);
                peli.Poster = await almacenadorArchivos.GuardarArchivo(
                    peliPoster,
                    ".jpg",
                    contenedor
                );
            }
            context.Add(peli);
            await context.SaveChangesAsync();
            return peli.Id;
        }
    }
}