using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Client.Pages.Actores
{
    public partial class IndiceActores
    {
        public List<Actor>? Actores { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var respuestaHttp = await repo.Get<List<Actor>>("api/actores");
            Actores = respuestaHttp.Response!;
        }
    }
}