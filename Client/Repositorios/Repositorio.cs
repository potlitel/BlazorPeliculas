using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Client.Repositorios
{
    public class Repositorio : IRepositorio
    {
        List<Pelicula> IRepositorio.ObtenerPeliculas()
        {
            return new List<Pelicula>()
            {
                new Pelicula
                {
                    Titulo = "<b>Wakanda Forever</b>",
                    FechaLanzamiento = new DateTime(2023, 9, 11)
                },
                new Pelicula
                {
                    Titulo = "<i>Moana</i>",
                    FechaLanzamiento = new DateTime(2016, 11, 13)
                },
                new Pelicula { Titulo = "Inception", FechaLanzamiento = new DateTime(2010, 7, 16) }
            };
        }
    }
}