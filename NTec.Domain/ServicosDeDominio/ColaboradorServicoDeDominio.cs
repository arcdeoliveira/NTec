using AutoMapper;
using Microsoft.AspNetCore.Http;
using NTec.Domain.Contratos.Armazenamentos;
using NTec.Domain.Contratos.Repositorios;
using NTec.Domain.Contratos.ServicosDeDominio;
using NTec.Domain.Dtos.Colaboradores;
using NTec.Domain.Dtos.Comum;
using NTec.Domain.Entidades;
using NTec.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTec.Domain.ServicosDeDominio
{
    //classe de regras de negócio(ServiceDomain)
    public class ColaboradorServicoDeDominio : BaseServicoDeDominio<Colaborador>, IColaboradorServicoDeDominio
    {
        private readonly ICargoRepositorio         _cargoRepositorio;
        private readonly IColaboradorArmazenamento _colaboradorArmazenamento;
        private readonly IColaboradorRepositorio   _colaboradorRepositorio;
        private readonly ISetorRepositorio         _setorRepositorio;
        private readonly IMapper _mapper;

        public ColaboradorServicoDeDominio(ICargoRepositorio cargoRepositorio, IColaboradorArmazenamento colaboradorArmazenamento,
                                                                               IColaboradorRepositorio colaboradorRepositorio,
                                                                               ISetorRepositorio setorRepositorio,
                                                                               IMapper mapper) : base(colaboradorRepositorio)
        {
            _cargoRepositorio         = cargoRepositorio;
            _colaboradorArmazenamento = colaboradorArmazenamento;
            _colaboradorRepositorio   = colaboradorRepositorio;
            _setorRepositorio         = setorRepositorio;
            _mapper = mapper;
        }

        public async Task<string> ValidarRelacionamentosColaborador(Guid? chefeId, int cargoId, int setorId)
        {
            if (chefeId.HasValue && !await _colaboradorRepositorio.ColaboradorExiste(chefeId.Value))
            {
                return Resources.ColaboradorNaoEncontrado;
            }

            if (!await _cargoRepositorio.CargoExiste(cargoId))
            {
                return Resources.CargoNaoEncontrado;
            }

            if (!await _setorRepositorio.SetorExiste(setorId))
            {
                return Resources.SetorNaoEncontrado;
            }

            return null;
        }

        public async Task<bool> PossuiSubordinados(Guid id)
        {
            return await _colaboradorRepositorio.ColaboradorPossuiSubordinados(id);
        }

        public async Task<string> SalvarFoto(IFormFile arquivo, string caminhoRaiz)
        {
            //Cadastro de foto colaborador não é obrigatório
            if (arquivo == null || arquivo.Length == 0)
            {
                return null;
            }

            if(string.IsNullOrEmpty(caminhoRaiz))
            {
                return Resources.CaminhoRaizObrigatório;
            }

            var caminhoDiretorio = $"{caminhoRaiz}\\Imagens\\Colaboradores";

            return await _colaboradorArmazenamento.SalvarFoto(arquivo, caminhoDiretorio);
        }

        public async Task<IEnumerable<object>> ObterDropDown()
        {
            return await _colaboradorRepositorio.ObterColaboradoresOrdenadoPorNome();
        }

        public void DeletarFoto(string caminhoRaiz, string nomeArquivo)
        {
            if(string.IsNullOrEmpty(caminhoRaiz) || string.IsNullOrEmpty(nomeArquivo))
            {
                return;
            }

            var caminhoDiretorio = $"{caminhoRaiz}\\Imagens\\Colaboradores";
            var caminhoArquivo   = $"{caminhoDiretorio}\\{nomeArquivo}";

            _colaboradorArmazenamento.DeletarFoto(caminhoArquivo);
        }

        public async Task<PaginacaoColecaoDto<ColaboradorPaginacaoDto>> ObterPaginacao(FiltroColaboradorPaginacaoDto filtroDto, string caminhoRaiz)
        {
            return await _colaboradorRepositorio.ObterColaboradoresPaginados(filtroDto, caminhoRaiz);
        }

        public async Task<bool> ColaboradorDuplicado(long cpf, Guid? id = null)
        {
            return await _colaboradorRepositorio.ColaboradorDuplicado(cpf, id);
        }

        public async Task<bool> CargoCadastradoEmColaborador(int cargoId)
        {
            return await _colaboradorRepositorio.CargoCadastradoEmColaborador(cargoId);
        }

        public async Task<bool> SetorCadastradoEmColaborador(int setorId)
        {
            return await _colaboradorRepositorio.SetorCadastradoEmColaborador(setorId);
        }

        public async Task<ColaboradorNodeDto> ObterHierarquia()
        {
            var colaborador = await _colaboradorRepositorio.ObterColaboradorComMaiorHierarquia();
            if(colaborador == null)
            {
                return null;    
            }

            var colaboradorNode = _mapper.Map<ColaboradorNodeDto>(colaborador);

            await _colaboradorRepositorio.ObterTodosSubordinadosDoColaborador(colaboradorNode);

            return colaboradorNode;
        }
    }
}
