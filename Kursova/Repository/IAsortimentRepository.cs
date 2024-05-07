using DeliveryApp.Models;

namespace DeliveryApp
{
    public interface IAsortimentRepository
    {
        Task<IEnumerable<Asortimet>> GetAsortiments(string sTerm = "", int catalogyId = 0);
        Task<IEnumerable<Category>> Categories();
    }
}