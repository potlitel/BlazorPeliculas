namespace BlazorPeliculas.Client.Pages.Peliculas
{
    public partial class CrearPelicula
    {
        public CrearPelicula()
        { }

        public void Crear()
        {
            Console.WriteLine(navigationManager.BaseUri);
            navigationManager.NavigateTo("pelicula");
        }
    }
}