using Microsoft.Extensions.DependencyInjection;
using NTec.Domain.Contratos.Repositorios;
using NTec.Domain.Entidades;
using NTec.Infra.Repositorios;

namespace NTec.MSTeste.Setores.Repositorio
{
    [TestClass]
    public class SetorCrudTeste
    {
        private readonly ISetorRepositorio _setorRepositorio;

        public SetorCrudTeste()
        {
            var servicos = Provider.ObterProvedoresdeServico();

            _setorRepositorio = servicos.GetRequiredService<ISetorRepositorio>();
        }

        [TestMethod]
        public async Task TestarCadastrarSetor()
        {
            Setor? setor = null;

            try
            {
                setor = new Setor
                {
                    DataDeCadastro = DateTime.Now,
                    Nome           = "Financeiro"
                };

                _setorRepositorio.Cadastrar(setor);
                await _setorRepositorio.Salvar();

                Assert.IsFalse(setor.Id == 0);
                Assert.IsTrue(setor.Id > 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if(setor != null)
                {
                    _setorRepositorio.Excluir(setor);
                    await _setorRepositorio.Salvar();
                }              
            }
        }

        [TestMethod]
        public async Task TestarExcluirSetor()
        {
            try
            {
                var setor = new Setor
                {
                    DataDeCadastro = DateTime.Now,
                    Nome           = "Recursos Humanos"
                };

                _setorRepositorio.Cadastrar(setor);
                await _setorRepositorio.Salvar();

                Assert.IsTrue(setor.Id > 0);

                _setorRepositorio.Excluir(setor);
                await _setorRepositorio.Salvar();

                Assert.IsNotNull(setor);

                var setorCadastrado = await _setorRepositorio.ObterPorId(setor.Id);

                Assert.IsNull(setorCadastrado);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task TestarAtualizarSetor()
        {
            Setor? setor = null;

            try
            {
                var pessoa     = "Maria Cristina Oliveira";
                var dataAtual  = DateTime.Now;

                setor = new Setor
                {
                    DataDeCadastro = dataAtual,
                    Nome           = "Service Desk"
                };

                _setorRepositorio.Cadastrar(setor);
                await _setorRepositorio.Salvar();

                var alteradoPorNoCadastro     = setor.AlteradoPor;
                var dataAtualizacaoNoCadastro = setor.DataDeAtualizacao;

                setor.AlteradoPor       = pessoa;
                setor.DataDeAtualizacao = dataAtual.AddDays(1);

                _setorRepositorio.Atualizar(setor);
                await _setorRepositorio.Salvar();

                var setorAtualizado = await _setorRepositorio.ObterPorId(setor.Id);

                Assert.IsNotNull(setorAtualizado);

                Assert.IsFalse(setorAtualizado.Excluido);
                Assert.IsFalse(setorAtualizado.DataDeExclusao.HasValue);

                Assert.IsNull(setorAtualizado.ExcluidoPor);

                Assert.AreEqual(setor.Id, setorAtualizado.Id);
                Assert.AreEqual(setor.DataDeCadastro, setorAtualizado.DataDeCadastro);
                Assert.AreEqual(setor.Nome, setorAtualizado.Nome);

                Assert.AreNotEqual(alteradoPorNoCadastro, setorAtualizado.AlteradoPor);
                Assert.AreNotEqual(dataAtualizacaoNoCadastro, setorAtualizado.DataDeAtualizacao);

                Assert.AreEqual(pessoa, setorAtualizado.AlteradoPor);
                Assert.AreEqual(dataAtual.AddDays(1), setorAtualizado.DataDeAtualizacao);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if(setor != null)
                {
                    _setorRepositorio.Excluir(setor);
                    await _setorRepositorio.Salvar();
                }              
            }
        }

        [TestMethod]
        public async Task TestarConsultarSetores()
        {
            List<Setor>? setores = null;

            try
            {
                setores = new List<Setor>
                {
                    new Setor
                    {
                        DataDeCadastro = DateTime.Now,
                        Nome           = "Recursos Humanos"
                    },
                    new Setor
                    {
                        DataDeCadastro = DateTime.Now,
                        Nome           = "Financeiro"
                    }
                };

                foreach (var setor in setores)
                {
                    _setorRepositorio.Cadastrar(setor);
                }

                await _setorRepositorio.Salvar();

                var lista = await _setorRepositorio.ObterTodos();

                Assert.IsNotNull(lista);

                Assert.IsTrue(lista.Count() >= 2);
                
                Assert.IsNotNull(lista.FirstOrDefault(setor => setor.Id == setores[0].Id));
                Assert.IsNotNull(lista.FirstOrDefault(setor => setor.Id == setores[1].Id));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
            finally
            {
                if (setores != null)
                {
                    foreach (var setor in setores)
                    {
                        _setorRepositorio.Excluir(setor);
                    }

                    await _setorRepositorio.Salvar();
                }
            }
        }

        [TestMethod]
        public async Task TestarSetorConsultaPorId()
        {
            Setor? setor = null;

            try
            {
                setor = new Setor
                {
                    DataDeCadastro = DateTime.Now,
                    Nome           = "Jurídico"
                };

                _setorRepositorio.Cadastrar(setor);
                await _setorRepositorio.Salvar();

                Assert.IsTrue(setor.Id > 0);
                Assert.IsNotNull(_setorRepositorio.ObterPorId(setor.Id));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if (setor != null)
                {
                    _setorRepositorio.Excluir(setor);
                    await _setorRepositorio.Salvar();
                }
            }
        }
    }
}
