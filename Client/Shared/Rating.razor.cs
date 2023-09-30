using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Shared
{
    public partial class Rating
    {
        [Parameter]
        public int MaximoPuntaje { get; set; }

        [Parameter]
        public int PuntajeSeleccionado { get; set; }

        [Parameter]
        public EventCallback<int> OnRating { get; set; }

        private bool votado = false;

        private async Task onClickHandle(int numeroEstrella)
        {
            PuntajeSeleccionado = numeroEstrella;
            votado = true;
            await OnRating.InvokeAsync(PuntajeSeleccionado);
        }

        private void onMouseOverHandle(int numeroEstrella)
        {
            if (!votado)
            {
                PuntajeSeleccionado = numeroEstrella;
            }
        }
    }
}
