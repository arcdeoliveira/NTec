using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace NTec.Domain.Contratos.Armazenamentos
{
    public interface IBaseArmazenamento
    {
        bool ArquivoExiste(string caminhoArquivo);
        bool DiretorioExiste(string caminhoDiretorio);
        bool ValidarArquivoImagem(IFormFile arquivo);

        void CriarDiretorio(string caminhoDiretorio);
        void DeletarArquivo(string caminhoArquivo);

        Task SalvarArquivo(IFormFile arquivo, string caminhoArquivo);
    }
}
