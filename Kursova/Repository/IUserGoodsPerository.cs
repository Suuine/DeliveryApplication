using DeliveryApp.Models;

namespace DeliveryApp
{
    public interface IUserGoodsPerository
    {
        Task<IEnumerable<Goods>> UserGoods();
    }
}