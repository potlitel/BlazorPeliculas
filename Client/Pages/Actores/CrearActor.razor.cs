using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Client.Pages.Actores
{
    public partial class CrearActor
    {
        private Actor Actor = new Actor();

        private FormularioActores? formActor;

        private void Crear()
        {
            formActor!.FormularioPosteadoConExito = true;
            Console.WriteLine("Ejecutando método Crear");
            Console.WriteLine($"Nombre del actor: {Actor.Nombre}");
            navigationManager.NavigateTo("actores");
        }
    }
}