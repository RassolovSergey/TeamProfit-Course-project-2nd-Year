using AutoMapper;
using Data.Entities;
using Server.DTO.Cost;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class CostService : ICostService
    {
        private readonly ICostRepository _repo;
        private readonly IMapper _mapper;

        public CostService(ICostRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CostDto>> GetAllAsync()
        {
            var costs = await _repo.GetAllAsync();
            return _mapper.Map<List<CostDto>>(costs);
        }

        public async Task<CostDto?> GetByIdAsync(int id)
        {
            var cost = await _repo.GetByIdAsync(id);
            return cost == null ? null : _mapper.Map<CostDto>(cost);
        }

        public async Task<CostDto> CreateAsync(CreateCostDto dto)
        {
            var cost = _mapper.Map<Cost>(dto);
            await _repo.AddAsync(cost);
            await _repo.SaveChangesAsync();
            return _mapper.Map<CostDto>(cost);
        }

        public async Task<CostDto?> UpdateAsync(int id, UpdateCostDto dto)
        {
            var cost = await _repo.GetByIdAsync(id);
            if (cost == null) return null;
            _mapper.Map(dto, cost);
            await _repo.UpdateAsync(cost);
            await _repo.SaveChangesAsync();
            return _mapper.Map<CostDto>(cost);
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
