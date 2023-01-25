using System.Collections.Generic;

namespace NTec.Domain.Dtos.Comum
{
    public class PaginacaoColecaoDto<T>
    {
        public PaginacaoColecaoDto(IEnumerable<T> objetos, int total) 
        {
            Objetos = objetos;
            Total   = total;
        }

        public IEnumerable<T> Objetos { get; set; }
        public int Total { get; set; }  
    }
}
