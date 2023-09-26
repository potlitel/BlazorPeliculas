using BlazorPeliculas.Shared.Entidades;
using System.Text;
using System.Text.Json;

namespace BlazorPeliculas.Client.Repositorios
{
    public class Repositorio : IRepositorio
    {
        private readonly HttpClient httpClient;

        public Repositorio(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        private JsonSerializerOptions OpcionesPorDefectoJSON =>
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            var respuestaHttp = await httpClient.GetAsync(url);
            if (respuestaHttp.IsSuccessStatusCode)
            {
                var respuesta = await DeserealizarRespuesta<T>(
                    respuestaHttp,
                    OpcionesPorDefectoJSON
                );
                return new HttpResponseWrapper<T>(respuesta, error: false, respuestaHttp);
            }
            else
            {
                return new HttpResponseWrapper<T>(default, error: true, respuestaHttp);
            }
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T enviar)
        {
            var enviarJson = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJson, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PostAsync(url, enviarContent);
            return new HttpResponseWrapper<object>(
                null,
                !responseHttp.IsSuccessStatusCode,
                responseHttp
            );
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T enviar)
        {
            var enviarJson = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJson, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PostAsync(url, enviarContent);

            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await DeserealizarRespuesta<TResponse>(
                    responseHttp,
                    OpcionesPorDefectoJSON
                );
                return new HttpResponseWrapper<TResponse>(response, error: false, responseHttp);
            }

            return new HttpResponseWrapper<TResponse>(
                default,
                !responseHttp.IsSuccessStatusCode,
                responseHttp
            );
        }

        private async Task<T> DeserealizarRespuesta<T>(
            HttpResponseMessage httpResponse,
            JsonSerializerOptions jsonSerializerOptions
        )
        {
            var respuestaString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(respuestaString, jsonSerializerOptions);
        }

        List<Pelicula> IRepositorio.ObtenerPeliculas()
        {
            return new List<Pelicula>()
            {
                new Pelicula
                {
                    Titulo = "<b>Wakanda Forever</b>",
                    FechaLanzamiento = new DateTime(2023, 9, 11),
                    Poster =
                        "https://upload.wikimedia.org/wikipedia/en/thumb/3/3b/Black_Panther_Wakanda_Forever_poster.jpg/220px-Black_Panther_Wakanda_Forever_poster.jpg"
                },
                new Pelicula
                {
                    Titulo = "<i>Moana</i>",
                    FechaLanzamiento = new DateTime(2016, 11, 13),
                    Poster =
                        "https://upload.wikimedia.org/wikipedia/en/thumb/2/26/Moana_Teaser_Poster.jpg/220px-Moana_Teaser_Poster.jpg"
                },
                new Pelicula
                {
                    Titulo = "Inception",
                    FechaLanzamiento = new DateTime(2010, 7, 16),
                    Poster =
                        "https://upload.wikimedia.org/wikipedia/en/2/2e/Inception_%282010%29_theatrical_poster.jpg"
                }
            };
        }
    }
}