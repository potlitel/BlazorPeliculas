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

        private List<Genero> GenerosNoSeleccionados = new List<Genero>();
        private List<Genero> GenerosSeleccionados = new List<Genero>();

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
            GenerosNoSeleccionados = new List<Genero>()
            {
                new Genero() { Id = 1, Nombre = "Comedia" },
                new Genero() { Id = 3, Nombre = "Acción" },
                new Genero() { Id = 4, Nombre = "Sci-fi" },
            };

            GenerosSeleccionados = new List<Genero>()
            {
                new Genero() { Id = 2, Nombre = "Drama" }
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