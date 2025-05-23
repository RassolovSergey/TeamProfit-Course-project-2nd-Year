using AutoMapper;
using Data.Entities;
using Server.DTO.Currency;
using Server.DTO.Currency.Server.DTO.Currency;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    /// <summary>
    /// Реализация сервиса для работы с валютами
    /// </summary>
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _repo;
        private readonly IMapper _mapper;

        public CurrencyService(ICurrencyRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CurrencyDto>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return _mapper.Map<List<CurrencyDto>>(entities);
        }

        public async Task<CurrencyDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity is null
                ? null
                : _mapper.Map<CurrencyDto>(entity);
        }

        public async Task<CurrencyDto> CreateAsync(CreateCurrencyDto dto)
        {
            var entity = _mapper.Map<Currency>(dto);
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return _mapper.Map<CurrencyDto>(entity);
        }

        public async Task<CurrencyDto?> UpdateAsync(int id, UpdateCurrencyDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            _mapper.Map(dto, entity);
            await _repo.UpdateAsync(entity);
            await _repo.SaveChangesAsync();
            return _mapper.Map<CurrencyDto>(entity);
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
