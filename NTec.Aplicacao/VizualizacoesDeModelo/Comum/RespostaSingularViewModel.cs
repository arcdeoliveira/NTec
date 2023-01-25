using System.Net;

namespace NTec.Aplicacao.VizualizacoesDeModelo.Comum
{
    public class RespostaSingularViewModel<T> : RespostaSimplesViewModel
    {
        public RespostaSingularViewModel(HttpStatusCode codigo, string mensagem) : base(codigo, mensagem)
        {
        }

        public T Objeto { get; set; }
    }
}
