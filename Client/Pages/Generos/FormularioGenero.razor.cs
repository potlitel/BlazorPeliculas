using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Pages.Generos
{
    public partial class FormularioGenero
    {
        [EditorRequired]
        [Parameter]
        public Genero Genero { get; set; } = null!;

        [EditorRequired]
        [Parameter]
        public EventCallback OnValidSubmit { get; set; }
    }
}