// Server/DTO/Product/UpdateProductDto.cs
using System.ComponentModel.DataAnnotations;

namespace ClientPart.Dto.Product
{
    /// <summary>DTO для обновления существующего продукта</summary>
    public class UpdateProductDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
