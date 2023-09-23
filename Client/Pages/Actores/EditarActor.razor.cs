using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Pages.Actores
{
    public partial class EditarActor
    {
        [Parameter]
        public int ActorId { get; set; }

        public EditarActor() { }
    }
}
