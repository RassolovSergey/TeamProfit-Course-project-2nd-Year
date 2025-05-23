using AutoMapper;
using Data.Entities;
using Server.DTO.Sale;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    /// <summary>Сервис для продаж</summary>
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _repo;
        private readonly IMapper _mapper;

        public SaleService(ISaleRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<SaleDto>> GetAllAsync()
        {
            var sales = await _repo.GetAllWithRewardsAsync();
            return _mapper.Map<List<SaleDto>>(sales);
        }

        public async Task<List<SaleDto>> GetByProjectAsync(int projectId)
        {
            var sales = await _repo.GetAllWithRewardsAsync();
            // Фильтруем продажи по проекту через Reward.ProjectId
            return sales
                .Where(s => s.Reward != null && s.Reward.ProjectId == projectId)
                .Select(_mapper.Map<SaleDto>)
                .ToList();
        }

        public async Task<SaleDto?> GetByIdAsync(int id)
        {
            var sale = await _repo.GetByIdAsync(id);
            return sale == null ? null : _mapper.Map<SaleDto>(sale);
        }

        public async Task<SaleDto> CreateAsync(CreateSaleDto dto)
        {
            var sale = _mapper.Map<Sale>(dto);
            await _repo.AddAsync(sale);
            await _repo.SaveChangesAsync();
            return _mapper.Map<SaleDto>(sale);
        }

        public async Task<SaleDto?> UpdateAsync(int id, UpdateSaleDto dto)
        {
            var sale = await _repo.GetByIdAsync(id);
            if (sale == null) return null;
            _mapper.Map(dto, sale);
            await _repo.UpdateAsync(sale);
            await _repo.SaveChangesAsync();
            return _mapper.Map<SaleDto>(sale);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exists = await _repo.GetByIdAsync(id) != null;
            if (!exists) return false;
            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
