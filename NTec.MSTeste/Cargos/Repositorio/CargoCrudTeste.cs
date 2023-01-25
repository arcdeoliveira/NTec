using Microsoft.Extensions.DependencyInjection;
using NTec.Domain.Contratos.Repositorios;
using NTec.Domain.Entidades;

namespace NTec.MSTeste.Cargos.Repositorio
{
    [TestClass]
    public class CargoCrudTeste
    {
        private readonly ICargoRepositorio _cargoRepositorio;

        public CargoCrudTeste()
        {
            var servicos = Provider.ObterProvedoresdeServico();

            _cargoRepositorio = servicos.GetRequiredService<ICargoRepositorio>();
        }

        [TestMethod]
        public async Task TestarCadastrarCargo()
        {
            Cargo? cargo = null;

            try
            {                
                cargo = new Cargo
                {
                    DataDeCadastro = DateTime.Now,
                    Nome           = "Desenvolvedor de Software"
                };

                _cargoRepositorio.Cadastrar(cargo);
                await _cargoRepositorio.Salvar();

                Assert.IsFalse(cargo.Id == 0);
                Assert.IsTrue(cargo.Id > 0);             
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if(cargo != null)
                {
                    _cargoRepositorio.Excluir(cargo);
                    await _cargoRepositorio.Salvar();
                }            
            }
        }

        [TestMethod]
        public async Task TestarExcluirCargo()
        {
            try
            {
                var cargo = new Cargo
                {
                    DataDeCadastro = DateTime.Now,
                    Nome           = "Médico Veterinário"
                };

                _cargoRepositorio.Cadastrar(cargo);
                await _cargoRepositorio.Salvar();

                Assert.IsTrue(cargo.Id > 0);

                _cargoRepositorio.Excluir(cargo);
                await _cargoRepositorio.Salvar();

                Assert.IsNotNull(cargo);

                var cargoCadastrado = await _cargoRepositorio.ObterPorId(cargo.Id);

                Assert.IsNull(cargoCadastrado);               
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task TestarAtualizarCargo()
        {
            Cargo? cargo = null;

            try
            {
                var nomePessoa = "Anderson Oliveira";
                var dataAtual  = DateTime.Now;

                cargo = new Cargo
                {
                    DataDeCadastro = dataAtual,
                    Nome           = "Diretor-Presidente"
                };

                _cargoRepositorio.Cadastrar(cargo);
                await _cargoRepositorio.Salvar();

                var alteradoPorNoCadastro     = cargo.AlteradoPor;
                var dataAtualizacaoNoCadastro = cargo.DataDeAtualizacao;

                cargo.AlteradoPor       = nomePessoa;
                cargo.DataDeAtualizacao = dataAtual.AddDays(1);

                _cargoRepositorio.Atualizar(cargo);
                await _cargoRepositorio.Salvar();

                var cargoAtualizado = await _cargoRepositorio.ObterPorId(cargo.Id);

                Assert.IsNotNull(cargoAtualizado);

                Assert.IsFalse(cargoAtualizado.Excluido);
                Assert.IsFalse(cargoAtualizado.DataDeExclusao.HasValue);

                Assert.IsNull(cargoAtualizado.ExcluidoPor);

                Assert.AreEqual(cargo.Id, cargoAtualizado.Id);
                Assert.AreEqual(cargo.DataDeCadastro, cargoAtualizado.DataDeCadastro);
                Assert.AreEqual(cargo.Nome, cargoAtualizado.Nome);
                
                Assert.AreNotEqual(alteradoPorNoCadastro, cargoAtualizado.AlteradoPor);
                Assert.AreNotEqual(dataAtualizacaoNoCadastro, cargoAtualizado.DataDeAtualizacao);

                Assert.AreEqual(nomePessoa, cargoAtualizado.AlteradoPor);
                Assert.AreEqual(dataAtual.AddDays(1), cargoAtualizado.DataDeAtualizacao);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if(cargo != null)
                {
                    _cargoRepositorio.Excluir(cargo);
                    await _cargoRepositorio.Salvar();
                }            
            }
        }

        [TestMethod]
        public async Task TestarConsultarCargos()
        {
            List<Cargo>? cargos = null;

            try
            {
                cargos = new List<Cargo>
                {
                    new Cargo
                    {
                        DataDeCadastro = DateTime.Now,
                        Nome           = "PM"
                    },
                    new Cargo
                    {
                        DataDeCadastro = DateTime.Now,
                        Nome           = "PO"
                    }
                };

                foreach(var cargo in cargos)
                {
                    _cargoRepositorio.Cadastrar(cargo);
                }

                await _cargoRepositorio.Salvar();

                var lista = await _cargoRepositorio.ObterTodos();

                Assert.IsNotNull(lista);

                Assert.IsTrue(lista.Count() >= 2);

                Assert.IsNotNull(lista.FirstOrDefault(cargo => cargo.Id == cargos[0].Id));
                Assert.IsNotNull(lista.FirstOrDefault(cargo => cargo.Id == cargos[1].Id));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString()); 
            }
            finally
            {
                if(cargos != null)
                {
                    foreach (var cargo in cargos)
                    {
                        _cargoRepositorio.Excluir(cargo);
                    }

                    await _cargoRepositorio.Salvar();
                }
            }
        }

        [TestMethod]
        public async Task TestarCargoConsultaPorId()
        {
            Cargo? cargo = null;

            try
            {
                cargo = new Cargo
                {
                    DataDeCadastro = DateTime.Now,
                    Nome           = "Analista de Teste Sênior"
                };

                _cargoRepositorio.Cadastrar(cargo);
                await _cargoRepositorio.Salvar();

                Assert.IsTrue(cargo.Id > 0); 
                Assert.IsNotNull(_cargoRepositorio.ObterPorId(cargo.Id));
            }
            catch (Exception ex) 
            { 
                Assert.Fail(ex.Message); 
            } 
            finally
            {
                if(cargo != null)
                {
                    _cargoRepositorio.Excluir(cargo);
                    await _cargoRepositorio.Salvar();
                }
            }
        }
    }
}
