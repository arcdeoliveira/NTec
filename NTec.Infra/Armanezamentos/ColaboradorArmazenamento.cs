using Microsoft.AspNetCore.Http;
using NTec.Domain.Contratos.Armazenamentos;
using NTec.Infra.Properties;
using System.IO;
using System.Threading.Tasks;

namespace NTec.Infra.Armanezamentos
{
    public class ColaboradorArmazenamento : IColaboradorArmazenamento
    { 
        private readonly IBaseArmazenamento _armazenamento;

        public ColaboradorArmazenamento(IBaseArmazenamento armazenamento)
        {
            _armazenamento = armazenamento;
        }

        public void DeletarFoto(string caminhoArquivo)
        {
            if(_armazenamento.ArquivoExiste(caminhoArquivo))
            {
                _armazenamento.DeletarArquivo(caminhoArquivo);
            }
        }

        public async Task<string> SalvarFoto(IFormFile arquivo, string caminhoDiretorio)
        {
            if(!_armazenamento.ValidarArquivoImagem(arquivo))
            {
                return Resources.FotoInvalida;
            }

            if(!_armazenamento.DiretorioExiste(caminhoDiretorio))
            {
                _armazenamento.CriarDiretorio(caminhoDiretorio);
            }

            var nomeArquivo    = $"{Path.GetRandomFileName}{arquivo.FileName}";
            var caminhoArquivo = $"{caminhoDiretorio}\\{nomeArquivo}";

            await _armazenamento.SalvarArquivo(arquivo, caminhoArquivo);

            return nomeArquivo;
        }
    }
}
