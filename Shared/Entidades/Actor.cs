using System.ComponentModel.DataAnnotations;

namespace BlazorPeliculas.Shared.Entidades
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;

        public string? Biografia { get; set; }
        public string? Foto { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}