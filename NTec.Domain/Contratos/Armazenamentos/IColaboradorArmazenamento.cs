using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace NTec.Domain.Contratos.Armazenamentos
{
    public interface IColaboradorArmazenamento
    {
        void DeletarFoto(string caminhoArquivo);

        Task<string> SalvarFoto(IFormFile arquivo, string caminhoDiretorio);
    }
}
