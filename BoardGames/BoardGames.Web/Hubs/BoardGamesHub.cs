using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using System.Web.Script.Serialization;
using BoardGames.Data.Common.Repository;
using BoardGames.Models;
using Microsoft.AspNet.SignalR;
using BoardGames.Logic;
using System.Collections.Concurrent;

namespace BoardGames.Web.Hubs
{
    public class BoardGamesHub : Hub
    {
        private static IDictionary<string, string> connectionIdToRoomName = new ConcurrentDictionary<string, string>();
        private static IDictionary<string, string> connectionIdToUsername = new ConcurrentDictionary<string, string>();
        private static IDictionary<string, GameState> roomNameToGameState = new ConcurrentDictionary<string, GameState>();

        public override Task OnConnected()
        {
            return Task.WhenAll(base.OnConnected(), this.JoinRoom());
        }

        public override Task OnDisconnected()
        {
            return Task.WhenAll(base.OnDisconnected(), this.LeaveRoom());
        }

        public void MakeMove(int pawnIndex)
        {
            if (this.IsGameStarted() && this.GetGameState().MakeMove(pawnIndex, Context.ConnectionId))
            {
                this.SendState();
            }
            else
            {
                this.Clients.Caller.invalidMove();
            }
        }

        public void SendChatMessage(string message)
        {
            this.SendMessage(string.Format("{0}: {1}", this.GetUserName(), message));
        }

        private async Task JoinRoom()
        {
            var repository = (IRepository<User>)DependencyResolver.Current.GetService(typeof(IRepository<User>));
            var username = GetUserName();
            var room = repository.All().Where(user => user.UserName == username).Single().Room;
            connectionIdToRoomName.Add(Context.ConnectionId, room.Name);
            connectionIdToUsername.Add(Context.ConnectionId, username);
            await Groups.Add(Context.ConnectionId, room.Name);
            this.SendMessage(string.Format("{0} joined.", this.GetUserName()));
            if (room.IsFull())
            {
                var connectionIds = connectionIdToRoomName.Keys.Where(x => connectionIdToRoomName[x] == room.Name).ToArray();
                var usernames = connectionIds.Select(x => connectionIdToUsername[x]).ToArray();
                roomNameToGameState[room.Name] = new GameState(connectionIds, usernames);
                this.SendState();
            }
        }

        private async Task LeaveRoom()
        {
            this.SendMessage(string.Format("{0} left.", this.GetUserName()));
            this.GetGameState().RemovePlayer(Context.ConnectionId);
            this.SendState();
            await Groups.Remove(Context.ConnectionId, connectionIdToRoomName[Context.ConnectionId]);
            connectionIdToRoomName.Remove(Context.ConnectionId);
            connectionIdToUsername.Remove(Context.ConnectionId);
        }

        private void SendState()
        {
            this.GetGroup().updateState(this.GetGameState());
        }

        private void SendMessage(string message)
        {
            this.GetGroup().addMessage(message);
        }

        private dynamic GetGroup()
        {
            return Clients.Group(connectionIdToRoomName[Context.ConnectionId]);
        }

        private string GetUserName()
        {
            return Context.User.Identity.Name;
        }

        private GameState GetGameState()
        {
            return roomNameToGameState[connectionIdToRoomName[Context.ConnectionId]];
        }

        private bool IsGameStarted()
        {
            return this.GetGameState() != null;
        }
    }
}