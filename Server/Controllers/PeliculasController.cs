using BlazorPeliculas.Server.Helpers;
using BlazorPeliculas.Shared.DTOs;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public async Task<ActionResult<HomePageDTO>> Get()
        {
            var limite = 6;

            var peliculasEnCartelera = await context.Peliculas
                .Where(peli => peli.EnCartelera)
                .Take(limite)
                .OrderByDescending(peli => peli.FechaLanzamiento)
                .ToListAsync();

            var fechaActual = DateTime.Today;

            var proximosEstrenos = await context.Peliculas
                .Where(peli => peli.FechaLanzamiento > fechaActual)
                .OrderBy(peli => peli.FechaLanzamiento)
                .Take(limite)
                .ToListAsync();

            var result = new HomePageDTO()
            {
                PeliculasEnCartelera = peliculasEnCartelera,
                ProximosEstrenos = proximosEstrenos,
            };

            return result;
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