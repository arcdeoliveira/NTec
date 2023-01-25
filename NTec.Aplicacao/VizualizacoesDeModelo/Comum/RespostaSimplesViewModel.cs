using System.Net;
using System.Text.Json.Serialization;

namespace NTec.Aplicacao.VizualizacoesDeModelo.Comum
{
    public class RespostaSimplesViewModel
    {
       public RespostaSimplesViewModel(HttpStatusCode codigo, string mensagem)
        {
            Codigo   = codigo;
            Mensagem = mensagem;
        }

        [JsonIgnore]
        public HttpStatusCode Codigo { get; set; }  
        public string Mensagem { get; set;}
    }
}
