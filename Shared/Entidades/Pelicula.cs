using System.ComponentModel.DataAnnotations;

namespace BlazorPeliculas.Shared.Entidades
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;

        [Required]
        public string? Resumen { get; set; }

        public bool EnCartelera { get; set; }
        public string? Trailer { get; set; }
        public DateTime FechaLanzamiento { get; set; }

        public string? Poster { get; set; }

        public List<GeneroPelicula> GenerosPelicula { get; set; } = new List<GeneroPelicula>();

        public List<PeliculaActor> PeliculasActor { get; set; } = new List<PeliculaActor>();

        public string? TituloCortado
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Titulo))
                {
                    return null;
                }

                if (Titulo.Length > 60)
                {
                    return Titulo.Substring(0, 60) + "...";
                }
                else
                {
                    return Titulo;
                }
            }
        }
    }
}