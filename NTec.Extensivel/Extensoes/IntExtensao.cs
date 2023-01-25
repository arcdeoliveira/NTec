namespace NTec.Helper.Aberto.Extensoes
{
    public static class IntExtensao
    {
        public static int Skip(this int numeroDapagina, int quantidadeDePagina)
        {
            return (numeroDapagina - 1) * quantidadeDePagina;
        }
    }
}
