using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataService;
using DataService.Models;
using Microsoft.AspNetCore.SignalR;
using ScrumPokerWeb.DTO;
using ScrumPokerWeb.SignalR;

namespace ScrumPokerWeb.Services
{
    public class UserService
    {
        private Dictionary<string, string> userWithSignalRIds;
        IRepository<User> repository;
        private IHubContext<RoomsHub> context;

        public UserService(IRepository<User> repository, IHubContext<RoomsHub> context)
        {
            this.repository = repository;
            this.context = context;
            userWithSignalRIds = new Dictionary<string, string>();
        }

        public User GetByName(string name)
        {
            return repository.GetAll().FirstOrDefault(u => u.Name == name);
        }

        public void Create(User user)
        {
            repository.Create(user);
        }

        public IEnumerable<UserDto> GetAll()
        {
            return DtoUtil.GetUsersTOs(repository.GetAll());
        }

        public void AddUserToConnectionMap(string user, string signalRId)
        {
            if (!userWithSignalRIds.ContainsKey(user))
            {
                userWithSignalRIds.Add(user, signalRId);
            }
        }

        public void DeleteUserFromConnectionMap(string identityName)
        {
            userWithSignalRIds.Remove(identityName);
        }

        public string GetConnectionIdNyName(string username)
        {
            return userWithSignalRIds
                .Where(pair => pair.Key == username)
                .Select(pair => pair.Value)
                .FirstOrDefault();
        }
    }
}
