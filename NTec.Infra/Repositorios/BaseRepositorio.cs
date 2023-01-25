using Microsoft.EntityFrameworkCore;
using NTec.Domain.Contratos.Repositorios;
using NTec.Domain.Dtos.Comum;
using NTec.Infra.Contexto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTec.Infra.Repositorios
{
    // Operações básicas para acesso a dados via context
    public abstract class BaseRepositorio<TEntidade> : IBaseRepositorio<TEntidade> where TEntidade : class
    {
        protected readonly DataContexto _contexto;

        public BaseRepositorio(DataContexto contexto) { _contexto = contexto; }  

        public void Atualizar(TEntidade entidade)
        {
            _contexto.Entry(entidade).State = EntityState.Modified;
        }

        public void Cadastrar(TEntidade entidade)
        {
            _contexto.Entry(entidade).State = EntityState.Added;
        }

        public void Excluir(TEntidade entidade)
        {
            _contexto.Entry(entidade).State = EntityState.Deleted;
        }

        public async Task<TEntidade> ObterPorId(object id)
        {
            return await _contexto.Set<TEntidade>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntidade>> ObterTodos(bool rastrear = false)
        {
            return rastrear 
                ? await _contexto.Set<TEntidade>().ToListAsync()
                : await _contexto.Set<TEntidade>().AsNoTracking().ToListAsync();
        }

        public async Task<PaginacaoColecaoDto<TEntidade>> ObterPaginacao(IQueryable<TEntidade> queryable, 
                                                                  IOrderedQueryable<TEntidade> ordenacao, 
                                                                  int skip, int take)
        {
            var total = await queryable.CountAsync();
            var lista = await ordenacao
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return new PaginacaoColecaoDto<TEntidade>(lista, total);
        }

        public IQueryable<TEntidade> Queryable()
        {
            return _contexto.Set<TEntidade>().AsNoTracking();
        }

        public async Task<bool> Salvar()
        {
           var resultado = await _contexto.SaveChangesAsync();

            return resultado > 0;
        }
    }
}
