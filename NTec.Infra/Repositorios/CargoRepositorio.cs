using Microsoft.EntityFrameworkCore;
using NTec.Domain.Contratos.Repositorios;
using NTec.Domain.Entidades;
using NTec.Infra.Contexto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using NTec.Domain.Dtos.Comum;
using NTec.Domain.Dtos.Cargos;
using AutoMapper;

namespace NTec.Infra.Repositorios
{
    public class CargoRepositorio : BaseRepositorio<Cargo>, ICargoRepositorio
    {
        private readonly IMapper _mapper;

        public CargoRepositorio(IMapper mapper, DataContexto contexto) : base(contexto)
        {
            _mapper = mapper;
        }

        public async Task<bool> CargoExiste(int cargoId)
        {
            return await _contexto.Cargos.AsNoTracking().AnyAsync(a => a.Id == cargoId);  
        }

        /// <summary>
        /// Verificar se o cargo a cadastrar ou atualizar, não é um cargo já cadastrado
        /// </summary>
        /// <param name="id">Id para verificar ao atualizar se o cargo nome existe além do cargo atual</param>
        /// <param name="nome">Nome do cargo a verificar</param>
        /// <returns>Se cargo já esta cadastrado ou não.</returns>
        public async Task<bool> CargoDuplicado(string nome, int? id = null)
        {
            return id.HasValue
                ? await _contexto.Cargos.AsNoTracking().AnyAsync(a => a.Nome.ToLower().Equals(nome) && a.Id != id.Value)
                : await _contexto.Cargos.AsNoTracking().AnyAsync(a => a.Nome.ToLower().Equals(nome));
        }

        public async Task<IEnumerable<object>> ObterCargosOrdenadoPorNome()
        {
            var queryable = Queryable();

            return await
            (from cargo in queryable
             where !cargo.Excluido
             orderby cargo.Nome
             select new { cargo.Id, cargo.Nome }).ToListAsync();
        }

        public async Task<PaginacaoColecaoDto<CargoPaginacaoDto>> ObterCargosPaginados(string nomeCargo, int skip, int take)
        {
            var query = _contexto.Cargos.Include(i => i.Colaboradores).AsNoTracking().Where(w => !w.Excluido);
            if (!string.IsNullOrEmpty(nomeCargo))
            {
                query = query.Where(w => w.Nome.ToLower().Contains(nomeCargo.ToLower()));
            }

            var total   = await query.CountAsync();
            var objetos = await  query
                .OrderByDescending(o => o.Id)
                .Skip(skip)
                .Take(take)
                .Select(cargo => _mapper.Map<CargoPaginacaoDto>(cargo))
                .ToListAsync();

            return new PaginacaoColecaoDto<CargoPaginacaoDto>(objetos, total);    
        }
    }
}
