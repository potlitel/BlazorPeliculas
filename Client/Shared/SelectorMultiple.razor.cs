using BlazorPeliculas.Client.Helpers;
using Microsoft.AspNetCore.Components;

namespace BlazorPeliculas.Client.Shared
{
    public partial class SelectorMultiple
    {
        private string removerTodoText = "<<";

        [Parameter]
        public List<SelectorMultipleModel> NoSeleccionados { get; set; } =
            new List<SelectorMultipleModel>();

        [Parameter]
        public List<SelectorMultipleModel> Seleccionados { get; set; } =
            new List<SelectorMultipleModel>();

        private void Seleccionar(SelectorMultipleModel item)
        {
            NoSeleccionados.Remove(item);
            Seleccionados.Add(item);
        }

        private void DesSeleccionar(SelectorMultipleModel item)
        {
            NoSeleccionados.Add(item);
            Seleccionados.Remove(item);
        }

        private void SeleccionarTodo()
        {
            Seleccionados.AddRange(NoSeleccionados);
            NoSeleccionados.Clear();
        }

        private void DesSeleccionarTodo()
        {
            NoSeleccionados.AddRange(Seleccionados);
            Seleccionados.Clear();
        }
    }
}