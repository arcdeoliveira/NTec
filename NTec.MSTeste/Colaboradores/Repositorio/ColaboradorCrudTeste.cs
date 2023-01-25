using Microsoft.Extensions.DependencyInjection;
using NTec.Domain.Contratos.Repositorios;
using NTec.Domain.Entidades;
using NTec.Domain.Enums;

namespace NTec.MSTeste.Colaboradores.Repositorio
{
    [TestClass]
    public class ColaboradorCrudTeste
    {
        private readonly ICargoRepositorio _cargoRepositorio;
        private readonly IColaboradorRepositorio _colaboradorRepositorio;
        private readonly ISetorRepositorio _setorRepositorio;

        public ColaboradorCrudTeste()
        {
            var servicos = Provider.ObterProvedoresdeServico();

            _cargoRepositorio       = servicos.GetRequiredService<ICargoRepositorio>();   
            _colaboradorRepositorio = servicos.GetRequiredService<IColaboradorRepositorio>();
            _setorRepositorio       = servicos.GetRequiredService<ISetorRepositorio>();
        }

        [TestMethod]
        public async Task TestarCadastrarColaborador()
        {
            Colaborador? colaborador = null;
            Cargo? cargo = null;
            Setor? setor = null;

            try
            {
                cargo = await ObterCargo();
                setor = await ObterSetor();

                colaborador = new Colaborador
                {
                    Aniversario    = new DateTime(1978,7,11),
                    Cargo          = cargo,
                    Cpf            = 12345678901,
                    DataDeCadastro = DateTime.Now,
                    Genero         = GeneroEnum.Masculino,
                    Nome           = "Sergio",
                    Setor          = setor,
                    SobreNome      = "Malandro"
                };

                _colaboradorRepositorio.Cadastrar(colaborador);
                await _colaboradorRepositorio.Salvar();

                Assert.IsNotNull(colaborador.Id.ToString());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                await DeletarEntidades(cargo, colaborador, null, setor);               
            }
        }

        [TestMethod]
        public async Task TestarCadastrarColaboradorPreenchendoChefeEntidade()
        {
            Cargo? cargo             = null;
            Colaborador? chefe       = null;
            Colaborador? colaborador = null;
            Setor? setor             = null;

            try
            {
                cargo = await ObterCargo();
                chefe = await ObterChefe();
                setor = await ObterSetor();

                colaborador = new Colaborador
                {
                    Aniversario    = new DateTime(1965, 2, 5),
                    Cargo          = cargo,
                    Chefe          = chefe,
                    Cpf            = 12345678901,
                    DataDeCadastro = DateTime.Now,
                    Genero         = GeneroEnum.NaoInformar,
                    Nome           = "Mathias",
                    Setor          = setor,
                    SobreNome      = "Vieira de Souto"
                };

                _colaboradorRepositorio.Cadastrar(colaborador);
                await _colaboradorRepositorio.Salvar();

                Assert.IsNotNull(colaborador.Id.ToString());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                await DeletarEntidades(cargo, colaborador, chefe, setor);
            }
        }

        [TestMethod]
        public async Task TestarExcluirColaborador()
        {
            Cargo? cargo = null;
            Setor? setor = null;

            try
            {
                var colaborador = await ObterChefe();

                cargo = colaborador.Cargo;
                setor = colaborador.Setor;

                Assert.IsNotNull(colaborador.Id.ToString());

                _colaboradorRepositorio.Excluir(colaborador);
                await _colaboradorRepositorio.Salvar();

                Assert.IsNotNull(colaborador);

                var colaboradorCadastrado = await _colaboradorRepositorio.ObterPorId(colaborador.Id);

                Assert.IsNull(colaboradorCadastrado);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                await DeletarEntidades(cargo, null, null, setor);
            }
        }

        [TestMethod]
        public async Task TestarAtualizarColaborador()
        {
            Cargo? cargo             = null;
            Colaborador? colaborador = null;
            Colaborador? novoChefe   = null;
            Setor? setor             = null;

            try
            {
                colaborador = await ObterChefe();
                cargo       = colaborador.Cargo;
                setor       = colaborador.Setor;              

                var alteradoPorNoCadastro     = colaborador.AlteradoPor;
                var dataAtualizacaoNoCadastro = colaborador.DataDeAtualizacao;
                var genero                    = GeneroEnum.NaoInformar;
                var antigoChefe               = colaborador.Chefe;

                colaborador.AlteradoPor       = "John Oliveira";
                colaborador.DataDeAtualizacao = DateTime.Now.AddDays(1);
                colaborador.Genero            = genero;

                novoChefe = new Colaborador
                {
                    Aniversario    = new DateTime(1995, 3, 29),
                    DataDeCadastro = DateTime.Now,
                    CargoId        = cargo.Id,
                    Cpf            = 12365478998,
                    Foto           = "foto.gif",
                    Genero         = GeneroEnum.Masculino,
                    Nome           = "Mauro",
                    SetorId        = setor.Id,
                    SobreNome      = "Da Silva"
                };

                colaborador.Chefe = novoChefe;

                _colaboradorRepositorio.Cadastrar(novoChefe);
                _colaboradorRepositorio.Atualizar(colaborador);

                await _colaboradorRepositorio.Salvar();

                var colaboradorAtualizado = await _colaboradorRepositorio.ObterPorId(colaborador.Id);

                Assert.IsNotNull(colaboradorAtualizado);

                Assert.IsFalse(colaboradorAtualizado.Excluido);
                Assert.IsFalse(colaboradorAtualizado.DataDeExclusao.HasValue);

                Assert.IsNull(colaboradorAtualizado.ExcluidoPor);

                Assert.AreEqual(colaborador.Id, colaboradorAtualizado.Id);
                Assert.AreEqual(colaborador.DataDeCadastro, colaboradorAtualizado.DataDeCadastro);
                Assert.AreEqual(colaborador.Nome, colaboradorAtualizado.Nome);
                Assert.AreEqual(genero, colaborador.Genero);

                Assert.AreNotEqual(alteradoPorNoCadastro, colaboradorAtualizado.AlteradoPor);
                Assert.AreNotEqual(dataAtualizacaoNoCadastro, colaboradorAtualizado.DataDeAtualizacao);
                Assert.AreNotEqual(antigoChefe, colaborador.Chefe);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                await DeletarEntidades(cargo, colaborador, novoChefe, setor);
            }
        }

