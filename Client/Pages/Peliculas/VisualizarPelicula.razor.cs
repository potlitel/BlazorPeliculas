using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Pages.Peliculas
{
    public partial class VisualizarPelicula
    {
        [Parameter]
        public int PeliculaId { get; set; }

        [Parameter]
        public string NombrePelicula { get; set; }

        public VisualizarPelicula() { }

        protected override void OnInitialized()
        {
            Console.WriteLine($"El id de la peli es {PeliculaId}");
        }
    }
}
