// Server/DTO/Product/UpdateProductDto.cs
using System.ComponentModel.DataAnnotations;

namespace Server.DTO.Product
{
    /// <summary>DTO для обновления существующего продукта</summary>
    public class UpdateProductDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
