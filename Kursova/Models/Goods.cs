using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryApp.Models
{
    [Table("Goods")]
    public class Goods
    {
        public int Id { get; set; }
        [Required]
        public string? UserId { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        [Required]
        public int GoodsStatusId { get; set; }
        public GoodsStatus? GoodsStatus { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<GoodsDetail> OrderDetails { get; set; } = new List<GoodsDetail>();
        public string? NameUser { get; set; }
        public int? DeliverOrdersID { get; set; }
        public DeliverOrders? DeliverOrders { get; set; }
    }
}
