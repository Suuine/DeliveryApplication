using DeliveryApp.Models;

namespace DeliveryApp.Repository
{
    public interface IAdminRepository
    {
        Task<List<Goods>> GetGoodsInDateRange(DateTime dateTime1, DateTime dateTime2);
        Task<List<Goods>> GetGoodsAsync();
        Task<List<UserViewModel>> DisplayUsers();
        Task<List<ApplicationUser>> GetUsersAsync();
    }
}
