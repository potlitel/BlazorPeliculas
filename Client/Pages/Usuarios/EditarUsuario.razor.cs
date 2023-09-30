using BlazorPeliculas.Shared.DTOs;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Pages.Usuarios
{
    public partial class EditarUsuario
    {
        [Parameter]
        public string UsuarioId { get; set; } = null!;

        private List<RolDTO>? roles;
        private string rolSeleccionado = "0";

        protected override async Task OnInitializedAsync()
        {
            var httpResponse = await repo.Get<List<RolDTO>>("api/usuarios/roles");
            if (httpResponse.Error)
            {
                var msgError = await httpResponse.ObtenerMensajeError();
                await swal.FireAsync("Error", msgError, SweetAlertIcon.Error);
            }
            else
            {
                roles = httpResponse.Response;
            }
        }

        private async Task AsignarRol()
        {
            await EditarRol("api/usuarios/asignarRol");
        }

        private async Task RemoverRol()
        {
            await EditarRol("api/usuarios/removerRol");
        }

        private async Task EditarRol(string url)
        {
            if (rolSeleccionado == "0")
            {
                await swal.FireAsync("Error", "Debe seleccionar un Rol", SweetAlertIcon.Error);
                return;
            }

            var rolDTO = new EditarRolDTO() { Rol = rolSeleccionado, UsuarioId = UsuarioId };
            var httpResponse = await repo.Post<EditarRolDTO>(url, rolDTO);

            if (httpResponse.Error)
            {
                var msgError = await httpResponse.ObtenerMensajeError();
                await swal.FireAsync("Error", msgError, SweetAlertIcon.Success);
            }
            else
            {
                await swal.FireAsync(
                    "Exitoso",
                    "Operación realizada con éxito",
                    SweetAlertIcon.Error
                );
            }
        }
    }
}