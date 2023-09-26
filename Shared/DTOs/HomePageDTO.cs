using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Shared.DTOs
{
    public class HomePageDTO
    {
        public List<Pelicula>? PeliculasEnCartelera { get; set; }
        public List<Pelicula>? ProximosEstrenos { get; set; }
    }
}