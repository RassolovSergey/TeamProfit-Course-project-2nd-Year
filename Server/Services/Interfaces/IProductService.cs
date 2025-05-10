using Server.DTO.Product;
using Server.DTO.Project;

namespace Server.Services.Interfaces
{
    public interface IProductService
    {
        /// <summary>Получить список всех товаров</summary>
        Task<List<ProductDto>> GetAllAsync();

        /// <summary>Получить товар по идентификатору или null, если не найден</summary>
        Task<ProductDto?> GetByIdAsync(int id);

        /// <summary>Создать новый товар и вернуть его DTO</summary>
        Task<ProductDto> CreateAsync(CreateProductDto dto);

        /// <summary>Обновить существующий товар, вернуть обновлённый DTO или null, если не найдено</summary>
        Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto);

        /// <summary>Удалить товар по идентификатору, вернуть true, если успешно</summary>
        Task<bool> DeleteAsync(int id);
    }
}
