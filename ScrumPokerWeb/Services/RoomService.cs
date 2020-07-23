using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using DataService;
using DataService.Models;
using Microsoft.AspNetCore.SignalR;
using ScrumPokerWeb.DTO;
using ScrumPokerWeb.SignalR;

namespace ScrumPokerWeb.Services
{
  /// <summary>
  /// Сервис комнат.
  /// </summary>
  public class RoomService
  {
    private readonly IRepository<Deck> deckRepository;
    private readonly DiscussionResultService discussionResultService;
    private readonly IHubContext<RoomsHub> hubContext;
    private readonly IRepository<Room> roomRepository;
    private readonly IRepository<User> userRepository;
    private readonly UserService userService;
    private readonly ConcurrentDictionary<long, Timer> discussionTimers;

    /// <summary>
    /// Конструктор сервиса.
    /// </summary>
    /// <param name="roomRepository">Репозиторий комнат.</param>
    /// <param name="userRepository">Репозиторий пользователей.</param>
    /// <param name="deckRepository">Репозиторий колод.</param>
    /// <param name="context">Контекcт сигнал р.</param>
    /// <param name="userService">Сервис пользователей.</param>
    /// <param name="discussionResultService">Сервис результатов.</param>
    public RoomService(IRepository<Room> roomRepository, IRepository<User> userRepository,
      IRepository<Deck> deckRepository, IHubContext<RoomsHub> context,
      UserService userService, DiscussionResultService discussionResultService)
    {
      this.roomRepository = roomRepository;
      this.deckRepository = deckRepository;
      this.userRepository = userRepository;
      this.hubContext = context;
      this.userService = userService;
      this.discussionResultService = discussionResultService;
      this.discussionTimers = new ConcurrentDictionary<long, Timer>();
    }

    /// <summary>
    /// Показывает все комнаты.
    /// </summary>
    /// <returns>Коллекцию ДТО комнат.</returns>
    public IEnumerable<RoomDto> GetAll()
    {
      return DtoUtil.GetRoomsDtos(this.roomRepository.GetAll());
    }

    /// <summary>
    /// Показывает комнату, если пользователь вошел в нее.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    /// <param name="loggedUser">Залогиненный пользователь.</param>
    /// <returns>ДТО комнаты.</returns>
    public RoomDto Get(long id, string loggedUser)
    {
      var room = this.roomRepository.Get(id);
      if (room.Users.Select(u => u.Name).Contains(loggedUser))
      {
        this.AddConnectionIdToGroup(loggedUser, room.Id).Wait();
        this.SendUpdateRoomToClients(room.Id).Wait();
        return DtoUtil.GetRoomDto(room);
      }

      throw new AccessViolationException("Сначала нужно войти в комнату");
    }

    /// <summary>
    /// Создает новую комнату.
    /// </summary>
    /// <param name="loggedUser">Владелец комнаты.</param>
    /// <param name="password">Пароль.</param>
    /// <param name="timerMinutes">На сколько минут будет установлен таймер.</param>
    /// <param name="deckId">ИД используемой колоды.</param>
    public void Create(string loggedUser, string password, string timerMinutes, string deckId)
    {
      var minutes = int.Parse(timerMinutes);
      var deck = this.deckRepository.Get(long.Parse(deckId));
      var room = new Room { Password = password, TimerMinutes = minutes, Deck = deck };
      var owner = this.GetUserByName(loggedUser);
      room.Owner = owner;
      this.roomRepository.Save(room);
      this.AddUserToRoom(owner, room);
      this.AddConnectionIdToGroup(loggedUser, room.Id).Wait();
      this.hubContext.Clients.All.SendAsync("UpdateRooms").Wait();
    }

