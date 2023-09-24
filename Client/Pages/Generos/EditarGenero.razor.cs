using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Pages.Generos
{
    public partial class EditarGenero
    {
        [Parameter]
        public int GeneroId { get; set; }

        public EditarGenero() { }

        private FormularioGenero? formGenero;

        private Genero? Genero;

        protected override void OnInitialized()
        {
            Genero = new Genero() { Id = GeneroId, Nombre = "Comedia" };
        }

        private void Editar()
        {
            formGenero!.FormularioPosteadoConExito = true;
            Console.WriteLine("Editando Género de Película");
            Console.WriteLine($"Id: {Genero!.Id}");
            Console.WriteLine($"Nombre: {Genero!.Nombre}");
            navigationManager.NavigateTo("generos");
        }
    }
}
