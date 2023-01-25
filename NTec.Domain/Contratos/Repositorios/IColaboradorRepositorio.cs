using NTec.Domain.Dtos.Colaboradores;
using NTec.Domain.Dtos.Comum;
using NTec.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTec.Domain.Contratos.Repositorios
{
    public interface IColaboradorRepositorio : IBaseRepositorio<Colaborador>
    {
        Task ObterTodosSubordinadosDoColaborador(ColaboradorNodeDto colaborador);

        Task<bool> ColaboradorDuplicado(long cpf, Guid? id = null);
        Task<bool> ColaboradorExiste(Guid id);
        Task<bool> ColaboradorPossuiSubordinados(Guid id);
        Task<bool> CargoCadastradoEmColaborador(int cargoId);
        Task<bool> SetorCadastradoEmColaborador(int setorId);

        Task<Colaborador> ObterColaboradorComMaiorHierarquia();

        Task<IEnumerable<object>> ObterColaboradoresOrdenadoPorNome();
        Task<PaginacaoColecaoDto<ColaboradorPaginacaoDto>> ObterColaboradoresPaginados(FiltroColaboradorPaginacaoDto filtroDto, string caminhoRaiz);
    }
}
