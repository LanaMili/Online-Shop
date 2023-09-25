using Spg.FlowerShop.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.DataTransferObjects
{
    public record ProductDto
    {
        [Required(ErrorMessage = "Der Produktname muss angegeben werden.")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Der Produktname ist zwischen 8 und 32 Stellen lang.")]
        public string ProductName { get; set; } = string.Empty;

        public decimal CurrentPrice { get; set; } = 0;

        [Required(ErrorMessage = "Ean muss angegeben werden.")]
        [StringLength(13, MinimumLength =13)]
        public string Ean { get; set; } = string.Empty;

        public string ProductImage { get; set; } = string.Empty;

        public Guid ProductCategoryID { get; set; } = default!;
    }
}
