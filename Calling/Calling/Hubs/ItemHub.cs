using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Calling.Models; // Import the ProductModel namespace

namespace Calling.Hubs
{
    public class ItemHub : Hub
    {
        public async Task SendProduct(ProductModel product)
        {
            await Clients.All.SendAsync("ReceiveItem", product);
        }

        public async Task ResetProduct()
        {
            
            await Clients.All.SendAsync("ReceiveResetProduct");
        }
    }
}
