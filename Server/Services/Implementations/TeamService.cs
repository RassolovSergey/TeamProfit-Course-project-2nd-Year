using Server.DTO.Team;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    /// <summary>
    /// Реализация ITeamService: инжектит репозиторий и содержит бизнес-логику по работе с командами
    /// </summary>
    public class TeamService : ITeamService
    {

        private readonly ITeamRepository _repo;
        public TeamService(ITeamRepository repo)
        {
            _repo = repo; // репозиторий данных
        }

        public Task<TeamDto> CreateAsync(CreateTeamDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TeamDto>> GetAllAsync()
        {
            var teams = await _repo.GetAllAsync(); // Получаем сущности Team

            // Маппим в DTO
            return teams.Select(t => new TeamDto
            {
                Id = t.Id,
                Name = t.Name,
                AdminId = t.AdminId,
                MembersCount = t.Users?.Count ?? 0
            }).ToList();
        }

        public Task<TeamDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TeamDto?> UpdateAsync(int id, UpdateTeamDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
