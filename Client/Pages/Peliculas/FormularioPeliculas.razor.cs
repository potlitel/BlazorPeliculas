using BlazorPeliculas.Client.Helpers;
using BlazorPeliculas.Shared.Entidades;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;

namespace BlazorPeliculas.Client.Pages.Peliculas
{
    public partial class FormularioPeliculas
    {
        private string? imgUrl;

        public FormularioPeliculas()
        { }

        [EditorRequired]
        [Parameter]
        public Pelicula Pelicula { get; set; } = null!;

        [EditorRequired]
        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public List<Genero> GenerosSeleccionados { get; set; } = new List<Genero>();

        [EditorRequired]
        [Parameter]
        public List<Genero> GenerosNoSeleccionados { get; set; } = new List<Genero>();

        private List<SelectorMultipleModel> Seleccionados { get; set; } =
            new List<SelectorMultipleModel>();

        private List<SelectorMultipleModel> NoSeleccionados { get; set; } =
            new List<SelectorMultipleModel>();

        private EditContext editContext = null!;

        public bool FormularioPosteadoConExito { get; set; } = false;

        protected override void OnInitialized()
        {
            //editContext = new EditContext(Actor);
            if (!string.IsNullOrEmpty(Pelicula.Poster))
            {
                imgUrl = Pelicula.Poster;
                Pelicula.Poster = null;
            }

            Seleccionados = GenerosSeleccionados
                .Select(item => new SelectorMultipleModel(item.Id.ToString(), item.Nombre))
                .ToList();

            NoSeleccionados = GenerosNoSeleccionados
                .Select(item => new SelectorMultipleModel(item.Id.ToString(), item.Nombre))
                .ToList();
        }

        private void ImgSeleccionada(string imagenBase64)
        {
            Pelicula.Poster = imagenBase64;
            imgUrl = null;
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