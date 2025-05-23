// Server/DTO/Product/ProductDto.cs
namespace Server.DTO.Product
{
    /// <summary>DTO для передачи информации о продукте</summary>
    public class ProductDto
    {
        /// <summary>Идентификатор продукта</summary>
        public int Id { get; set; }

        /// <summary>Название продукта</summary>
        public string Name { get; set; } = null!;

        /// <summary>Список идентификаторов связанных наград</summary>
        public List<int>? RewardIds { get; set; }
    }
}
