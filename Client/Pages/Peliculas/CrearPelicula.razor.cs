using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Client.Pages.Peliculas
{
    public partial class CrearPelicula
    {
        public CrearPelicula()
        { }

        private Pelicula Pelicula = new Pelicula();

        private FormularioPeliculas? formPelicula;

        public void Crear()
        {
            Console.WriteLine(navigationManager.BaseUri);
            navigationManager.NavigateTo("pelicula");
        }
    }
}