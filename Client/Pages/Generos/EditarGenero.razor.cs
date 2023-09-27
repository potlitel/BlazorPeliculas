using BlazorPeliculas.Shared.Entidades;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Pages.Generos
{
    public partial class EditarGenero
    {
        [Parameter]
        public int GeneroId { get; set; }

        public EditarGenero()
        { }

        private FormularioGenero? formGenero;

        private Genero? Genero;

        protected override async Task OnInitializedAsync()
        {
            //Genero = new Genero() { Id = GeneroId, Nombre = "Comedia" };
            var respuestaHttp = await repo.Get<Genero>($"api/generos/{GeneroId}");
            if (respuestaHttp.Error)
            {
                if (
                    respuestaHttp.HttpResponseMessage.StatusCode
                    == System.Net.HttpStatusCode.NotFound
                )
                {
                    navigationManager.NavigateTo("generos");
                }
                else
                {
                    var msgError = await respuestaHttp.ObtenerMensajeError();
                    await swal.FireAsync("Error", msgError, SweetAlertIcon.Error);
                }
            }
            else
            {
                Genero = respuestaHttp.Response;
            }
        }

        private async void Editar()
        {
            var respuestaHttp = await repo.Put("api/generos", Genero);
            if (respuestaHttp.Error)
            {
                var msgError = await respuestaHttp.ObtenerMensajeError();
                await swal.FireAsync("Error", msgError, SweetAlertIcon.Error);
            }
            else
            {
                formGenero!.FormularioPosteadoConExito = true;
                navigationManager.NavigateTo("generos");
            }
            //formGenero!.FormularioPosteadoConExito = true;
            //Console.WriteLine("Editando Género de Película");
            //Console.WriteLine($"Id: {Genero!.Id}");
            //Console.WriteLine($"Nombre: {Genero!.Nombre}");
            //navigationManager.NavigateTo("generos");
        }
    }
}