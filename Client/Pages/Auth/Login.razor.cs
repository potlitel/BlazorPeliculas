using BlazorPeliculas.Shared.DTOs;
using CurrieTechnologies.Razor.SweetAlert2;

namespace BlazorPeliculas.Client.Pages.Auth
{
    public partial class Login
    {
        private UserInfo userInfo = new UserInfo();

        private async Task LoguearUsuario()
        {
            var httpResponse = await repo.Post<UserInfo, UserTokenDTO>(
                "api/cuentas/Login",
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