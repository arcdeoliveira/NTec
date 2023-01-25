using System.Collections.Generic;
using System.Net;

namespace NTec.Aplicacao.VizualizacoesDeModelo.Comum
{
    public class RespostaColecaoViewModel<T> : RespostaSimplesViewModel
    {
        public RespostaColecaoViewModel(IEnumerable<T> objetos, HttpStatusCode codigo, string mensagem) 
            : base(codigo, mensagem)
        {
            Objetos = objetos;
        }    

        public IEnumerable<T> Objetos { get; set; }
    }
}
