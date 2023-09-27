using AutoMapper;
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
        private readonly IMapper mapper;
        private readonly string contenedor = "peliculas";

        public PeliculasController(
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

        [HttpGet("actualizar/{id:int}")]
        public async Task<ActionResult<PeliculaActualizacionDTO>> PutGet(int id)
        {
            var peliculaActionResult = await Get(id);

            if (peliculaActionResult.Result is NotFoundResult)
                return NotFound();

            var pelicula = peliculaActionResult.Value;
            var generosSeleccionadosIds = pelicula!.Generos.Select(g => g.Id).ToList();
            var generosNoSeleccionados = context.Generos
                .Where(gen => !generosSeleccionadosIds.Contains(gen.Id))
                .ToList();

            var modelo = new PeliculaActualizacionDTO
            {
                Pelicula = pelicula.Pelicula,
                GenerosNoSeleccionados = generosNoSeleccionados,
                GenerosSeleccionados = pelicula.Generos,
                Actores = pelicula.Actores,
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

            EscribirOrdenActores(peli);

            context.Add(peli);
            await context.SaveChangesAsync();
            return peli.Id;
        }

        private static void EscribirOrdenActores(Pelicula peli)
        {
            if (peli.PeliculasActor is not null)
            {
                for (int i = 0; i < peli.PeliculasActor.Count; i++)
                {
                    peli.PeliculasActor[i].Orden = i + 1;
                }
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(Pelicula peli)
        {
            var peliBD = await context.Peliculas
                .Include(p => p.PeliculasActor)
                .Include(p => p.GenerosPelicula)
                .FirstOrDefaultAsync(p => p.Id == peli.Id);

            if (peliBD is null)
            {
                return NotFound();
            }

            //Mapeamos todas las props de peli a peliBD menos la prop Poster
            peliBD = mapper.Map(peli, peliBD);

            if (!string.IsNullOrWhiteSpace(peli.Poster))
            {
                var peliActor = Convert.FromBase64String(peli.Poster);
                peli.Poster = await almacenadorArchivos.EditarArchivo(
                    peliActor,
                    ".jpg",
                    contenedor,
                    peliBD.Poster!
                );
            }

            EscribirOrdenActores(peliBD);

            context.Update(peliBD);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var peliBD = await context.Peliculas.FirstOrDefaultAsync(pel => pel.Id == id);

            if (peliBD is null)
            {
                return NotFound();
            }

            context.Remove(peliBD);
            await context.SaveChangesAsync();
            await almacenadorArchivos.EliminarArchivo(peliBD.Poster!, contenedor);

            return NoContent();
        }
    }
}