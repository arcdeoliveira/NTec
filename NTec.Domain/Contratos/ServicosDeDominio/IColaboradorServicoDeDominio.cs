using Microsoft.AspNetCore.Http;
using NTec.Domain.Dtos.Colaboradores;
using NTec.Domain.Dtos.Comum;
using NTec.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTec.Domain.Contratos.ServicosDeDominio
{
    public interface IColaboradorServicoDeDominio : IBaseServicoDeDominio<Colaborador>
    {
        void DeletarFoto(string caminhoRaiz, string nomeArquivo);

        Task<bool> ColaboradorDuplicado(long cpf, Guid? id = null);
        Task<bool> PossuiSubordinados(Guid id);
        Task<bool> CargoCadastradoEmColaborador(int cargoId);
        Task<bool> SetorCadastradoEmColaborador(int setorId);

        Task<string> ValidarRelacionamentosColaborador(Guid? chefeId, int cargoId, int setorId);
        Task<string> SalvarFoto(IFormFile arquivo, string caminhoRaiz);

        Task<ColaboradorNodeDto> ObterHierarquia();

        Task<PaginacaoColecaoDto<ColaboradorPaginacaoDto>> ObterPaginacao(FiltroColaboradorPaginacaoDto filtroDto, string caminhoRaiz);
        Task<IEnumerable<object>> ObterDropDown();
    }
}
