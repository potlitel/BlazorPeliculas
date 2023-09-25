using BlazorPeliculas.Shared.Entidades;
using CurrieTechnologies.Razor.SweetAlert2;

namespace BlazorPeliculas.Client.Pages.Generos
{
    public partial class CrearGenero
    {
        private Genero Genero = new Genero();

        private FormularioGenero? formGenero;

        public CrearGenero()
        { }

        private async Task Crear()
        {
            var httpResponse = await repositorio.Post("api/generos", Genero);

            if (httpResponse.Error)
            {
                var msgError = await httpResponse.ObtenerMensajeError();
                await swal.FireAsync("Error", msgError, SweetAlertIcon.Error);
            }
            else
            {
                formGenero!.FormularioPosteadoConExito = true;
                Console.WriteLine("Ejecutando método Crear");
                Console.WriteLine($"Nombre del género: {Genero.Nombre}");
                navigationManager.NavigateTo("generos");
            }
        }
    }
}