using DeliveryApp.Models;

namespace DeliveryApp
{
    public interface IDeliverReposity
    {
        Task<bool> TakeOrder(int goodsId);
        Task<bool> ApplyStatus(int orderId, int status);
    }
}