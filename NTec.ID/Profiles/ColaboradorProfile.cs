using AutoMapper;
using NTec.Aplicacao.VizualizacoesDeModelo.Colaboradores;
using NTec.Domain.Dtos.Colaboradores;
using NTec.Domain.Entidades;

namespace NTec.Helper.Restrito.Profiles
{
    public class ColaboradorProfile : Profile
    {
        public ColaboradorProfile()
        {
            CreateMap<ColaboradorCadastroViewModel, Colaborador>()
                .ForMember(dst => dst.DataDeCadastro,
                           map => map.MapFrom(src => DateTime.Now));

            CreateMap<ColaboradorAtualizarViewModel, Colaborador>()
             .ForMember(dst => dst.DataDeAtualizacao,
                        map => map.MapFrom(src => DateTime.Now));

            CreateMap<ColaboradorExcluirViewModel, Colaborador>()
             .ForMember(dst => dst.DataDeExclusao,
                        map => map.MapFrom(src => DateTime.Now))
             .ForMember(dst => dst.Excluido,
                        map => map.MapFrom(src => true));

            CreateMap<ColaboradorFiltroViewModel, FiltroColaboradorPaginacaoDto>();

            CreateMap<Colaborador, ColaboradorDetalheViewModel>();

            CreateMap<Colaborador, ColaboradorPaginacaoDto>()
                .ForMember(dst => dst.PosicaoChefe,
                           map => map.MapFrom(src => src.Subordinados.Where(w => !w.Excluido).Any() ? "sim" : "não"))
                .ForMember(dst => dst.CargoNome,
                           map => map.MapFrom(src => src.Cargo == null ? string.Empty : src.Cargo.Nome))
                .ForMember(dst => dst.FotoCaminho,
                           map => map.MapFrom(src => src.Foto))
                .ForMember(dst => dst.Genero,
                           map => map.MapFrom(src => src.Genero.ToString()))
                .ForMember(dst => dst.Data,
                           map => map.MapFrom(src => src.DataDeAtualizacao.HasValue
                                ? src.DataDeAtualizacao.Value.ToShortDateString()
                                : src.DataDeCadastro.ToShortDateString()))
                .ForMember(dst => dst.NomeCompleto,
                           map => map.MapFrom(src => $"{src.Nome} {src.SobreNome}"))
                .ForMember(dst => dst.SetorNome,
                           map => map.MapFrom(src => src.Setor == null ? string.Empty : src.Setor.Nome));

            CreateMap<Colaborador, ColaboradorNodeDto>()
                .ForMember(dst => dst.Description,
                           map => map.MapFrom(src => src.Cargo == null ? string.Empty : src.Cargo.Nome))
                .ForMember(dst => dst.Image,
                           map => map.MapFrom(src => src.Foto ?? ""))
                .ForMember(dst => dst.Name,
                           map => map.MapFrom(src => $"{src.Nome} {src.SobreNome}"))
                .ForMember(dst => dst.Children,
                           map => map.MapFrom(src => src.Subordinados == null ? null : src.Subordinados.Select(s => new ColaboradorNodeDto
                           {
                               Id        = s.Id,
                               Name      = $"{s.Nome} {s.SobreNome}",
                               Image    = s.Foto ?? "assets/5.png",
                               Description = src.Cargo == null ? string.Empty : src.Cargo.Nome
                           })));
        }
    }
}
