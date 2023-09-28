namespace BlazorPeliculas.Shared.DTOs
{
    public class ParametrosBusquedaPeliculasDTO
    {
        public int Pagina { get; set; } = 1;
        public int CantidadRegistros { get; set; } = 10;

        public PaginacionDTO paginacionDTO
        {
            get
            {
                return new PaginacionDTO
                {
                    Pagina = Pagina,
                    CantidadRegistros = CantidadRegistros,
                };
            }
        }

        public string? Titulo { get; set; }

        public int GeneroID { get; set; }

        public bool EnCartelera { get; set; }

        public bool Estrenos { get; set; }

        public bool MasVotadas { get; set; }
    }
}