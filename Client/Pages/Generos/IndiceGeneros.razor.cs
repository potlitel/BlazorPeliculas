using BlazorPeliculas.Shared.Entidades;
using CurrieTechnologies.Razor.SweetAlert2;

namespace BlazorPeliculas.Client.Pages.Generos
{
    public partial class IndiceGeneros
    {
        public List<Genero>? Generos { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await CargarDatos();
        }

        private async Task CargarDatos()
        {
            var respuestaHttp = await repo.Get<List<Genero>>("api/generos");
            Generos = respuestaHttp.Response!;
        }

        private async Task Borrar(Genero genero)
        {
            var resultado = await swal.FireAsync(
                new SweetAlertOptions
                {
                    Title = "Confirmación",
                    Text = $"Desea eliminar el Género {genero.Nombre}?",
                    ShowCancelButton = true,
                    Icon = SweetAlertIcon.Warning
                }
            );

            var confirmado = !string.IsNullOrEmpty(resultado.Value);

            if (confirmado)
            {
                var respuestaHttp = await repo.Delete($"api/generos/{genero.Id}");

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
                    await CargarDatos();
                }
            }
        }
    }
}