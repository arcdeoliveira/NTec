using NTec.Domain.Dtos.Colaboradores;
using NTec.Domain.Entidades;
using System.Linq;

namespace NTec.Infra.Filtros
{
    public static class FiltroColaboradorPaginacao
    {
        public static IQueryable<Colaborador> Filtrar(IQueryable<Colaborador> queryable, FiltroColaboradorPaginacaoDto filtroDto)
        {
            if (queryable == null)
            {
                return queryable;
            }

            queryable = queryable.Where(w => !w.Excluido);

            if (filtroDto.Cpf.HasValue)
            {
                queryable = queryable.Where(w => w.Cpf.Equals(filtroDto.Cpf));
            }

            if (filtroDto.CargoId.HasValue)
            {
                queryable = queryable.Where(w => w.CargoId == filtroDto.CargoId.Value);
            }

            if (filtroDto.Chefe.HasValue)
            {
                queryable = filtroDto.Chefe.Value
                    ? queryable.Where(w => w.Subordinados.Any())
                    : queryable.Where(w => !w.Subordinados.Any());
            }

            if (filtroDto.Genero.HasValue)
            {
                queryable = queryable.Where(w => w.Genero == filtroDto.Genero.Value);
            }

            if (!string.IsNullOrEmpty(filtroDto.Nome))
            {
                queryable = queryable.Where(w => w.Nome.Contains(filtroDto.Nome) || w.SobreNome.Contains(filtroDto.Nome));
            }

            if (filtroDto.SetorId.HasValue)
            {
                queryable = queryable.Where(w => w.SetorId == filtroDto.SetorId.Value);
            }

            return queryable;
        }
    }
}
