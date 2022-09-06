using ECommerceApi.Adapters;
using ECommerceApi.Controllers;
using ECommerceApi.Models;

namespace ECommerceApi.Domain;

public class OrderProcessor
{
    private readonly MongoDbOrdersAdapter _adapter;

    public OrderProcessor(MongoDbOrdersAdapter adapter)
    {
        _adapter = adapter;
    }

    public async Task<OrderResponse> ProcessOrder(OrderRequest order)
    {

        var response = new OrderResponse
        {
            OrderId = Guid.NewGuid().ToString(),
            Customer = order.Customer,
            ExpectedShipDate = DateTime.Now.AddDays(3),
            OrderTotal = order.Items.Sum(o => o.Price * o.Qty)
        };

        var orderToSave = new OrderEntity
        {
            Id = response.OrderId,
            Customer = order.Customer,
            ExpectedShipDate = response.ExpectedShipDate,
            Items = order.Items,
            OrderTotal = response.OrderTotal
        };
        await _adapter.Orders.InsertOneAsync(orderToSave);
      
        return response;
    }
}
