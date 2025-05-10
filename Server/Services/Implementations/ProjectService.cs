using Server.DTO.Project;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        public Task<ProjectDto> CreateAsync(CreateProjectDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProjectDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProjectDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectDto?> UpdateAsync(int id, UpdateProjectDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
