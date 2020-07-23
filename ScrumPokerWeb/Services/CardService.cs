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
  /// Сервис карт.
  /// </summary>
  public class CardService
  {
    private IRepository<Card> repository;
    private IHubContext<RoomsHub> context;
    private UserService service;

    /// <summary>
    /// Конструктор сервиса.
    /// </summary>
    /// <param name="repository">Репозиторий карт.</param>
    /// <param name="context">Контекст сигнал р.</param>
    /// <param name="service">Сервис пользователей.</param>
    public CardService(IRepository<Card> repository, IHubContext<RoomsHub> context, UserService service)
    {
      this.repository = repository;
      this.context = context;
      this.service = service;
    }

    /// <summary>
    /// Доступные карты.
    /// </summary>
    /// <param name="owner">Имя владельца.</param>
    /// <returns>Коллекцию ДТО карт.</returns>
    public IEnumerable<CardDto> GetAvailable(string owner)
    {
      List<Card> cards = this.repository.GetAll().Where(c => c.Owner == null).ToList();
      if (owner != null)
      {
        cards.AddRange(this.repository.GetAll().Where(c => c.Owner == owner).ToList());
      }
      return DtoConverters.GetCardsDtos(cards);
    }

    /// <summary>
    /// Возвращает карту по ИД.
    /// </summary>
    /// <param name="id">ИД карты.</param>
    /// <returns>ДТО карты.</returns>
    public CardDto Get(long id)
    {
      return DtoConverters.GetCardDto(this.repository.Get(id));
    }

    /// <summary>
    /// Сохраняет в репозиторий новую карту. Оповещает клиента о изменении списка карт.
    /// </summary>
    /// <param name="card">Сущность карты.</param>
    /// <param name="user">Имя пользователя.</param>
    public void Save(Card card, string user)
    {
      this.repository.Save(card);
      this.UpdateClientCards(user).Wait();
    }

    /// <summary>
    /// Удаляет карту, если пользователь имеет на это право. Оповещает клиента о изменении списка карт.
    /// </summary>
    /// <param name="id">ИД карты.</param>
    /// <param name="whoWantDelete">Имя пользователя, удаляющего карту.</param>
    public void Delete(long id, string whoWantDelete)
    {
      Card card = this.repository.Get(id);
      if (card.Owner != whoWantDelete)
      {
        throw new AccessViolationException("Вы не можете удалять чужие карты!");
        
      }
      this.repository.Delete(id);
      this.UpdateClientCards(whoWantDelete).Wait();
    }

    private async Task UpdateClientCards(string name)
    {
      await this.context.Clients
          .Client(this.service.GetConnectionIdByName(name))
          .SendAsync("UpdateCards");
    }
  }
}
