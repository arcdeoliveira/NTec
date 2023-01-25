using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NTec.Domain.Contratos.Repositorios;
using NTec.Domain.Dtos.Colaboradores;
using NTec.Domain.Dtos.Comum;
using NTec.Domain.Entidades;
using NTec.Helper.Aberto.Extensoes;
using NTec.Infra.Contexto;
using NTec.Infra.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTec.Infra.Repositorios
{
    public class ColaboradorRepositorio : BaseRepositorio<Colaborador>, IColaboradorRepositorio
    {
        private readonly IMapper _mapper;

        public ColaboradorRepositorio(IMapper mapper, DataContexto contexto) : base(contexto)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Verificar se o colaborador a cadastrar ou atualizar, não é um colaborador já cadastrado.
        /// </summary>
        /// <param name="id">Id para verificar ao atualizar se o colaborador cpf existe além do colaborador atual</param>
        /// <param name="cpf">Cpf do colaborador a verificar</param>
        /// <returns>Se Colaborador já esta cadastrado ou não.</returns>
        public async Task<bool> ColaboradorDuplicado(long cpf, Guid? id = null)
        {
            var query = _contexto.Colaboradors.AsNoTracking();

            return id.HasValue
                ? await query.AnyAsync(a => a.Cpf.Equals(cpf) && a.Id != id.Value)
                : await query.AnyAsync(a => a.Cpf.Equals(cpf));
        }

        public async Task<bool> ColaboradorExiste(Guid id)
        {
            return await _contexto.Colaboradors.AsNoTracking().AnyAsync(a => a.Id == id);
        }

        /// <summary>
        /// Verifica se colaborador possui a subordinados.
        /// </summary>
        /// <param name="id">Id do colaborador a verificar</param>
        /// <returns>Falso ou verdadeiro</returns>
        public async Task<bool> ColaboradorPossuiSubordinados(Guid id)
        {
            var colaborador = await _contexto.Colaboradors.Include(i => i.Subordinados).AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            if(colaborador == null)
            {
                return false;
            }

            return colaborador.Subordinados.Where(w => !w.Excluido).Any();
        }

        public async Task<IEnumerable<object>> ObterColaboradoresOrdenadoPorNome()
        {
            var queryable = Queryable();

            return await
            (from colaborador in queryable
             where !colaborador.Excluido
             orderby colaborador.Nome
             select new { colaborador.Id, Nome = $"{colaborador.Nome} {colaborador.SobreNome}" }).ToListAsync();
        }

        public async Task<PaginacaoColecaoDto<ColaboradorPaginacaoDto>> ObterColaboradoresPaginados(FiltroColaboradorPaginacaoDto filtroDto, 
            string caminhoRaiz)
        {
            int skip  = filtroDto.PaginaNumero.Skip(filtroDto.PaginaQuantidade);
            var query = _contexto.Colaboradors.Include(i => i.Cargo).Include(i => i.Setor).Include(i => i.Subordinados).AsNoTracking();
                query = FiltroColaboradorPaginacao.Filtrar(query, filtroDto);

            var total   = await query.CountAsync();
            var objetos = await query                
                .OrderByDescending(o => o.Id)
                .Skip(skip)
                .Take(filtroDto.PaginaQuantidade)
                .Select(colaborador => _mapper.Map<ColaboradorPaginacaoDto>(colaborador))            
                .ToListAsync();

            objetos.ForEach(f => f.FotoCaminho = string.IsNullOrEmpty(f.FotoCaminho) ? null : $"{caminhoRaiz}\\{f.FotoCaminho}");

            return new PaginacaoColecaoDto<ColaboradorPaginacaoDto>(objetos, total);
        }

        public async Task<bool> CargoCadastradoEmColaborador(int cargoId)
        {
            return await _contexto.Colaboradors.AnyAsync(a => a.CargoId == cargoId && !a.Excluido);   
        }

        public async Task<bool> SetorCadastradoEmColaborador(int setorId)
        {
            return await _contexto.Colaboradors.AnyAsync(a => a.SetorId == setorId && !a.Excluido);
        }

        //Colaborador que não tem chefe é a maior posição da empresa
        public async Task<Colaborador> ObterColaboradorComMaiorHierarquia()
        {
            return await ObterQueryParaHierarquia(_contexto).FirstOrDefaultAsync(f => !f.ChefeId.HasValue && !f.Excluido);
        }

        //Algorítimo recursivo
        public async Task ObterTodosSubordinadosDoColaborador(ColaboradorNodeDto colaborador)
        {
            if(colaborador.Children == null)
            {
                return;
            }

            foreach(var subordinado in colaborador.Children)
            {
                subordinado.Children = ObterQueryParaHierarquia(_contexto)
                    .Where(w => !w.Excluido && w.ChefeId == subordinado.Id)
                    .Select(colaborador => _mapper.Map<ColaboradorNodeDto>(colaborador))
                    .ToList();

                await ObterTodosSubordinadosDoColaborador(subordinado);
            }
        }

        private static IQueryable<Colaborador> ObterQueryParaHierarquia(DataContexto contexto)
        {
            return contexto.Colaboradors                    
                .Include(i => i.Cargo)                    
                .Include(i => i.Subordinados.Where(w => !w.Excluido))  
                .ThenInclude(t => t.Cargo)
                .AsNoTracking();
        }
    }
}