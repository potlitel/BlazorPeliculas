using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Pages.Peliculas
{
    public partial class EditarPelicula
    {
        [Parameter]
        public int PeliculaId { get; set; }

        public EditarPelicula() { }
    }
}
