using Microsoft.Extensions.DependencyInjection;
using NTec.Aplicacao.AplicacaoDeServicos;
using NTec.Aplicacao.Contratos;
using NTec.Domain.Contratos.Armazenamentos;
using NTec.Domain.Contratos.Repositorios;
using NTec.Domain.Contratos.ServicosDeDominio;
using NTec.Domain.ServicosDeDominio;
using NTec.Infra.Armanezamentos;
using NTec.Infra.Repositorios;

namespace NTec.Helper.Restrito.DI
{
    public static class ServicoDeColecoesDasCamadasExtensao
    {
        public static void ConfigurarCamadasDeDependencias(this IServiceCollection servicos)
        {
            RegistrarServicosDeArmazenamento(servicos);
            RegistrarAplicacoesDeServico(servicos);
            RegistrarDominiosDeServico(servicos);
            RegistrarRepositorios(servicos);
        }

        public static void RegistrarServicosDeArmazenamento(IServiceCollection servicos)
        {
            servicos.AddSingleton<IBaseArmazenamento, BaseArmazenamento>();
            servicos.AddScoped<IColaboradorArmazenamento, ColaboradorArmazenamento>();
        }

        public static void RegistrarAplicacoesDeServico(IServiceCollection servicos)
        {
            servicos.AddScoped<ICargoAplicacaoDeServico, CargoAplicacaoDeServico>();
            servicos.AddScoped<IColaboradorAplicacaoServico, ColaboradorAplicacaoDeServico>();
            servicos.AddScoped<ISetorAplicacaoDeServico, SetorAplicacaoDeServico>();
        }

        public static void RegistrarDominiosDeServico(IServiceCollection servicos)
        {
            servicos.AddScoped<ICargoServicoDeDominio, CargoServicoDeDominio>();
            servicos.AddScoped<IColaboradorServicoDeDominio, ColaboradorServicoDeDominio>();
            servicos.AddScoped<ISetorServicoDeDominio, SetorServicoDeDominio>();
        }

        public static void RegistrarRepositorios(IServiceCollection servicos)
        {
            servicos.AddScoped<ICargoRepositorio, CargoRepositorio>();
            servicos.AddScoped<IColaboradorRepositorio, ColaboradorRepositorio>();
            servicos.AddScoped<ISetorRepositorio, SetorRepositorio>();
        }
    }
}
