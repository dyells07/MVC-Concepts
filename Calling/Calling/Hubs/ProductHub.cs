using Microsoft.AspNetCore.SignalR;

namespace Calling.Hubs
{
    public class ProductHub : Hub
    {
        public static List<string> _products = new();
        public async Task SendProduct(string productName)
        {
            _products.Add(productName);
            await Clients.All.SendAsync("ReceiveProduct", productName, _products.Count());
        }
        public async Task ResetProduct()
        {
            _products.Clear();
            await Clients.All.SendAsync("ReceiveResetProduct");
        }
    }
}