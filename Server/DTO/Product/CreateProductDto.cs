// Server/DTO/Product/CreateProductDto.cs
using System.ComponentModel.DataAnnotations;

namespace Server.DTO.Product
{
    /// <summary>DTO для создания нового продукта</summary>
    public class CreateProductDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
