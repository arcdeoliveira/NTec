using AutoMapper;
using NTec.Aplicacao.VizualizacoesDeModelo.Setores;
using NTec.Domain.Dtos.Setores;
using NTec.Domain.Entidades;

namespace NTec.Helper.Restrito.Profiles
{
    public class SetorProfile : Profile
    {
        public SetorProfile()
        {
            CreateMap<SetorCadastroViewModel, Setor>()
                .ForMember(dst => dst.DataDeCadastro,
                           map => map.MapFrom(src => DateTime.Now));

            CreateMap<SetorAtualizarViewModel, Setor>()
                .ForMember(dst => dst.DataDeAtualizacao,
                           map => map.MapFrom(src => DateTime.Now));

            CreateMap<SetorExcluirViewModel, Setor>()
                .ForMember(dst => dst.DataDeExclusao,
                           map => map.MapFrom(src => DateTime.Now))
                .ForMember(dst => dst.Excluido,
                           map => map.MapFrom(src => true));

            CreateMap<Setor, SetorPaginacaoDto>()
                .ForMember(dst => dst.Data,
                           map => map.MapFrom(src => src.DataDeAtualizacao.HasValue
                                ? src.DataDeAtualizacao.Value.ToShortDateString()
                                : src.DataDeCadastro.ToShortDateString()))
                .ForMember(dst => dst.QuantidadeColaboradores,
                           map => map.MapFrom(src => src.Colaboradores == null ? 0 : src.Colaboradores.Where(w => !w.Excluido).Count()));
        }
    }
}
