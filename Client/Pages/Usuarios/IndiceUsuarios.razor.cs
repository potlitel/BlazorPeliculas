using BlazorPeliculas.Shared.DTOs;
using CurrieTechnologies.Razor.SweetAlert2;

namespace BlazorPeliculas.Client.Pages.Usuarios
{
    public partial class IndiceUsuarios
    {
        private List<UsuarioDTO>? Usuarios;
        public int paginaActual = 1;
        public int paginasTotales;

        protected override async Task OnInitializedAsync()
        {
            await Cargar();
        }

        private async Task Cargar(int pagina = 1)
        {
            var responseHttp = await repo.Get<List<UsuarioDTO>>($"api/usuarios?pagina={pagina}");
            if (responseHttp.Error)
            {
                var msgError = await responseHttp.ObtenerMensajeError();
                await swal.FireAsync("Error", msgError, SweetAlertIcon.Error);
            }
            else
            {
                paginasTotales = int.Parse(
                    responseHttp.HttpResponseMessage.Headers
                        .GetValues("totalPaginas")
                        .FirstOrDefault()!
                );
                Usuarios = responseHttp.Response;
            }
        }

        private async Task paginaSeleccionada(int pagina)
        {
            paginaActual = pagina;
            await Cargar(pagina);
        }
    }
}