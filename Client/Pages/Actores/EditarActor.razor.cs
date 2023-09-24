using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Pages.Actores
{
    public partial class EditarActor
    {
        [Parameter]
        public int ActorId { get; set; }

        public EditarActor()
        { }

        private FormularioActores? formActor;

        private Actor? Actor;

        protected override void OnInitialized()
        {
            Actor = new Actor()
            {
                Id = ActorId,
                Nombre = "Alain Jorge Acuña",
                FechaNacimiento = DateTime.Today
            };
        }

        private void Editar()
        {
            formActor!.FormularioPosteadoConExito = true;
            Console.WriteLine("Editando Actor de Película");
            Console.WriteLine($"Id: {Actor!.Id}");
            Console.WriteLine($"Nombre: {Actor!.Nombre}");
            navigationManager.NavigateTo("generos");
        }
    }
}