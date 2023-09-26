using BlazorPeliculas.Shared.Entidades;
using CurrieTechnologies.Razor.SweetAlert2;

namespace BlazorPeliculas.Client.Pages.Peliculas
{
    public partial class CrearPelicula
    {
        public CrearPelicula()
        { }

        private Pelicula Pelicula = new Pelicula();

        private List<Genero> GenerosNoSeleccionados = new List<Genero>();

        private FormularioPeliculas? formPelicula;

        protected override void OnInitialized()
        {
            GenerosNoSeleccionados = new List<Genero>()
            {
                new Genero() { Id = 1, Nombre = "Comedia" },
                new Genero() { Id = 2, Nombre = "Drama" },
                new Genero() { Id = 3, Nombre = "Acción" },
                new Genero() { Id = 4, Nombre = "Sci-fi" },
            };
        }

        public async void Crear()
        {
            var responseHttp = await repositorio.Post<Pelicula, int>("/api/peliculas", Pelicula);
            if (responseHttp.Error)
            {
                var mensajeError = await responseHttp.ObtenerMensajeError();
                await swal.FireAsync("Error", mensajeError, SweetAlertIcon.Error);
            }
            else
            {
                var peliculaId = responseHttp.Response;
                navigationManager.NavigateTo(
                    $"/pelicula/{peliculaId}/{Pelicula.Titulo.Replace(" ", "-")}"
                );
            }
            //Console.WriteLine(navigationManager.BaseUri);
            //navigationManager.NavigateTo("pelicula");
        }
    }
}