    /// <summary>
    /// Добавляет пользователя в комнату и показывает комнату.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    /// <param name="loggedUser">Имя пользователя.</param>
    /// <param name="password">Пароль к комнате.</param>
    /// <returns>ДТО комнаты.</returns>
    public RoomDto AddUserToRoomAndGet(long id, string loggedUser, string password)
    {
      var room = this.roomRepository.Get(id);
      var user = this.GetUserByName(loggedUser);
      if (room.Password == password)
      {
        if (!room.Users.Contains(user)) this.AddUserToRoom(user, room);
        this.AddConnectionIdToGroup(loggedUser, room.Id).Wait();
        this.SendUpdateRoomToClients(room.Id).Wait();
        return DtoUtil.GetRoomDto(this.roomRepository.Get(room.Id));
      }

      throw new AccessViolationException("Неверный пароль к комнате");
    }

    /// <summary>
    /// Изменяет пароль комнаты.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    /// <param name="loggedUser">Залогиненный пользователь.</param>
    /// <param name="newPassword">Новый пароль.</param>
    public void ChangeRoomPassword(long id, string loggedUser, string newPassword)
    {
      var room = this.roomRepository.Get(id);
      if (room.Owner.Name != loggedUser)
      {
        throw new AccessViolationException("Нельзя менять пароль чужой комнаты!");
      }
      room.Password = newPassword;
      this.roomRepository.Update(room);

      this.hubContext.Clients.All.SendAsync("UpdateRooms").Wait();
    }

    /// <summary>
    /// Выход из комнаты.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    /// <param name="loggedUser">Имя выходящего пользователя.</param>
    public void ExitFromRoom(long id, string loggedUser)
    {
      var room = this.roomRepository.Get(id);
      var user = this.GetUserByName(loggedUser);
      if (room.Users.Contains(user)) this.RemoveUserFromRoom(room, user);
      this.RemoveConnectionFromGroup(user.Name, room.Id).Wait();
      this.SendUpdateRoomToClients(room.Id).Wait();
    }

    /// <summary>
    /// Удалить пользователя из комнаты.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    /// <param name="userId">ИД удаляемого юзера.</param>
    /// <param name="loggedUser">Юзер, пославший запрос на удаление.</param>
    public void DeleteUserFromRoom(long id, long userId, string loggedUser)
    {
      var room = this.roomRepository.Get(id);
      if (room.Owner.Name != loggedUser)
      {
        throw new AccessViolationException("У вас нет прав на удаление пользователя из комнаты.");
      }
      var user = this.userRepository.Get(userId);
      if (room.Users.Contains(user))
      {
        this.RemoveUserFromRoom(room, user);
        this.RemoveConnectionFromGroup(user.Name, room.Id).Wait();
      }

      this.SendUpdateRoomToClients(room.Id).Wait();
    }

    /// <summary>
    /// Удаление комнаты.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    /// <param name="whoWantDelete">Юзер, пославший запрос на удаление.</param>
    public void Delete(long id, string whoWantDelete)
    {
      var room = this.roomRepository.Get(id);
      if (room.Owner.Name != whoWantDelete)
        throw new AccessViolationException("Нельзя удалять чужие комнаты!");
      this.roomRepository.Delete(id);
      this.hubContext.Clients.All.SendAsync("UpdateRooms").Wait();
    }

    /// <summary>
    /// Начать новое обсуждение в комнате.
    /// </summary>
    /// <param name="roomId">ИД комнаты.</param>
    /// <param name="loggedUser">Юзер, пославший запрос.</param>
    /// <param name="theme">Тема обсуждения.</param>
    public void StartNewDiscussion(long roomId, string loggedUser, string theme)
    {
      var room = this.roomRepository.Get(roomId);
      if (room.Owner.Name != loggedUser)
      {
        throw new AccessViolationException("Только владелец комнаты может начать обсуждение.");
      }
      var discussionId = this.discussionResultService.Create(theme);
      room.CurrentDiscussionResultId = discussionId;
      this.roomRepository.Update(room);
      if (room.TimerMinutes != 0)
      {
        var timer = this.CreateTimer(room.TimerMinutes, discussionId, loggedUser);
        this.discussionTimers.TryAdd(discussionId, timer);
      }
      this.hubContext.Clients.Group($"room{room.Id}").SendAsync("StartDiscussion").Wait();
    }

