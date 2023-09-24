using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorPeliculas.Client.Pages.Peliculas
{
    public partial class FiltroPeliculas
    {
        private string titulo = "";
        private string generoSeleccionado = "0";
        private List<Genero> generos = new List<Genero>();
        private bool futurosEstrenos = false;
        private bool enCartelera = false;
        private bool masVotadas = false;
        private List<Pelicula>? peliculas;

        private void TituloKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                CargarNuevasPeliculas();
            }
        }

        protected override void OnInitialized()
        {
            peliculas = repositorio.ObtenerPeliculas();
        }

        private void CargarNuevasPeliculas()
        {
            peliculas = repositorio
                .ObtenerPeliculas()
                .Where(p => p.Titulo.ToLower().Contains(titulo.ToLower()))
                .ToList();
            Console.WriteLine($"Titulo {titulo}");
            Console.WriteLine($"Genero seleccionado {generoSeleccionado}");
            Console.WriteLine($"En cartelera {enCartelera}");
            Console.WriteLine($"Futuros estrenos {futurosEstrenos}");
            Console.WriteLine($"Más votadas {masVotadas}");
        }

        private void LimpiarCampos()
        {
            peliculas = repositorio.ObtenerPeliculas();
            titulo = "";
            generoSeleccionado = "0";
            futurosEstrenos = false;
            enCartelera = false;
            masVotadas = false;
        }
    }
}
