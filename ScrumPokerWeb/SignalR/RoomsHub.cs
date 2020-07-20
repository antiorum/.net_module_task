using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ScrumPokerWeb.Services;

namespace ScrumPokerWeb.SignalR
{
    [Authorize]
    public class RoomsHub : Hub
    {
        private UserService service;

        public RoomsHub(UserService service)
        {
            this.service = service;
        }


        public override Task OnConnectedAsync()
        {
            service.AddUserToConnectionMap(Context.User.Identity.Name, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            service.DeleteUserFromConnectionMap(Context.User.Identity.Name);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
