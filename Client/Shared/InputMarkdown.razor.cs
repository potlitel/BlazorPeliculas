using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Shared
{
    public partial class InputMarkdown
    {
        public InputMarkdown()
        { }

        //[Parameter]
        //public Expression<Func<Tvalue>>? For { get; set; }

        [Parameter]
        public string Label { get; set; } = "Campo";

        [Parameter]
        public string Alain { get; set; } = "Campo";
    }
}