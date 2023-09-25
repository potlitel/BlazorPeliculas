using BlazorPeliculas.Shared.Entidades;
using CurrieTechnologies.Razor.SweetAlert2;

namespace BlazorPeliculas.Client.Pages.Actores
{
    public partial class CrearActor
    {
        private Actor Actor = new Actor();

        private FormularioActores? formActor;

        private async Task Crear()
        {
            var httpResponse = await repositorio.Post("api/actores", Actor);

            if (httpResponse.Error)
            {
                var msgError = await httpResponse.ObtenerMensajeError();
                await swal.FireAsync("Error", msgError, SweetAlertIcon.Error);
            }
            else
            {
                formActor!.FormularioPosteadoConExito = true;
                Console.WriteLine("Ejecutando método Crear");
                Console.WriteLine($"Nombre del actor: {Actor.Nombre}");
                navigationManager.NavigateTo("actores");
            }
        }
    }
}