using BlazorPeliculas.Shared.Entidades;

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

        public void Crear()
        {
            Console.WriteLine(navigationManager.BaseUri);
            navigationManager.NavigateTo("pelicula");
        }
    }
}