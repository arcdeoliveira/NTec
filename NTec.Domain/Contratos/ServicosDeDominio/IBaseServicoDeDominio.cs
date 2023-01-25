using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTec.Domain.Contratos.ServicosDeDominio
{
    public interface IBaseServicoDeDominio<TEntidade> where TEntidade : class
    {
        void Atualizar(TEntidade entidade);
        void Cadastrar(TEntidade entidade);
        void Excluir(TEntidade entidade);

        Task<bool> Salvar();
        Task<TEntidade> ObterPorId(object id);

        Task<IEnumerable<TEntidade>> ObterTodos();
    }
}
