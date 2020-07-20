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
    public class CardService
    {
        private IRepository<Card> repository;
        private IHubContext<RoomsHub> context;
        private UserService service;

        public CardService(IRepository<Card> repository, IHubContext<RoomsHub> context, UserService service)
        {
            this.repository = repository;
            this.context = context;
            this.service = service;
        }

        public IEnumerable<CardDto> GetAvailable(string owner)
        {
            List<Card> cards = repository.GetAll().Where(c => c.Owner == null).ToList();
            if (owner!= null)
            {
                cards.AddRange(repository.GetAll().Where(c => c.Owner == owner).ToList());
            }
            return DtoUtil.GetCardsTOs(cards);
        }

        public CardDto Get(long id)
        {
            return DtoUtil.GetCardTO(repository.Get(id));
        }

        public void Save(Card card, string user)
        {
            repository.Create(card);
            UpdateClientCards(user).Wait();
        }

        public void Delete(long id, string whoWantDelete)
        {
            Card card = repository.Get(id);
            if (card.Owner == whoWantDelete)
            {
                repository.Delete(id);
            } else
            {
                throw new AccessViolationException("Вы не можете удалять чужие карты!");
            }
            UpdateClientCards(whoWantDelete).Wait();
        }

        private async Task UpdateClientCards(string name)
        {
            await context.Clients
                .Client(service.GetConnectionIdNyName(name))
                .SendAsync("UpdateCards");
        }
    }
}
