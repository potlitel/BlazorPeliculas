using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Pages.Peliculas
{
    public partial class EditarPelicula
    {
        [Parameter]
        public int PeliculaId { get; set; }

        private FormularioPeliculas? formPelicula;

        private Pelicula? Pelicula;

        public EditarPelicula()
        { }

        protected override void OnInitialized()
        {
            Pelicula = new Pelicula()
            {
                Id = PeliculaId,
                //Nombre = "Alain Jorge Acuña",
                //FechaNacimiento = DateTime.Today
            };
        }

        private void Editar()
        {
            formPelicula!.FormularioPosteadoConExito = true;
            Console.WriteLine("Editando Actor de Película");
            Console.WriteLine($"Id: {Pelicula!.Id}");
            Console.WriteLine($"Titulo: {Pelicula!.Titulo}");
            navigationManager.NavigateTo("generos");
        }
    }
}