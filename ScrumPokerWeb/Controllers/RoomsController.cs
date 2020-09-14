using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumPokerWeb.DTO;
using ScrumPokerWeb.Services;

namespace ScrumPokerWeb.Controllers
{
  /// <summary>
  /// Контроллер комнат.
  /// </summary>
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class RoomsController : ControllerBase
  {
    /// <summary>
    /// Сервис-синглтон.
    /// </summary>
    private readonly RoomService service;

    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="service">Сервис комнат.</param>
    public RoomsController(RoomService service)
    {
      this.service = service;
    }

    /// <summary>
    /// Имя залогиненного юзера.
    /// </summary>
    private string LoggedUser => User.Identity.Name;

    /// <summary>
    /// Показать все комнаты.
    /// </summary>
    /// <returns>Коллекция отображений комнат.</returns>
    [HttpGet]
    public IEnumerable<RoomDto> GetAll()
    {
      return service.GetAll();
    }

    /// <summary>
    /// Найти комнату по ИД (пользователь должен предварительно войти в комнату).
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    /// <returns>Отображение комнаты.</returns>
    [HttpGet("{id}")]
    public RoomDto Get(long id)
    {
      return service.Get(id, LoggedUser);
    }

    /// <summary>
    /// Войти в комнату.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    /// <returns>Отображение комнаты.</returns>
    [HttpPost("{id}")]
    public RoomDto LoginRoom(long id)
    {
      string password = Request.Form["password"];
      return service.AddUserToRoomAndGet(id, LoggedUser, password);
    }

    /// <summary>
    /// Создаёт новую комнату.
    /// </summary>
    /// <returns>ИД созданной комнаты.</returns>
    [HttpPost]
    public long Post()
    {
      string password = Request.Form["password"];
      string name = Request.Form["name"];
      string timerMinutes = Request.Form["timer"];
      if (timerMinutes == string.Empty) timerMinutes = "0";
      string deckId = Request.Form["deck"];
      return service.Create(LoggedUser, password, name, timerMinutes, deckId);
    }

    /// <summary>
    /// Удаляет комнату.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    [HttpDelete("{id}")]
    public void Delete(long id)
    {
      service.Delete(id, LoggedUser);
    }

    /// <summary>
    /// Изменить пароль комнаты.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    [HttpPut("{id}/changePassword")]
    public void Put(long id)
    {
      string newPassword = Request.Form["password"];
      service.ChangeRoomPassword(id, LoggedUser, newPassword);
    }

    /// <summary>
    /// Поменять таймер в комнате.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    [HttpPut("{id}/changeTimer")]
    public void ChangeTimer(long id)
    {
      string newTimerDuration = Request.Form["timer"];
      if (newTimerDuration == string.Empty) newTimerDuration = "0";
      service.ChangeRoomTimer(id, LoggedUser, newTimerDuration);
    }

    /// <summary>
    /// Удалить пользователя из комнаты.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    /// <param name="userId">ИД пользователя.</param>
    [HttpPost("{id}/deleteUser/{userId}")]
    public void DeleteUserFromRoom(long id, long userId)
    {
      service.DeleteUserFromRoom(id, userId, LoggedUser);
    }

    /// <summary>
    /// Выйти из комнаты.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    [HttpPost("{id}/exit")]
    public void ExitFromRoom(long id)
    {
      service.ExitFromRoom(id, LoggedUser);
    }

    /// <summary>
    /// Начать дискуссию в комнате. Пользатель должен должен быть владельцем комнаты.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    [HttpPut("{id}/startDiscussion")]
    public void StartDiscussion(long id)
    {
      string theme = Request.Form["theme"];
      service.StartNewDiscussion(id, LoggedUser, theme);
    }

    /// <summary>
    /// Добавление оценки в текущее обсуждение.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    [HttpPut("{id}/addMark")]
    public void AddMarkInDiscussion(long id)
    {
      string cardId = Request.Form["cardId"];
      long discussionResultId = long.Parse(Request.Form["discussionId"]);
      service.AddOrChangeMarkInCurrentDiscussion(id, LoggedUser, cardId, discussionResultId);
    }

    /// <summary>
    /// Закончить дискуссию в комнате. Пользатель должен должен быть владельцем комнаты.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    [HttpPut("{id}/endDiscussion")]
    public void EndDiscussion(long id)
    {
      string resume = Request.Form["resume"];
      service.EndCurrentDiscussion(id, LoggedUser, resume);
    }

    /// <summary>
    /// Начать заново дискуссию в комнате. Пользатель должен должен быть владельцем комнаты.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    [HttpPut("{id}/restartDiscussion")]
    public void RestartDiscussion(long id)
    {
      long discussionResultId = long.Parse(Request.Form["discussionId"]);
      service.RestartDiscussion(id, LoggedUser, discussionResultId);
    }

    /// <summary>
    /// Удалить результат обсуждения.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    /// <param name="discussionId">ИД результата обсуждения.</param>
    [HttpDelete("{id}/deleteDiscussion/{discussionId}")]
    public void DeleteDiscussionResult(long id, long discussionId)
    {
      service.DeleteDiscussionResult(id, LoggedUser, discussionId);
    }

    /// <summary>
    /// Изменить тему результата обсуждения.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    /// <param name="discussionId">ИД результата обсуждения.</param>
    [HttpPut("{id}/renameDiscussion/{discussionId}")]
    public void RenameDiscussionResult(long id, long discussionId)
    {
      string newName = Request.Form["discussionNewName"];
      service.RenameDiscussionResult(id, LoggedUser, discussionId, newName);
    }
  }
}