using NTec.Domain.Contratos.Repositorios;
using NTec.Domain.Contratos.ServicosDeDominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTec.Domain.ServicosDeDominio
{
    //classe abstrata e generica para as demais Serviços de domínio
    public abstract class BaseServicoDeDominio<TEntidade> : IBaseServicoDeDominio<TEntidade> where TEntidade : class
    {
        private readonly IBaseRepositorio<TEntidade> _repositorio;

        public BaseServicoDeDominio(IBaseRepositorio<TEntidade> repositorio)
        {
            _repositorio = repositorio;
        }

        public virtual void Atualizar(TEntidade entidade)
        {
            _repositorio.Atualizar(entidade);
        }

        public virtual void Cadastrar(TEntidade entidade)
        {
            _repositorio.Cadastrar(entidade);
        }

        public virtual void Excluir(TEntidade entidade) 
        {
            _repositorio.Excluir(entidade);
        }

        public async Task<bool> Salvar()
        {
            return await _repositorio.Salvar();
        }

        public virtual async Task<TEntidade> ObterPorId(object id)
        {
            return await _repositorio.ObterPorId(id);
        }

        public virtual async Task<IEnumerable<TEntidade>> ObterTodos()
        {
            return await _repositorio.ObterTodos();
        }      
    }
}
