﻿using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace WebsiteCustomerChatMVC.SignarR.Hubs
{
    public class AdminChatHub : Hub
    {
        private bool Authorized(string Token)
        {
            
            return false;

        }

        public async Task NewClient(string SyncData)
        {
           await Clients.All.SendAsync("NewClient", "new client");
        }


        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;


  
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            
            string connectionId = Context.ConnectionId;



            await base.OnDisconnectedAsync(exception);
        }
    }
}