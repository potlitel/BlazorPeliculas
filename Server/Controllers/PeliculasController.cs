using AutoMapper;
using BlazorPeliculas.Server.Helpers;
using BlazorPeliculas.Shared.DTOs;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly string contenedor = "peliculas";

        public PeliculasController(
            ApplicationDBContext context,
            IAlmacenadorArchivos almacenadorArchivos,
            IMapper mapper,
            UserManager<IdentityUser> userManager
        )
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
            var promedioVoto = 0.0;
            var votoUsuario = 0;

            if (await context.VotosPeliculas.AnyAsync(v => v.PeliculaId == id))
            {
                promedioVoto = await context.VotosPeliculas
                    .Where(vot => vot.PeliculaId == id)
                    .AverageAsync(vot => vot.Voto);
                if (HttpContext.User.Identity!.IsAuthenticated)
                {
                    var usuario = await userManager.FindByEmailAsync(
                        HttpContext.User.Identity!.Name!
                    );

                    if (usuario is null)
                    {
                        return BadRequest("Usuario no encontrado");
                    }

                    var usuarioId = usuario.Id;

                    var votoUsuarioDB = await context.VotosPeliculas.FirstOrDefaultAsync(
                        voto => voto.PeliculaId == id && voto.UsuarioId == usuarioId
                    );

                    if (votoUsuarioDB is not null)
                    {
                        votoUsuario = votoUsuarioDB.Voto;
                    }
                }
            }

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

        [HttpGet("filtrar")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Pelicula>>> Get(
            [FromQuery] ParametrosBusquedaPeliculasDTO modelo
        )
        {
            //Modelo de ejecución diferida, se arma el query dinamicamente en dependencia de la selección del usuario
            var peliculasQueryable = context.Peliculas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(modelo.Titulo))
                peliculasQueryable = peliculasQueryable.Where(
                    pel => pel.Titulo.Contains(modelo.Titulo)
                );

            if (modelo.EnCartelera)
                peliculasQueryable = peliculasQueryable.Where(pel => pel.EnCartelera);

            if (modelo.Estrenos)
            {
                var hoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable.Where(pel => pel.FechaLanzamiento > hoy);
            }

            if (modelo.GeneroID != 0)
                peliculasQueryable = peliculasQueryable.Where(
                    pel => pel.GenerosPelicula.Select(gen => gen.GeneroId).Contains(modelo.GeneroID)
                );

            if (modelo.MasVotadas)
            {
                peliculasQueryable = peliculasQueryable.OrderByDescending(
                    p => p.VotosPeliculas.Average(vp => vp.Voto)
                );
            }

            await HttpContext.InsertarParametrosPaginacionEnRespuesta(
                peliculasQueryable,
                modelo.CantidadRegistros
            );
            var peliculas = await peliculasQueryable.Paginar(modelo.paginacionDTO).ToListAsync();

            return peliculas;
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