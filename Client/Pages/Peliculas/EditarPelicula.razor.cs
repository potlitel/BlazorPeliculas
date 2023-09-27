using BlazorPeliculas.Shared.DTOs;
using BlazorPeliculas.Shared.Entidades;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Pages.Peliculas
{
    public partial class EditarPelicula
    {
        [Parameter]
        public int PeliculaId { get; set; }

        private FormularioPeliculas? formPelicula;

        private Pelicula? Pelicula;
        private List<Genero> GenerosNoSeleccionados = new List<Genero>();
        private List<Genero> GenerosSeleccionados = new List<Genero>();
        private List<Actor> ActoresSeleccionados = new List<Actor>();

        public EditarPelicula()
        { }

        protected override async Task OnInitializedAsync()
        {
            var responseHttp = await repo.Get<PeliculaActualizacionDTO>(
                $"api/peliculas/actualizar/{PeliculaId}"
            );
            if (responseHttp.Error)
            {
                if (
                    responseHttp.HttpResponseMessage.StatusCode
                    == System.Net.HttpStatusCode.NotFound
                )
                {
                    navigationManager.NavigateTo("/");
                }
                else
                {
                    var msgError = await responseHttp.ObtenerMensajeError();
                    await swal.FireAsync("Error", msgError, SweetAlertIcon.Error);
                }
            }
            else
            {
                var modelo = responseHttp.Response!;
                ActoresSeleccionados = modelo.Actores;
                GenerosNoSeleccionados = modelo.GenerosNoSeleccionados;
                GenerosSeleccionados = modelo.GenerosSeleccionados;
                Pelicula = modelo.Pelicula;
            }
        }

        private async Task Editar()
        {
            var respuestaHttp = await repo.Put("api/peliculas", Pelicula);
            if (respuestaHttp.Error)
            {
                var msgError = await respuestaHttp.ObtenerMensajeError();
                await swal.FireAsync("Error", msgError, SweetAlertIcon.Error);
            }
            else
            {
                //formGenero!.FormularioPosteadoConExito = true;
                navigationManager.NavigateTo($"pelicula/{PeliculaId}");
            }
            //formPelicula!.FormularioPosteadoConExito = true;
            //Console.WriteLine("Editando Actor de Película");
            //Console.WriteLine($"Id: {Pelicula!.Id}");
            //Console.WriteLine($"Titulo: {Pelicula!.Titulo}");
            //navigationManager.NavigateTo("generos");
        }
    }
}