using BlazorPeliculas.Shared.Entidades;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Pages.Actores
{
    public partial class EditarActor
    {
        [Parameter]
        public int ActorId { get; set; }

        public EditarActor() { }

        private FormularioActores? formActor;

        private Actor? Actor;

        protected override async Task OnInitializedAsync()
        {
            var respuestaHttp = await repo.Get<Actor>($"api/actores/{ActorId}");
            if (respuestaHttp.Error)
            {
                if (
                    respuestaHttp.HttpResponseMessage.StatusCode
                    == System.Net.HttpStatusCode.NotFound
                )
                {
                    navigationManager.NavigateTo("actores");
                }
                else
                {
                    var msgError = await respuestaHttp.ObtenerMensajeError();
                    await swal.FireAsync("Error", msgError, SweetAlertIcon.Error);
                }
            }
            else
            {
                Actor = respuestaHttp.Response;
            }
            //Actor = new Actor()
            //{
            //    Id = ActorId,
            //    Nombre = "Alain Jorge Acuña",
            //    FechaNacimiento = DateTime.Today
            //};
        }

        private async Task Editar()
        {
            var respuestaHttp = await repo.Put("api/actores", Actor);
            if (respuestaHttp.Error)
            {
                var msgError = await respuestaHttp.ObtenerMensajeError();
                await swal.FireAsync("Error", msgError, SweetAlertIcon.Error);
            }
            else
            {
                //formGenero!.FormularioPosteadoConExito = true;
                navigationManager.NavigateTo("actores");
            }
            //formActor!.FormularioPosteadoConExito = true;
            //Console.WriteLine("Editando Actor de Película");
            //Console.WriteLine($"Id: {Actor!.Id}");
            //Console.WriteLine($"Nombre: {Actor!.Nombre}");
            //navigationManager.NavigateTo("generos");
        }
    }
}
