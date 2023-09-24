using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Client.Pages.Generos
{
    public partial class CrearGenero
    {
        private Genero Genero = new Genero();

        public CrearGenero() { }

        private void Crear()
        {
            Console.WriteLine("Ejecutando método Crear");
            Console.WriteLine($"Nombre del género: {Genero.Nombre}");
        }
    }
}
