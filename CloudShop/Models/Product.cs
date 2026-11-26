using System.ComponentModel.DataAnnotations;

namespace CloudShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название товара")]
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}
