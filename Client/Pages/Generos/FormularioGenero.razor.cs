using BlazorPeliculas.Client.Helpers;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;

namespace BlazorPeliculas.Client.Pages.Generos
{
    public partial class FormularioGenero
    {
        private EditContext editContext = null!;

        [EditorRequired]
        [Parameter]
        public Genero Genero { get; set; } = null!;

        [EditorRequired]
        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        public bool FormularioPosteadoConExito { get; set; } = false;

        protected override void OnInitialized()
        {
            editContext = new EditContext(Genero);
        }

        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {
            var formEditado = editContext.IsModified();

            if (!formEditado)
            {
                return;
            }

            if (FormularioPosteadoConExito)
            {
                return;
            }

            var confirmado = await js.Confirm("Desea salir del form y perder los cambios?");

            if (confirmado)
            {
                return;
            }

            context.PreventNavigation();
        }
    }
}