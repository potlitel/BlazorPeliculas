using BlazorPeliculas.Shared.Entidades;
using CurrieTechnologies.Razor.SweetAlert2;

namespace BlazorPeliculas.Client.Pages.Actores
{
    public partial class IndiceActores
    {
        public List<Actor>? Actores { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await CargarDatos();
        }

        private async Task CargarDatos()
        {
            var respuestaHttp = await repo.Get<List<Actor>>("api/actores");
            Actores = respuestaHttp.Response!;
        }

        private async Task Borrar(Actor actor)
        {
            var resultado = await swal.FireAsync(
                new SweetAlertOptions
                {
                    Title = "Confirmación",
                    Text = $"Desea eliminar el actor {actor.Nombre}?",
                    ShowCancelButton = true,
                    Icon = SweetAlertIcon.Warning
                }
            );

            var confirmado = !string.IsNullOrEmpty(resultado.Value);

            if (confirmado)
            {
                var respuestaHttp = await repo.Delete($"api/actores/{actor.Id}");

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
                    await CargarDatos();
                }
            }
        }
    }
}