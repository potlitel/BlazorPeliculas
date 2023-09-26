using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Client.Pages.Generos
{
    public partial class IndiceGeneros
    {
        public List<Genero>? Generos { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var respuestaHttp = await repo.Get<List<Genero>>("api/generos");
            Generos = respuestaHttp.Response!;
        }
    }
}