        [TestMethod]
        public async Task TestarConsultarColaboradoes()
        {
            Cargo? cargo = null;
            Setor? setor = null;

            List<Colaborador>? colaboradores = null;

            try
            {
                cargo         = await ObterCargo();
                setor         = await ObterSetor();
                colaboradores = new List<Colaborador>
                {
                    new Colaborador
                    {
                        Aniversario    = new DateTime(2000, 3, 29),
                        DataDeCadastro = DateTime.Now,
                        CargoId        = cargo.Id,
                        Cpf            = 12365478998,
                        Foto           = "foto.gif",
                        Genero         = GeneroEnum.Masculino,
                        Nome           = "Gerverson",
                        SetorId        = setor.Id,
                        SobreNome      = "Marques Nunes"
                    },
                    new Colaborador
                    {
                        Aniversario    = new DateTime(1974, 3, 2),
                        DataDeCadastro = DateTime.Now,
                        CargoId        = cargo.Id,
                        Cpf            = 12365478998,
                        Foto           = "foto.gif",
                        Genero         = GeneroEnum.Masculino,
                        Nome           = "Mauro",
                        SetorId        = setor.Id,
                        SobreNome      = "Da Silva"
                    },
                };

                foreach (var colaborador in colaboradores)
                {
                    _colaboradorRepositorio.Cadastrar(colaborador);
                }

                await _colaboradorRepositorio.Salvar();

                var lista = await _colaboradorRepositorio.ObterTodos();

                Assert.IsNotNull(lista);

                Assert.IsTrue(lista.Count() >= 3);

                Assert.IsNotNull(lista.FirstOrDefault(f => f.Id == colaboradores[0].Id));
                Assert.IsNotNull(lista.FirstOrDefault(f => f.Id == colaboradores[1].Id));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
            finally
            {
                if (colaboradores != null)
                {
                    foreach (var colaborador in colaboradores)
                    {
                        _colaboradorRepositorio.Excluir(colaborador);
                    }

                    await _colaboradorRepositorio.Salvar();
                }

                await DeletarEntidades(cargo, null, null, setor);
            }
        }

        [TestMethod]
        public async Task TestarColaboradorConsultaPorId()
        {
            Cargo? cargo             = null;
            Colaborador? colaborador = null;
            Setor? setor             = null;
            

            try
            {
                cargo       = await ObterCargo();
                setor       = await ObterSetor();
                colaborador = new Colaborador
                {
                    Aniversario    = new DateTime(2000, 3, 29),
                    DataDeCadastro = DateTime.Now,
                    CargoId        = cargo.Id,
                    Cpf            = 12365478998,
                    Foto           = "foto.gif",
                    Genero         = GeneroEnum.Masculino,
                    Nome           = "Gerverson",
                    SetorId        = setor.Id,
                    SobreNome      = "Marques Nunes"
                };

                _colaboradorRepositorio.Cadastrar(colaborador);
                await _colaboradorRepositorio.Salvar();

                Assert.IsNotNull(colaborador.Id.ToString());
                Assert.IsNotNull(_colaboradorRepositorio.ObterPorId(colaborador.Id));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                await DeletarEntidades(cargo, colaborador, null, setor);
            }
        }

        private async Task<Cargo> ObterCargo()
        {
            var cargo = new Cargo { DataDeCadastro = DateTime.Now, Nome = "Analista de Qualidade" };

            _cargoRepositorio.Cadastrar(cargo);
            await _cargoRepositorio.Salvar();

            return cargo;
        }

        private async Task<Setor> ObterSetor()
        {
            var setor = new Setor { DataDeCadastro = DateTime.Now, Nome = "Recursos Humanos" };

            _setorRepositorio.Cadastrar(setor);
            await _cargoRepositorio.Salvar();

            return setor;
        }

        private async Task<Colaborador> ObterChefe()
        {
            var colaborador = new Colaborador
            {
                Aniversario    = new DateTime(1988, 12, 16),
                Cargo          = await ObterCargo(),
                Cpf            = 09876543213,
                DataDeCadastro = DateTime.Now,
                Foto           = "fotoPrincipal.png",
                Genero         = GeneroEnum.Feminino,
                Nome           = "Maiara",
                Setor          = await ObterSetor(),
                SobreNome      = "Avezedo da Silva"
            };

            _colaboradorRepositorio.Cadastrar(colaborador);
            await _colaboradorRepositorio.Salvar();

            return colaborador;
        }

        private async Task DeletarEntidades(Cargo? cargo, Colaborador? colaborador, Colaborador? chefe, Setor? setor)
        {
            if (chefe != null)
            {
                _colaboradorRepositorio.Excluir(chefe);
            }

            if (colaborador != null)
            {
                _colaboradorRepositorio.Excluir(colaborador);
            }

            if (cargo != null)
            {
                _cargoRepositorio.Excluir(cargo);
            }

            if (setor != null)
            {
                _setorRepositorio.Excluir(setor);
            }

            await _cargoRepositorio.Salvar();
        }
    }
}
