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

        public bool MostrarFormulario { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            var respuestaHttp = await repositorio.Get<List<Genero>>("api/generos");
            GenerosNoSeleccionados = respuestaHttp.Response!;
            MostrarFormulario = true;
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