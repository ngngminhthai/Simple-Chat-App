﻿using ChatApp.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DemoSignalR
{
    public class signalrServer : Hub
    {
        public readonly static List<User> _Connections = new List<User>();

        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();

        private static readonly Dictionary<string, List<string>> _groups = new Dictionary<string, List<string>>();


        private readonly ChatDbContext _context;

        public signalrServer(ChatDbContext context)
        {
            _context = context;
        }
        private string IdentityName
        {
            get { return Context.User.Identity.Name; }
        }

        public async Task Join(string roomName)
        {
            try
            {
                var user = _Connections.Where(u => u.UserName == IdentityName).FirstOrDefault();
                if (user != null && user.Room != roomName)
                {
                    if (!string.IsNullOrEmpty(user.Room))
                        await Clients.OthersInGroup(user.Room).SendAsync("removeUser", user);

                    await Leave(user.Room);
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

                    user.Room = roomName;

                    await Clients.OthersInGroup(roomName).SendAsync("addUser", user);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public async Task Leave(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
        public override Task OnConnectedAsync()
        {
            try
            {
                var user = _context.Users.Where(u => u.UserName == IdentityName).FirstOrDefault();

                if (!_Connections.Any(u => u.UserName == IdentityName))
                {
                    user.Room = "";
                    _Connections.Add(user);
                    _ConnectionsMap.Add(IdentityName, Context.ConnectionId);
                }
                Clients.All.SendAsync("Connected", string.Join("|", _ConnectionsMap.Keys));
                Clients.Caller.SendAsync("getProfileInfo", user.UserName);
            }
            catch (Exception ex)
            {
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = _Connections.Where(u => u.UserName == IdentityName).First();
                _Connections.Remove(user);

                Clients.OthersInGroup(user.Room).SendAsync("removeUser", user);

                _ConnectionsMap.Remove(user.UserName);
                Clients.All.SendAsync("Disconnected", string.Join("|", user.UserName));

            }
            catch (Exception ex)
            {
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMsg(string ConversationName, string content, string isGroupChat, string groupId)
        {
            var user = await _context.Users.Where(u => u.UserName == IdentityName).FirstOrDefaultAsync();
            if (Boolean.Parse(isGroupChat))
                await _context.Chats.AddAsync(new Chat { Message = content, Sender = user, GroupChatId = Int32.Parse(groupId) });
            else
                await _context.Chats.AddAsync(new Chat { Message = content, Sender = user, IndividualChatId = Int32.Parse(groupId) });
            await _context.SaveChangesAsync();
            await Clients.Group(ConversationName).SendAsync("ReceiveMessage", content, user.Id, user.UserName);
        }
    }
}
