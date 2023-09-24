using BlazorPeliculas.Shared.Entidades;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;

namespace BlazorPeliculas.Client.Pages.Actores
{
    public partial class FormularioActores
    {
        private EditContext editContext = null!;

        [EditorRequired]
        [Parameter]
        public Actor Actor { get; set; } = null!;

        [EditorRequired]
        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        public bool FormularioPosteadoConExito { get; set; } = false;

        protected override void OnInitialized()
        {
            editContext = new EditContext(Actor);
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

            //var confirmado = await js.Confirm("Desea salir del form y perder los cambios?");
            var resultado = await swal.FireAsync(
                new SweetAlertOptions
                {
                    Title = "Confirmación",
                    Text = "Desea salir del form y perder los cambios?",
                    ShowCancelButton = true,
                    Icon = SweetAlertIcon.Warning
                }
            );

            var confirmado = !string.IsNullOrEmpty(resultado.Value);

            if (confirmado)
            {
                return;
            }

            context.PreventNavigation();
        }
    }
}