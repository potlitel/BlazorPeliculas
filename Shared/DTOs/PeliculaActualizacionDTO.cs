using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Shared.DTOs
{
    public class PeliculaActualizacionDTO
    {
        public Pelicula Pelicula { get; set; } = null!;
        public List<Actor> Actores { get; set; } = new List<Actor>();
        public List<Genero> GenerosSeleccionados { get; set; } = new List<Genero>();
        public List<Genero> GenerosNoSeleccionados { get; set; } = new List<Genero>();
    }
}