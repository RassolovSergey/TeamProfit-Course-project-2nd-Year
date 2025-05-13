using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Server.DTO.UserProject;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    /// <summary>
    /// Бизнес-логика для работы с параметрами сотрудничества пользователя в проекте
    /// </summary>
    public class UserProjectService : IUserProjectService
    {
        private readonly IUserProjectRepository _repo;

        public UserProjectService(IUserProjectRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Получить все записи UserProject
        /// </summary>
        public async Task<List<UserProjectDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list
                .Select(up => new UserProjectDto
                {
                    UserId = up.UserId,
                    ProjectId = up.ProjectId,
                    TypeCooperation = up.TypeCooperation,
                    FixedPrice = up.FixedPrice,
                    PercentPrice = up.PercentPrice
                })
                .ToList();
        }

        /// <summary>
        /// Получить одну запись по составному ключу (userId + projectId)
        /// </summary>
        public async Task<UserProjectDto?> GetAsync(int userId, int projectId)
        {
            var up = await _repo.GetByIdsAsync(userId, projectId);
            if (up is null) return null;

            return new UserProjectDto
            {
                UserId = up.UserId,
                ProjectId = up.ProjectId,
                TypeCooperation = up.TypeCooperation,
                FixedPrice = up.FixedPrice,
                PercentPrice = up.PercentPrice
            };
        }

        /// <summary>
        /// Создать новую запись UserProject
        /// </summary>
        public async Task<UserProjectDto> CreateAsync(CreateUserProjectDto dto)
        {
            var up = new UserProject
            {
                UserId = dto.UserId,
                ProjectId = dto.ProjectId,
                TypeCooperation = dto.TypeCooperation,
                FixedPrice = dto.FixedPrice,
                PercentPrice = dto.PercentPrice
            };

            await _repo.AddAsync(up);
            await _repo.SaveChangesAsync();

            return new UserProjectDto
            {
                UserId = up.UserId,
                ProjectId = up.ProjectId,
                TypeCooperation = up.TypeCooperation,
                FixedPrice = up.FixedPrice,
                PercentPrice = up.PercentPrice
            };
        }

        /// <summary>
        /// Обновить существующую запись по составному ключу
        /// </summary>
        public async Task<UserProjectDto?> UpdateAsync(int userId, int projectId, UpdateUserProjectDto dto)
        {
            var up = await _repo.GetByIdsAsync(userId, projectId);
            if (up is null) return null;

            up.TypeCooperation = dto.TypeCooperation;
            up.FixedPrice = dto.FixedPrice;
            up.PercentPrice = dto.PercentPrice;

            await _repo.UpdateAsync(up);
            await _repo.SaveChangesAsync();

            return new UserProjectDto
            {
                UserId = up.UserId,
                ProjectId = up.ProjectId,
                TypeCooperation = up.TypeCooperation,
                FixedPrice = up.FixedPrice,
                PercentPrice = up.PercentPrice
            };
        }

        /// <summary>
        /// Удалить запись по составному ключу
        /// </summary>
        public async Task<bool> DeleteAsync(int userId, int projectId)
        {
            var up = await _repo.GetByIdsAsync(userId, projectId);
            if (up is null) return false;

            await _repo.DeleteByIdsAsync(userId, projectId);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
