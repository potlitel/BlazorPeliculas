using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Pages.Generos
{
    public partial class EditarGenero
    {
        [Parameter]
        public int GeneroId { get; set; }

        public EditarGenero() { }
    }
}
