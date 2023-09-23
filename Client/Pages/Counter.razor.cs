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

        private IJSObjectReference? modulo;

        private int currentCount = 0;
        private static int currentCountStatic = 0;

        [JSInvokable]
        public async Task IncrementCount()
        {
            modulo = await js.InvokeAsync<IJSObjectReference>("import", "./js/Counter.js");
            await modulo.InvokeVoidAsync("mostrarAlerta", "Hola desde js");

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

        private async Task IncrementCountJavascript()
        {
            await js.InvokeVoidAsync("pruebaPuntoNetInstancia", DotNetObjectReference.Create(this));
        }
    }
}
