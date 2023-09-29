using BlazorPeliculas.Client.Helpers;
using MathNet.Numerics.Statistics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorPeliculas.Client.Pages
{
    public partial class Counter
    {
        //[Inject]
        //private ServicioSingleton singleton { get; set; } = null!;

        //[Inject]
        //private ServicioTransient transient { get; set; } = null!;

        [Inject]
        private IJSRuntime js { get; set; } = null!;

        //[CascadingParameter(Name = "Color")]
        //protected string Color { get; set; } = null!;

        //[CascadingParameter(Name = "Size")]
        //protected string Size { get; set; } = null!;
        [CascadingParameter]
        protected AppState AppState { get; set; } = null!;

        [CascadingParameter]
        private Task<AuthenticationState> authStateTask { get; set; } = null!;

        private IJSObjectReference? modulo;

        private int currentCount = 0;
        private static int currentCountStatic = 0;

        [JSInvokable]
        public async Task IncrementCount()
        {
            //modulo = await js.InvokeAsync<IJSObjectReference>("import", "./js/Counter.js");
            //await modulo.InvokeVoidAsync("mostrarAlerta", "Hola desde js");
            var arreglo = new double[] { 1, 2, 3, 4, 5 };
            var max = arreglo.Maximum();
            var min = arreglo.Minimum();

            //currentCount++;
            //currentCountStatic = currentCount;
            //singleton.valor = currentCount;
            //transient.valor = currentCount;
            //await js.InvokeVoidAsync("pruebaPuntoNetStatic");
            //await js.InvokeVoidAsync("alert", $"El maximo es {max} y el min es {min}");

            var resultAuthStateTask = await authStateTask;
            var userAuthenticated = resultAuthStateTask.User.Identity!.IsAuthenticated;

            if (userAuthenticated)
                currentCount += 1;
            else
                currentCount -= 1;
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