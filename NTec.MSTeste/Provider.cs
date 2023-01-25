using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NTec.Helper.Restrito.DI;

namespace NTec.MSTeste
{
    public static class Provider
    {
        public static IServiceProvider ObterProvedoresdeServico()
        {
            var servicos     = new ServiceCollection();
            var configuracao = ObterConfiguracao();

            ConfigurarDependecias(servicos, configuracao);

            return servicos.BuildServiceProvider();
        }

        private static IConfigurationRoot ObterConfiguracao()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();    
        }

        private static void ConfigurarDependecias(IServiceCollection servicos, IConfigurationRoot configuracao)
        {
            servicos.ConfigurarAutoMapper();
            servicos.ConfigurarContexto(configuracao);
            servicos.ConfigurarCamadasDeDependencias();
        }
    }
}
