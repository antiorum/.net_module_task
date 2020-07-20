using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DataService;
using DataService.Models;
using Microsoft.AspNetCore.SignalR;
using ScrumPokerWeb.DTO;
using ScrumPokerWeb.SignalR;

namespace ScrumPokerWeb.Services
{
  public class DeckService
  {
    IRepository<Deck> deckRepo;
    IRepository<Card> cardRepo;
    private IHubContext<RoomsHub> context;
    private UserService service;

    public DeckService(IRepository<Deck> deckRepo, IRepository<Card> cardRepo, IHubContext<RoomsHub> context, UserService service)
    {
      this.deckRepo = deckRepo;
      this.cardRepo = cardRepo;
      this.context = context;
      this.service = service;
    }

    public void Delete(long id, string whoWantToDelete)
    {
      Deck deck = deckRepo.Get(id);
      if (whoWantToDelete == deck.Owner)
      {
        deckRepo.Delete(id);
      }
      else
      {
        throw new AccessViolationException("Вы не можете удалять чужие колоды!");
      }
      UpdateClientDecks(whoWantToDelete).Wait();
    }

    public void Update(long id, string whoWantToUpdate, string name, string cardsIds)
    {
      Deck oldDeck = deckRepo.Get(id);
      if (whoWantToUpdate == oldDeck.Owner)
      {
        oldDeck.Name = name ?? oldDeck.Name;
        
        foreach (Card c in oldDeck.Cards)
        {
          c.Decks.Remove(oldDeck);
          cardRepo.Update(c);
        }

        ISet<Card> newCards = new HashSet<Card>();
        foreach (long cardId in ServiceUtils.ParseIds(cardsIds))
        {
          Card card = cardRepo.Get(cardId);
          newCards.Add(card);
          card.Decks.Add(oldDeck);
          cardRepo.Update(card);
        }

        oldDeck.Cards = newCards;
        deckRepo.Update(oldDeck);
      }
      else
      {
        throw new AccessViolationException("Вы не можете изменять чужие колоды!");
      }
      UpdateClientDecks(whoWantToUpdate).Wait();
    }

    public void Create(string owner, string name, string cardsIds)
    {
      Deck newDeck = new Deck();
      newDeck.Name = name;
      newDeck.Owner = owner;
      deckRepo.Create(newDeck);


      foreach (long id in ServiceUtils.ParseIds(cardsIds))
      {
        Card card = cardRepo.Get(id);
        card.Decks.Add(newDeck);
        cardRepo.Update(card);
        newDeck.Cards.Add(card);
      }
      deckRepo.Update(newDeck);

      UpdateClientDecks(owner).Wait();
    }

    public DeckDto Get(long id)
    {
      return DtoUtil.GetDeckTO(deckRepo.Get(id));
    }

    public IEnumerable<DeckDto> GetAvailable(string owner)
    {
      List<Deck> decks = deckRepo.GetAll().Where(d => d.Owner == null).ToList();
      if (owner != null)
      {
        decks.AddRange(deckRepo.GetAll().Where(d => d.Owner == owner));
      }
      return DtoUtil.GetDecksTOs(decks);
    }

    private async Task UpdateClientDecks(string name)
    {
      await context.Clients
          .Client(service.GetConnectionIdNyName(name))
          .SendAsync("UpdateDecks");
    }
  }
}
