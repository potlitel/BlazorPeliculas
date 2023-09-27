using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorPeliculas.Client.Shared
{
    public partial class CustomInputImg
    {
        //private string? imagenUrl;

        [Parameter]
        public string Label { get; set; } = "Image";

        [Parameter]
        public string? imagenUrl { get; set; }

        [Parameter]
        public EventCallback<string> imagenSeleccionada { get; set; }

        private string? imagenBase64;

        private async Task OnChange(InputFileChangeEventArgs e)
        {
            var imagenes = e.GetMultipleFiles();
            foreach (var img in imagenes)
            {
                var arrBytes = new byte[img.Size];
                await img.OpenReadStream().ReadAsync(arrBytes);
                imagenBase64 = Convert.ToBase64String(arrBytes);
                imagenUrl = null;
                await imagenSeleccionada.InvokeAsync(imagenBase64);
                //Console.WriteLine(imagenBase64);
                StateHasChanged();
            }
        }
    }
}