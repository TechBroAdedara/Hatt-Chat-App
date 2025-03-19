using Microsoft.AspNetCore.SignalR;

namespace Hatt.Hubs;

public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }


}