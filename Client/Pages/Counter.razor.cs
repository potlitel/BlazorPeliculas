using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Pages
{
    public partial class Counter
    {
        [Inject]
        ServicioSingleton singleton { get; set; } = null!;

        [Inject]
        ServicioTransient transient { get; set; } = null!;

        private int currentCount = 0;

        private void IncrementCount()
        {
            currentCount++;
            singleton.valor = currentCount;
            transient.valor = currentCount;
        }
    }
}
