using BlazorPeliculas.Shared.DTOs;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Pages.Peliculas
{
    public partial class VisualizarPelicula
    {
        [Parameter]
        public int PeliculaId { get; set; }

        [Parameter]
        public string? NombrePelicula { get; set; }

        private PeliculaVisualizarDTO? modelo;

        protected override async Task OnInitializedAsync()
        {
            //Console.WriteLine($"El id de la peli es {PeliculaId}");
            var httpResponse = await repo.Get<PeliculaVisualizarDTO>($"api/peliculas/{PeliculaId}");

            if (httpResponse.Error)
            {
                var msgError = await httpResponse.ObtenerMensajeError();
                await swal.FireAsync("Error", msgError, SweetAlertIcon.Error);
            }
            else
            {
                modelo = httpResponse.Response;
            }
        }
    }
}