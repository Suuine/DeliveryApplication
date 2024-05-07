using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryApp.Models
{
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string? NameCategory { get; set; }
        public List<Asortimet>? Asortimets { get; set;} = new List<Asortimet>();
    }
}
