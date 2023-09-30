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

        private async Task OnRating(int votoSeleccionado)
        {
            modelo!.VotoUsuario = votoSeleccionado;
            var votoPeliculaDTO = new VotoPeliculaDTO()
            {
                PeliculaId = PeliculaId,
                Voto = votoSeleccionado
            };
            var responseHttp = await repo.Post("api/votos", votoPeliculaDTO);

            if (responseHttp.Error)
            {
                var msgError = await responseHttp.ObtenerMensajeError();
                await swal.FireAsync("Error", msgError, SweetAlertIcon.Error);
            }
            else
            {
                await swal.FireAsync("Exitos", "Su voto ha sido registrado", SweetAlertIcon.Info);
            }
        }
    }
}