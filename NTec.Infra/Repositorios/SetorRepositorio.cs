using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NTec.Domain.Contratos.Repositorios;
using NTec.Domain.Dtos.Comum;
using NTec.Domain.Dtos.Setores;
using NTec.Domain.Entidades;
using NTec.Infra.Contexto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTec.Infra.Repositorios
{
    public class SetorRepositorio : BaseRepositorio<Setor>, ISetorRepositorio
    {
        private readonly IMapper _mapper;

        public SetorRepositorio(IMapper mapper, DataContexto contexto) : base(contexto)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<object>> ObterSetoresOrdenadoPorNome()
        {
            var queryable = Queryable();

            return await
            (from setor in queryable
             where !setor.Excluido
             orderby setor.Nome
             select new { setor.Id, setor.Nome }).ToListAsync();
        }

        public async Task<PaginacaoColecaoDto<SetorPaginacaoDto>> ObterSetoresPaginados(string nomeSetor, int skip, int take)
        {
            var query = _contexto.Setors.Include(i => i.Colaboradores).AsNoTracking().Where(w => !w.Excluido);
            if (!string.IsNullOrEmpty(nomeSetor))
            {
                query = query.Where(w => w.Nome.ToLower().Contains(nomeSetor.ToLower()));
            }

            var total   = await query.CountAsync();
            var objetos = await query
                .OrderByDescending(o => o.Id)
                .Skip(skip)
                .Take(take)
                .Select(setor => _mapper.Map<SetorPaginacaoDto>(setor))
                .ToListAsync();

            return new PaginacaoColecaoDto<SetorPaginacaoDto>(objetos, total);
        }

        /// <summary>
        /// Verificar se o setor a cadastrar ou atualizar, não é um setor já cadastrado
        /// </summary>
        /// <param name="id">Id para verificar ao atualizar se o setor  nome existe além do setor atual</param>
        /// <param name="nome">Nome do setor a verificar</param>
        /// <returns>Se setor já esta cadastrado ou não.</returns>
        public async Task<bool> SetorDuplicado(string nome, int? id = null)
        {
            return id.HasValue
                ? await _contexto.Setors.AsNoTracking().AnyAsync(a => a.Nome.ToLower().Equals(nome) && a.Id != id.Value)
                : await _contexto.Setors.AsNoTracking().AnyAsync(a => a.Nome.ToLower().Equals(nome));
        }

        public async Task<bool> SetorExiste(int setorId)
        {
            return await _contexto.Setors.AsNoTracking().AnyAsync(a => a.Id == setorId);
        }
    }
}
