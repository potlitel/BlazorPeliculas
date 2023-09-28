using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorPeliculas.Client.Pages.Peliculas
{
    public partial class FiltroPeliculas
    {
        [Parameter]
        [SupplyParameterFromQuery]
        public string titulo { get; set; } = "";

        [Parameter]
        [SupplyParameterFromQuery(Name = "generoid")]
        public int generoSeleccionado { get; set; }

        private List<Genero> generos = new List<Genero>();

        [Parameter]
        [SupplyParameterFromQuery(Name = "estrenos")]
        public bool futurosEstrenos { get; set; } = false;

        [Parameter]
        [SupplyParameterFromQuery]
        public bool enCartelera { get; set; } = false;

        [Parameter]
        [SupplyParameterFromQuery]
        public bool masVotadas { get; set; } = false;

        private List<Pelicula>? peliculas;
        private Dictionary<string, string> queryStringsDict = new Dictionary<string, string>();

        [Parameter]
        [SupplyParameterFromQuery(Name = "pagina")]
        public int paginaActual { get; set; } = 1;

        private int paginasTotales;

        private async Task TituloKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await CargarNuevasPeliculas();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (paginaActual == 0)
                paginaActual = 1;
            await ObtenerGeneros();
            var queryStrings = GenerarQueryStrings();
            //peliculas = repo.ObtenerPeliculas();
            await RealizarBusqueda(queryStrings);
        }

        private async Task PaginaSeleccionada(int pagina)
        {
            paginaActual = pagina;
            await CargarNuevasPeliculas();
        }

        private async Task ObtenerGeneros()
        {
            var responseHttp = await repo.Get<List<Genero>>("api/generos");
            generos = responseHttp.Response!;
        }

        private async Task CargarNuevasPeliculas()
        {
            var queryStrings = GenerarQueryStrings();
            navigationManager.NavigateTo($"/peliculas/buscar?{queryStrings}");
            await RealizarBusqueda(queryStrings);
        }

        private async Task RealizarBusqueda(string queryStrings)
        {
            var responseHttp = await repo.Get<List<Pelicula>>(
                $"/api/peliculas/filtrar?{queryStrings}"
            );
            paginasTotales = int.Parse(
                responseHttp.HttpResponseMessage.Headers.GetValues("totalPaginas").FirstOrDefault()!
            );
            peliculas = responseHttp.Response!;
        }

        private string GenerarQueryStrings()
        {
            if (queryStringsDict is null)
                queryStringsDict = new Dictionary<string, string>();

            queryStringsDict["generoid"] = generoSeleccionado.ToString();
            queryStringsDict["titulo"] = titulo ?? string.Empty;
            queryStringsDict["encartelera"] = enCartelera.ToString();
            queryStringsDict["estrenos"] = futurosEstrenos.ToString();
            queryStringsDict["masvotadas"] = masVotadas.ToString();
            queryStringsDict["pagina"] = paginaActual.ToString();

            var valoresPorDefecto = new List<string>() { "false", "", "0" };

            return string.Join(
                "&",
                queryStringsDict
                    .Where(x => !valoresPorDefecto.Contains(x.Value.ToLower()))
                    .Select(x => $"{x.Key}={System.Web.HttpUtility.UrlEncode(x.Value)}")
            );
        }

        private async Task LimpiarCampos()
        {
            titulo = "";
            generoSeleccionado = 0;
            futurosEstrenos = false;
            enCartelera = false;
            masVotadas = false;
            await CargarNuevasPeliculas();
        }
    }
}