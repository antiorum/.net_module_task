using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumPokerWeb.DTO;
using ScrumPokerWeb.Services;

namespace ScrumPokerWeb.Controllers
{
  /// <summary>
  /// Контроллер колод.
  /// </summary>
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class DecksController : ControllerBase
  {
    private readonly DeckService service;

    /// <summary>
    /// Создаёт контроллер.
    /// </summary>
    /// <param name="service">Сервис карт.</param>
    public DecksController(DeckService service)
    {
      this.service = service;
    }

    private string LoggedUser => User.Identity.Name;


    /// <summary>
    /// Доступные пользователю колоды.
    /// </summary>
    /// <returns>Коллекция объектов-отображений колод.</returns>
    [AllowAnonymous]
    [HttpGet]
    public IEnumerable<DeckDto> GetAvailable()
    {
      return service.GetAvailable(LoggedUser);
    }

    /// <summary>
    /// Поиск колоды оп ИД.
    /// </summary>
    /// <param name="id">ИД колоды.</param>
    /// <returns>Отображение колоды.</returns>
    [HttpGet("{id}")]
    public DeckDto Get(long id)
    {
      return service.Get(id);
    }

    /// <summary>
    /// Создание новой колоды из запроса.
    /// </summary>
    [HttpPost]
    public void Post()
    {
      string name = Request.Form["name"];
      string cards = Request.Form["cards"];
      service.Create(LoggedUser, name, cards);
    }

    /// <summary>
    /// Изменение колоды из запроса.
    /// </summary>
    /// <param name="id">ИД колоды.</param>
    [HttpPut("{id}")]
    public void Put(long id)
    {
      string name = Request.Form["name"];
      string cards = Request.Form["cards"];
      service.Update(id, LoggedUser, name, cards);
    }

    /// <summary>
    /// Удаление колоды запросом.
    /// </summary>
    /// <param name="id">ИД колоды.</param>
    [HttpDelete("{id}")]
    public void Delete(long id)
    {
      service.Delete(id, LoggedUser);
    }
  }
}