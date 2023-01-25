using NTec.Helper.Aberto.Extensoes;

namespace NTec.MSTeste.Extensoes
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TesteCaminhoFeliz()
        {
            try
            {
                //Arrange
                var pagina     = 3;
                var quantidade = 15;

                //Act
                var resultado = pagina.Skip(quantidade);

                //Assert
                Assert.IsNotNull(resultado);
                Assert.IsInstanceOfType(resultado, typeof(int));
                Assert.AreEqual(30, resultado);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void TestarPassandoValorZeroNosParametros()
        {
            try
            {
                //Arrange
                var pagina     = 0;
                var quantidade = 0;

                //Act
                var resultado = pagina.Skip(quantidade);

                //Assert
                //Assert
                Assert.IsNotNull(resultado);
                Assert.IsInstanceOfType(resultado, typeof(int));
                Assert.IsTrue(resultado == 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }
    }
}