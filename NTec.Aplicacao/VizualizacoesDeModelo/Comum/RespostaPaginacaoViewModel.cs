using NTec.Domain.Dtos.Comum;
using System.Collections.Generic;
using System.Net;

namespace NTec.Aplicacao.VizualizacoesDeModelo.Comum
{
    public class RespostaPaginacaoViewModel<T> : RespostaColecaoViewModel<T>
    {
        public RespostaPaginacaoViewModel(IEnumerable<T> objetos, HttpStatusCode codigo, string mensagem) 
            : base(objetos, codigo, mensagem)
        {
        }

        public int Total { get; set; }
        public PaginacaoDto Paginacao { get; set; }
    }
}
