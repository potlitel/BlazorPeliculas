using Microsoft.EntityFrameworkCore;

namespace BlazorPeliculas.Server.Helpers
{
    public static class HttpContextExtensions
    {
        public static async Task InsertarParametrosPaginacionEnRespuesta<T>(
            this HttpContext context,
            IQueryable<T> queryable,
            int cantidadRegistrosAMostar
        )
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            double conteo = await queryable.CountAsync();
            double totalPaginas = Math.Ceiling(conteo / cantidadRegistrosAMostar);
            context.Response.Headers.Add("conteo", conteo.ToString());
            context.Response.Headers.Add("totalPaginas", totalPaginas.ToString());
        }
    }
}