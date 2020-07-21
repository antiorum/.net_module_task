using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumPokerWeb.DTO;
using ScrumPokerWeb.Services;

namespace ScrumPokerWeb.Controllers
{
  /// <summary>
  /// Контроллер результатов обсуждений.
  /// </summary>
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class DiscussionResultsController : ControllerBase
  {
    /// <summary>
    /// Сервис результатов обсуждений.
    /// </summary>
    private readonly DiscussionResultService service;

    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="service">Сервис-синглтон.</param>
    public DiscussionResultsController(DiscussionResultService service)
    {
      this.service = service;
    }

    /// <summary>
    /// Имя залогиненного пользователя.
    /// </summary>
    private string LoggedUser => User.Identity.Name;

    /// <summary>
    /// Показать все результаты обсуждений.
    /// </summary>
    /// <returns>Коллекцию результатов обсуждений.</returns>
    [HttpGet]
    public IEnumerable<DiscussionResultDto> Get()
    {
      return service.GetAll();
    }

    /// <summary>
    /// Показать результаты обсуждений текущего пользователя.
    /// </summary>
    /// <returns>Коллекцию результатов обсуждений.</returns>
    [HttpGet("own")]
    public IEnumerable<DiscussionResultDto> GetOwnResults()
    {
      return service.GetByName(LoggedUser);
    }

    /// <summary>
    /// Выдаёт результат обсуждения по его ИД.
    /// </summary>
    /// <param name="id">ИД результата обсуждения.</param>
    /// <returns>Объект-отображения результата.</returns>
    [HttpGet("{id}")]
    public DiscussionResultDto Get(long id)
    {
      return service.Get(id);
    }
  }
}