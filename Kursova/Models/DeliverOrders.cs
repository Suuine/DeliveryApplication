using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryApp.Models
{
    [Table("DeliverOrders")]
    public class DeliverOrders
    {
        public int Id { get; set; }
        public string? DelivererId { get; set; }
        public string? NameDeliverer { get; set; }
        public List<Goods> Goods { get; set; } = new List<Goods>();
    }
}
