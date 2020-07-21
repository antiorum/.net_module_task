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
  /// <summary>
  /// Сервис колод.
  /// </summary>
  public class DeckService
  {
    private readonly IRepository<Card> cardRepo;
    private readonly IHubContext<RoomsHub> context;
    private readonly IRepository<Deck> deckRepo;
    private readonly UserService service;

    /// <summary>
    /// Конструктор сервиса.
    /// </summary>
    /// <param name="deckRepo">Репозиторий колод.</param>
    /// <param name="cardRepo">Репозиторий карт.</param>
    /// <param name="context">Контекст сигнал р.</param>
    /// <param name="service">Сервия пользователей.</param>
    public DeckService(IRepository<Deck> deckRepo, IRepository<Card> cardRepo, IHubContext<RoomsHub> context,
      UserService service)
    {
      this.deckRepo = deckRepo;
      this.cardRepo = cardRepo;
      this.context = context;
      this.service = service;
    }

    /// <summary>
    /// Удалить колоду.
    /// </summary>
    /// <param name="id">ИД колоды.</param>
    /// <param name="whoWantToDelete">Пользователь, удаляющий колоду.</param>
    public void Delete(long id, string whoWantToDelete)
    {
      var deck = this.deckRepo.Get(id);
      if (whoWantToDelete == deck.Owner)
        this.deckRepo.Delete(id);
      else
        throw new AccessViolationException("Вы не можете удалять чужие колоды!");
      this.UpdateClientDecks(whoWantToDelete).Wait();
    }

    /// <summary>
    /// Изменить колоду.
    /// </summary>
    /// <param name="id">ИД колоды.</param>
    /// <param name="whoWantToUpdate">Пользователь, изменяющий колоду.</param>
    /// <param name="name">Новое название.</param>
    /// <param name="cardsIds">Новый список карт.</param>
    public void Update(long id, string whoWantToUpdate, string name, string cardsIds)
    {
      var oldDeck = this.deckRepo.Get(id);
      if (whoWantToUpdate == oldDeck.Owner)
      {
        oldDeck.Name = name ?? oldDeck.Name;

        foreach (var c in oldDeck.Cards)
        {
          c.Decks.Remove(oldDeck);
          this.cardRepo.Update(c);
        }

        ISet<Card> newCards = new HashSet<Card>();
        foreach (var cardId in ServiceUtils.ParseIds(cardsIds))
        {
          var card = this.cardRepo.Get(cardId);
          newCards.Add(card);
          card.Decks.Add(oldDeck);
          this.cardRepo.Update(card);
        }

        oldDeck.Cards = newCards;
        this.deckRepo.Update(oldDeck);
      }
      else
      {
        throw new AccessViolationException("Вы не можете изменять чужие колоды!");
      }

      this.UpdateClientDecks(whoWantToUpdate).Wait();
    }

    /// <summary>
    /// Создает новую колоду.
    /// </summary>
    /// <param name="owner">Владелец колоды.</param>
    /// <param name="name">Название колоды.</param>
    /// <param name="cardsIds">Список карт.</param>
    public void Create(string owner, string name, string cardsIds)
    {
      var newDeck = new Deck();
      newDeck.Name = name;
      newDeck.Owner = owner;
      this.deckRepo.Create(newDeck);

      foreach (var id in ServiceUtils.ParseIds(cardsIds))
      {
        var card = this.cardRepo.Get(id);
        card.Decks.Add(newDeck);
        this.cardRepo.Update(card);
        newDeck.Cards.Add(card);
      }

      this.deckRepo.Update(newDeck);

      this.UpdateClientDecks(owner).Wait();
    }

    /// <summary>
    /// Находит колоду по ИД.
    /// </summary>
    /// <param name="id">ИД колоды.</param>
    /// <returns>ДТО колоды.</returns>
    public DeckDto Get(long id)
    {
      return DtoUtil.GetDeckDto(this.deckRepo.Get(id));
    }

    /// <summary>
    /// Находит доступные колоды.
    /// </summary>
    /// <param name="owner">Владелец колод.</param>
    /// <returns>Коллекцию ДТО колод.</returns>
    public IEnumerable<DeckDto> GetAvailable(string owner)
    {
      var decks = this.deckRepo.GetAll().Where(d => d.Owner == null).ToList();
      if (owner != null) decks.AddRange(this.deckRepo.GetAll().Where(d => d.Owner == owner));
      return DtoUtil.GetDecksDtos(decks);
    }

    private async Task UpdateClientDecks(string name)
    {
      await this.context.Clients
        .Client(this.service.GetConnectionIdByName(name))
        .SendAsync("UpdateDecks");
    }
  }
}