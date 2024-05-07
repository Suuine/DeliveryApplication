using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryApp.Models
{
    [Table("GoodsDetail")]
    public class GoodsDetail
    {
        public int Id { get; set; }
        [Required]
        public int GoodsId { get; set; } 
        [Required]
        public int AsortimentId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double UnitPrice { get; set; }
        public Goods? Goods { get; set; } 
        public Asortimet? Asortimet { get; set; }
    }
}
