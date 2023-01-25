using Microsoft.AspNetCore.Http;
using NTec.Domain.Contratos.Armazenamentos;
using NTec.Domain.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NTec.Infra.Armanezamentos
{
    public class BaseArmazenamento : IBaseArmazenamento
    {
        public void CriarDiretorio(string caminhoDiretorio)
        {
            Directory.CreateDirectory(caminhoDiretorio);
        }

        public bool DiretorioExiste(string caminhoDiretorio)
        {
            return Directory.Exists(caminhoDiretorio);
        }

        public bool ValidarArquivoImagem(IFormFile arquivo)
        {
            var permitidas = TiposDeFotosPermitidas();

            return permitidas.Contains(arquivo.ContentType);
        }

        private static IEnumerable<string> TiposDeFotosPermitidas()
        {
            return new List<string>
            {
                TipoImagemFotoEnum.bmp.ToString(),
                TipoImagemFotoEnum.gif.ToString(),
                TipoImagemFotoEnum.jpeg.ToString(),
                TipoImagemFotoEnum.png.ToString()
            };
        }

        public async Task SalvarArquivo(IFormFile arquivo, string caminhoArquivo)
        {
            using var stream = new FileStream(caminhoArquivo, FileMode.Create);

            await arquivo.CopyToAsync(stream);
        }

        public bool ArquivoExiste(string caminhoArquivo)
        {
            return File.Exists(caminhoArquivo);
        }

        public void DeletarArquivo(string caminhoArquivo)
        {
            File.Delete(caminhoArquivo);
        }
    }
}