    /// <summary>
    /// Добавить или изменить оценку в текущем обсуждении.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    /// <param name="loggedUser">Пользователь, отправивший запрос.</param>
    /// <param name="cardId">ИД карты.</param>
    public void AddOrChangeMarkInCurrentDiscussion(long id, string loggedUser, string cardId)
    {
      var room = this.roomRepository.Get(id);
      if (!room.Users.Select(u => u.Name).Contains(loggedUser))
        throw new AccessViolationException("Только участники комнаты могут добавлять оценки.");
      this.discussionResultService.AddOrChangeMark(room.CurrentDiscussionResultId, loggedUser, cardId);
      this.hubContext.Clients.Group($"room{room.Id}").SendAsync("UserVoted", loggedUser).Wait();
    }

    /// <summary>
    /// Завершает текущую дискуссию.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    /// <param name="loggedUser">Пользователь, пославший запрос.</param>
    /// <param name="resume">Итог обсуждения или комментарий.</param>
    public void EndCurrentDiscussion(long id, string loggedUser, string resume)
    {
      var room = this.roomRepository.Get(id);
      if (room.Owner.Name != loggedUser)
        throw new AccessViolationException("Только владелец комнаты может закончить обсуждение.");

      this.discussionResultService.EndDiscussion(room.CurrentDiscussionResultId, resume);
      this.discussionTimers.GetValueOrDefault(room.CurrentDiscussionResultId)?.Stop();
      this.hubContext.Clients.Group($"room{room.Id}").SendAsync("EndDiscussion").Wait();
    }

    /// <summary>
    /// Сбрасывает результаты текущего обсуждения и начинает заново.
    /// </summary>
    /// <param name="id">ИД комнаты.</param>
    /// <param name="loggedUser">Пользователь, пославший запрос.</param>
    public void RestartDiscussion(long id, string loggedUser)
    {
      var room = this.roomRepository.Get(id);
      if (room.Owner.Name != loggedUser)
        throw new AccessViolationException("Только владелец комнаты может заново начать обсуждение.");
      this.discussionResultService.ResetDiscussionResult(room.CurrentDiscussionResultId);
      this.discussionTimers.GetValueOrDefault(room.CurrentDiscussionResultId)?.Start();
      this.hubContext.Clients.Group($"room{room.Id}").SendAsync("StartDiscussion").Wait();
    }

    private void RemoveUserFromRoom(Room room, User user)
    {
      room.Users.Remove(user);
      this.roomRepository.Update(room);
      user.Rooms.Remove(room);
      this.userRepository.Update(user);
    }

    private void AddUserToRoom(User user, Room room)
    {
      room.Users.Add(user);
      this.roomRepository.Update(room);
      user.Rooms.Add(room);
      this.userRepository.Update(user);
    }

    private User GetUserByName(string name)
    {
      return this.userRepository.GetAll().FirstOrDefault(u => u.Name == name);
    }

    private async Task AddConnectionIdToGroup(string username, long roomId)
    {
      var connectionId = this.userService.GetConnectionIdByName(username);
      await this.hubContext.Groups.RemoveFromGroupAsync(connectionId, $"room{roomId}");
      await this.hubContext.Groups.AddToGroupAsync(connectionId, $"room{roomId}");
    }

    private async Task RemoveConnectionFromGroup(string username, long roomId)
    {
      var connectionId = this.userService.GetConnectionIdByName(username);
      await this.hubContext.Groups.RemoveFromGroupAsync(connectionId, $"room{roomId}");
    }

    private async Task SendUpdateRoomToClients(long roomId)
    {
      await this.hubContext.Clients.Group($"room{roomId}").SendAsync("UpdateUsersInRoom");
    }

    private Timer CreateTimer(int minutes, long discussionResultId, string loggedUser)
    {
      var timer = new Timer(1000 * 60 * minutes);
      timer.AutoReset = false;
      timer.Elapsed += (e, a) =>
        this.EndCurrentDiscussion(discussionResultId, loggedUser, string.Empty);
      timer.Enabled = true;
      timer.Start();
      return timer;
    }
  }
}