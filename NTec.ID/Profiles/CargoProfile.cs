using AutoMapper;
using NTec.Aplicacao.VizualizacoesDeModelo.Cargos;
using NTec.Domain.Dtos.Cargos;
using NTec.Domain.Entidades;

namespace NTec.Helper.Restrito.Profiles
{
    public class CargoProfile : Profile
    {
        public CargoProfile()
        {
            CreateMap<CargoCadastroViewModel, Cargo>()
               .ForMember(dst => dst.DataDeCadastro,
                          map => map.MapFrom(src => DateTime.Now));

            CreateMap<CargoAtualizarViewModel, Cargo>()
                .ForMember(dst => dst.DataDeAtualizacao,
                           map => map.MapFrom(src => DateTime.Now));

            CreateMap<CargoExcluirViewModel, Cargo>()
               .ForMember(dst => dst.DataDeExclusao,
                          map => map.MapFrom(src => DateTime.Now))
               .ForMember(dst => dst.Excluido,
                          map => map.MapFrom(src => true));

            CreateMap<Cargo, CargoPaginacaoDto>()
             .ForMember(dst => dst.Data,
                        map => map.MapFrom(src => src.DataDeAtualizacao.HasValue
                             ? src.DataDeAtualizacao.Value.ToShortDateString()
                             : src.DataDeCadastro.ToShortDateString()))
             .ForMember(dst => dst.QuantidadeColaboradores,
                        map => map.MapFrom(src => src.Colaboradores == null ? 0 : src.Colaboradores.Where(w => !w.Excluido).Count()));
        }
    }
}
