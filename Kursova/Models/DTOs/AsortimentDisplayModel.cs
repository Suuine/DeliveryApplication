namespace DeliveryApp.Models.DTOs
{
    public class AsortimentDisplayModel
    {
        public IEnumerable<Asortimet>? Asortiments { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
    }
}
