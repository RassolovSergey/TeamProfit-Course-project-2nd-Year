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
        public Task<TeamDto> CreateAsync(CreateTeamDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TeamDto>> GetAllAsync()
        {
            throw new NotImplementedException();
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
