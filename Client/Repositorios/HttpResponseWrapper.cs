namespace BlazorPeliculas.Client.Repositorios
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Response = response;
            Error = error;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Error { get; set; }
        public T? Response { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
    }
}