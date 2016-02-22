using Microsoft.AspNet.SignalR;

namespace BoardGames.Web.Hubs
{
    public class Chat : Hub
    {
        public void SendMessage(string message)
        {
            var msg = string.Format("{0}: {1}", Context.ConnectionId, message);
            Clients.All.addMessage(msg);
        }
    }
}