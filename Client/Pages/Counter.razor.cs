using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorPeliculas.Client.Pages
{
    public partial class Counter
    {
        [Inject]
        private ServicioSingleton singleton { get; set; } = null!;

        [Inject]
        private ServicioTransient transient { get; set; } = null!;

        [Inject]
        private IJSRuntime js { get; set; } = null!;

        private int currentCount = 0;
        private static int currentCountStatic = 0;

        private async Task IncrementCount()
        {
            currentCount++;
            currentCountStatic = currentCount;
            singleton.valor = currentCount;
            transient.valor = currentCount;
            await js.InvokeVoidAsync("pruebaPuntoNetStatic");
        }

        [JSInvokable]
        public static Task<int> ObtenerCurrentCount()
        {
            return Task.FromResult(currentCountStatic);
        }
    }
}
