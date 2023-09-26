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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PeliculaVisualizarDTO>> Get(int id)
        {
            var pelicula = await context.Peliculas
                .Where(peli => peli.Id == id)
                .Include(p => p.GenerosPelicula)
                .ThenInclude(pg => pg.Genero)
                .Include(p => p.PeliculasActor.OrderBy(pa => pa.Orden))
                .ThenInclude(pa => pa.Actor)
                .FirstOrDefaultAsync();

            if (pelicula is null)
            {
                return NotFound();
            }

            //TODO: sistema de votación
            var promedioVoto = 4;
            var votoUsuario = 5;

            var modelo = new PeliculaVisualizarDTO()
            {
                Pelicula = pelicula,
                Generos = pelicula.GenerosPelicula.Select(gp => gp.Genero!).ToList(),
                Actores = pelicula.PeliculasActor
                    .Select(
                        pa =>
                            new Actor
                            {
                                Id = pa.ActorId,
                                Nombre = pa.Actor!.Nombre,
                                Foto = pa.Actor.Foto,
                                Personaje = pa.Personaje
                            }
                    )
                    .ToList(),
                PromedioVotos = promedioVoto,
                VotoUsuario = votoUsuario
            };

            return modelo;
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

            if (peli.PeliculasActor is not null)
            {
                for (int i = 0; i < peli.PeliculasActor.Count; i++)
                {
                    peli.PeliculasActor[i].Orden = i + 1;
                }
            }

            context.Add(peli);
            await context.SaveChangesAsync();
            return peli.Id;
        }
    }
}