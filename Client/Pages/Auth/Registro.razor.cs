using BlazorPeliculas.Shared.DTOs;
using CurrieTechnologies.Razor.SweetAlert2;

namespace BlazorPeliculas.Client.Pages.Auth
{
    public partial class Registro
    {
        private UserInfo userInfo = new UserInfo();

        private async Task CrearUsuario()
        {
            var httpResponse = await repo.Post<UserInfo, UserTokenDTO>(
                "api/cuentas/crear",
                userInfo
            );

            if (httpResponse.Error)
            {
                var msgError = await httpResponse.ObtenerMensajeError();
                await swal.FireAsync("Error", msgError, SweetAlertIcon.Error);
            }
            else
            {
                await loginService.Login(httpResponse.Response!.Token);
                navigationManager.NavigateTo("");
            }
        }
    }
}