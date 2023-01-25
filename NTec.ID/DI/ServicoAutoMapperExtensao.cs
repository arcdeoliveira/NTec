using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NTec.Helper.Restrito.Profiles;

namespace NTec.Helper.Restrito.DI
{
    public static class ServicoAutoMapperExtensao
    {
        public static void ConfigurarAutoMapper(this IServiceCollection servicos)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CargoProfile>();
                cfg.AddProfile<ColaboradorProfile>();
                cfg.AddProfile<SetorProfile>();
            });

            var mapper = configuration.CreateMapper();

            servicos.AddSingleton(mapper);
        }
    }
}
