using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryApp.Models
{
    [Table("Asortiment")]
    public class Asortimet
    {
        public int Id { get; set; }
        [Required]
        public string? Image { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public int Cost { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; } = new Category();
        public List<GoodsDetail>? GoodsDetails { get; set; }
        public List<CartDetail>? CartDetail { get; set; }

        [NotMapped]
        public string? CategoryName { get; set; } 
    }
}
