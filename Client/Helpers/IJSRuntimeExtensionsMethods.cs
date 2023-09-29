using Microsoft.JSInterop;

namespace BlazorPeliculas.Client.Helpers
{
    public static class IJSRuntimeExtensionsMethods
    {
        public static async ValueTask<bool> Confirm(this IJSRuntime js, string mensaje)
        {
            await js.InvokeVoidAsync("console.log", "Prueba de console log");
            return await js.InvokeAsync<bool>("confirm", mensaje);
        }

        public static ValueTask<object> GuardarEnLocalStorage(
            this IJSRuntime js,
            string llave,
            string valor
        )
        {
            return js.InvokeAsync<object>("localStorage.setItem", llave, valor);
        }

        public static ValueTask<object> ObtenerDeLocalStorage(this IJSRuntime js, string llave)
        {
            return js.InvokeAsync<object>("localStorage.getItem", llave);
        }

        public static ValueTask<object> RemoverDelLocalStorage(this IJSRuntime js, string llave)
        {
            return js.InvokeAsync<object>("localStorage.removeItem", llave);
        }
    }
}