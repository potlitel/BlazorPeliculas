using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Client.Repositorios
{
    public interface IRepositorio
    {
        public List<Pelicula> ObtenerPeliculas();

        Task<HttpResponseWrapper<object>> Post<T>(string url, T enviar);

        Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T enviar);
    }
}