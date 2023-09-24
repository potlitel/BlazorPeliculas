using Markdig;
using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Shared
{
    public partial class MostrarMarkdown
    {
        public MostrarMarkdown()
        { }

        [Parameter]
        public string? ContenidoMarkdown { get; set; }

        [Parameter]
        public RenderFragment PlantillaCarga { get; set; } = null!;

        private string? ContenidoHTML;

        protected override void OnParametersSet()
        {
            if (ContenidoMarkdown is not null)
            {
                ContenidoHTML = Markdown.ToHtml(ContenidoMarkdown);
            }
        }
    }
}