using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Client.Pages.Generos
{
    public partial class CrearGenero
    {
        private Genero Genero = new Genero();

        private FormularioGenero? formGenero;

        public CrearGenero() { }

        private void Crear()
        {
            formGenero!.FormularioPosteadoConExito = true;
            Console.WriteLine("Ejecutando método Crear");
            Console.WriteLine($"Nombre del género: {Genero.Nombre}");
            navigationManager.NavigateTo("generos");
        }
    }
}
