namespace Kursova.Models
{
    public class OrdersEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
        public string Orderer { get; set; }
        public string Deliver { get; set; }
        public bool Taken { get; set; }
    }
}
