using System.Collections.Generic;
using DataService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumPokerWeb.DTO;
using ScrumPokerWeb.Services;

namespace ScrumPokerWeb.Controllers
{
  /// <summary>
  ///   Контроллер карт.
  /// </summary>
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class CardsController : ControllerBase
  {
    /// <summary>
    ///   Сервис карт.
    /// </summary>
    private readonly CardService service;

    /// <summary>
    ///   Инстанцирует контроллер.
    /// </summary>
    /// <param name="service">Сервис-сигнлтон.</param>
    public CardsController(CardService service)
    {
      this.service = service;
    }

    /// <summary>
    ///   Имя залогиненного пользователя.
    /// </summary>
    private string LoggedUser => User.Identity.Name;

    /// <summary>
    ///   Доступные карты пользователя.
    /// </summary>
    /// <returns>Коллекцию объектов-отображений карт.</returns>
    [HttpGet]
    public IEnumerable<CardDto> GetAvailable()
    {
      return service.GetAvailable(LoggedUser);
    }

    /// <summary>
    ///   Находит карту по ИД.
    /// </summary>
    /// <param name="id">ИД карты.</param>
    /// <returns>Объект-представление карты.</returns>
    [HttpGet("byId/{id}")]
    public CardDto Get(long id)
    {
      return service.Get(id);
    }

    /// <summary>
    /// Создаёт новую карту из запроса.
    /// </summary>
    /// <param name="card">Параметры карты.</param>
    [HttpPost]
    public void Post([FromForm] Card card)
    {
      card.Owner = LoggedUser;
      service.Save(card, LoggedUser);
    }

    /// <summary>
    /// Удаляет карту по идентификатору.
    /// </summary>
    /// <param name="id">ИД карты.</param>
    [HttpDelete("{id}")]
    public void Delete(long id)
    {
      service.Delete(id, LoggedUser);
    }
  }
}