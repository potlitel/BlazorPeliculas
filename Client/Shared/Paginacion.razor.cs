using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Shared
{
    public partial class Paginacion
    {
        [Parameter]
        public int PaginaActual { get; set; } = 1;

        [Parameter]
        public int PaginasTotales { get; set; }

        [Parameter]
        public int Radio { get; set; } = 3;

        [Parameter]
        public EventCallback<int> PaginaSeleccionada { get; set; }

        private List<PaginasModel> Enlaces = new List<PaginasModel>();

        private async Task PaginaSeleccionadaInterno(PaginasModel paginaModel)
        {
            if (paginaModel.Pagina == PaginaActual)
            {
                return;
            }

            if (!paginaModel.Habilitada)
            {
                return;
            }

            await PaginaSeleccionada.InvokeAsync(paginaModel.Pagina);
        }

        protected override void OnParametersSet()
        {
            Enlaces = new List<PaginasModel>();

            var enlaceAnteriorHabilitada = PaginaActual != 1;
            var enlaceAnteriorPagina = PaginaActual - 1;
            Enlaces.Add(
                new PaginasModel
                {
                    Texto = "Anterior",
                    Pagina = enlaceAnteriorPagina,
                    Habilitada = enlaceAnteriorHabilitada,
                }
            );

            for (int i = 1; i <= PaginasTotales; i++)
            {
                if (i >= PaginaActual - Radio && i <= PaginaActual + Radio)
                {
                    Enlaces.Add(
                        new PaginasModel
                        {
                            Pagina = i,
                            Activa = PaginaActual == i,
                            Texto = i.ToString(),
                        }
                    );
                }
            }

            var enlaceSiguienteHabilitado = PaginaActual != PaginasTotales;
            var enlaceSiguientePagina = PaginaActual + 1;
            Enlaces.Add(
                new PaginasModel
                {
                    Texto = "Siguiente",
                    Pagina = enlaceSiguientePagina,
                    Habilitada = enlaceSiguienteHabilitado,
                }
            );
        }
    }

    public class PaginasModel
    {
        public string Texto { get; set; } = null!;
        public int Pagina { get; set; }
        public bool Habilitada { get; set; } = true;
        public bool Activa { get; set; } = false;
    }
}
