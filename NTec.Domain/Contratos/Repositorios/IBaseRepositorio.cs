using NTec.Domain.Dtos.Comum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTec.Domain.Contratos.Repositorios
{
    //interface para as operações genericas de acesso a banco de dados.
    public interface IBaseRepositorio<TEntidade> where TEntidade : class
    {
        void Atualizar(TEntidade entidade);
        void Cadastrar(TEntidade entidade);
        void Excluir(TEntidade entidade);

        IQueryable<TEntidade> Queryable();

        Task<bool> Salvar();
        Task<TEntidade> ObterPorId(object id);

        Task<IEnumerable<TEntidade>> ObterTodos(bool rastrear = false);
        Task<PaginacaoColecaoDto<TEntidade>> ObterPaginacao(IQueryable<TEntidade> queryable,
                                                     IOrderedQueryable<TEntidade> ordenacao,
                                                     int skip, int take);
    }
}
