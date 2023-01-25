namespace NTec.Domain.Dtos.Comum
{
    public class PaginacaoDto
    {
        public PaginacaoDto() 
        {
            PaginaNumero     = 1;
            PaginaQuantidade = 10;
        }

        public PaginacaoDto(int paginaNumero, int paginaQuantidade)
        {
            PaginaNumero     = paginaNumero;
            PaginaQuantidade = paginaQuantidade;
        }

        public int PaginaNumero { get; set; }
        public int PaginaQuantidade { get; set;}
    }
}
