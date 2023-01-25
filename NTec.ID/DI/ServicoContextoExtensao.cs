using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NTec.Infra.Contexto;

namespace NTec.Helper.Restrito.DI
{
    public static class ServicoContextoExtensao
    {
        public static void ConfigurarContexto(this IServiceCollection servicos, IConfigurationRoot configuracao)
        {
            var conexao = configuracao.GetConnectionString("ConexaoBancoDeDados");

            servicos.AddDbContext<DataContexto>(opcoes => opcoes.UseSqlServer(conexao));
        }
    }
}
