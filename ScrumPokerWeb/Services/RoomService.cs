using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using DataService;
using DataService.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.SignalR;
using ScrumPokerWeb.DTO;
using ScrumPokerWeb.SignalR;

namespace ScrumPokerWeb.Services
{
  public class RoomService
  {
    private readonly IRepository<Room> roomRepo;
    private readonly IRepository<Deck> deckRepo;
    private readonly IRepository<User> userRepo;
    private readonly IHubContext<RoomsHub> hubContext;
    private readonly UserService userService;
    private readonly DiscussionResultService discussionResultService;
    private ConcurrentDictionary<long, Timer> discussionTimers;

    public RoomService(IRepository<Room> roomRepo, IRepository<User> userRepo,
        IRepository<Deck> deckRepo, IHubContext<RoomsHub> context,
        UserService service, DiscussionResultService discussionResultService)
    {
      this.roomRepo = roomRepo;
      this.deckRepo = deckRepo;
      this.userRepo = userRepo;
      this.hubContext = context;
      this.userService = service;
      this.discussionResultService = discussionResultService;
      discussionTimers = new ConcurrentDictionary<long, Timer>();
    }

    public IEnumerable<RoomDto> GetAll()
    {
      return DtoUtil.GetRoomsTOs(roomRepo.GetAll());
    }

    public RoomDto Get(long id, string loggedUser)
    {
      Room room = roomRepo.Get(id);
      if (room.Users.Select(u => u.Name).Contains(loggedUser))
      {
        return DtoUtil.GetRoomTO(room);
      }
      else
      {
        throw new AccessViolationException("Сначала нужно войти в комнату");
      }
    }

    public void Create(string loggedUser, string password, string timerMinutes, string deckId)
    {
      int minutes = int.Parse(timerMinutes);
      Deck deck = deckRepo.Get(long.Parse(deckId));
      Room room = new Room { Password = password, TimerMinutes = minutes, Deck = deck };
      User owner = GetUserByName(loggedUser);
      room.Owner = owner;
      roomRepo.Create(room);
      AddUserToRoom(owner, room);
      AddConnectionIdToGroup(loggedUser, room.Id).Wait();
      hubContext.Clients.All.SendAsync("UpdateRooms").Wait();
    }

    public RoomDto AddUserToRoomAndGet(long id, string loggedUser, string password)
    {
      Room room = roomRepo.Get(id);
      User user = GetUserByName(loggedUser);
      if (room.Password == password)
      {
        if (!room.Users.Contains(user))
        {
          AddUserToRoom(user, room);
        }
        AddConnectionIdToGroup(loggedUser, room.Id).Wait();
        SendUpdateRoomToClients(room.Id).Wait();
        return DtoUtil.GetRoomTO(roomRepo.Get(room.Id));
      }
      else
      {
        throw new AccessViolationException("Неверный пароль к комнате");
      }
    }

    public void ChangeRoomPassword(long id, string loggedUser, string newPassword)
    {
      Room room = roomRepo.Get(id);
      if (room.Owner.Name == loggedUser)
      {
        room.Password = newPassword;
        roomRepo.Update(room);
      }
      else
      {
        throw new AccessViolationException("Нельзя менять пароль чужой комнаты!");
      }
      hubContext.Clients.All.SendAsync("UpdateRooms").Wait();
    }

    public void ExitFromRoom(long id, string loggedUser)
    {
      Room room = roomRepo.Get(id);
      User user = GetUserByName(loggedUser);
      if (room.Users.Contains(user))
      {
        RemoveUserFromRoom(room, user);
      }
      RemoveConnectionFromGroup(user.Name, room.Id).Wait();
      SendUpdateRoomToClients(room.Id).Wait();
    }

    public void DeleteUserFromRoom(long id, long userId, string loggedUser)
    {
      Room room = roomRepo.Get(id);
      if (room.Owner.Name == loggedUser)
      {
        User user = userRepo.Get(userId);
        if (room.Users.Contains(user))
        {
          RemoveUserFromRoom(room, user);
          RemoveConnectionFromGroup(user.Name, room.Id).Wait();
        }
      }
      else
      {
        throw new AccessViolationException("У вас нет прав на удаление пользователя из комнаты.");
      }
      SendUpdateRoomToClients(room.Id).Wait();
    }

    public void Delete(long id, string whoWantDelete)
    {
      var room = roomRepo.Get(id);
      if (room.Owner.Name == whoWantDelete)
      {
        roomRepo.Delete(id);
      }
      else
      {
        throw new AccessViolationException("Нельзя удалять чужие комнаты!");
      }
      hubContext.Clients.All.SendAsync("UpdateRooms").Wait();
    }

    public void StartNewDiscussion(long roomId, string loggedUser, string theme)
    {
      Room room = roomRepo.Get(roomId);
      if (room.Owner.Name == loggedUser)
      {
        long discussionId = discussionResultService.Create(theme);
        room.CurrentDiscussionResultId = discussionId;
        roomRepo.Update(room);
        if (room.TimerMinutes != 0)
        {
          Timer timer = CreateTimer(room.TimerMinutes, discussionId, loggedUser);
          discussionTimers.TryAdd(discussionId, timer);
        }
      }
      else
      {
        throw new AccessViolationException("Только владелец комнаты может начать обсуждение.");
      }

      hubContext.Clients.Group($"room{room.Id}").SendAsync("StartDiscussion").Wait();
    }

    public void AddOrChangeMarkInCurrentDiscussion(long id, string loggedUser, string cardId)
    {
      Room room = roomRepo.Get(id);
      if (room.Users.Select(u => u.Name).Contains(loggedUser))
      {
        discussionResultService.AddOrChangeMark(room.CurrentDiscussionResultId, loggedUser, cardId);
      }
      else
      {
        throw new AccessViolationException("Только участники комнаты могут добавлять оценки.");
      }

      hubContext.Clients.Group($"room{room.Id}").SendAsync("UserVoted", loggedUser).Wait();
    }


    public void EndCurrentDiscussion(long id, string loggedUser, string resume)
    {
      Room room = roomRepo.Get(id);
      if (room.Owner.Name == loggedUser)
      {
        discussionResultService.EndDiscussion(room.CurrentDiscussionResultId, resume);
      }
      else
      {
        throw new AccessViolationException("Только владелец комнаты может закончить обсуждение.");
      }

      discussionTimers.GetValueOrDefault(room.CurrentDiscussionResultId).Stop();
      hubContext.Clients.Group($"room{room.Id}").SendAsync("EndDiscussion").Wait();
    }

    public void RestartDiscussion(long id, string loggedUser)
    {
      Room room = roomRepo.Get(id);
      if (room.Owner.Name == loggedUser)
      {
        discussionResultService.ResetDiscussionResult(room.CurrentDiscussionResultId);
      }
      else
      {
        throw new AccessViolationException("Только владелец комнаты может заново начать обсуждение.");
      }
      discussionTimers.GetValueOrDefault(room.CurrentDiscussionResultId).Start();
      hubContext.Clients.Group($"room{room.Id}").SendAsync("StartDiscussion").Wait();
    }

    private void RemoveUserFromRoom(Room room, User user)
    {
      room.Users.Remove(user);
      roomRepo.Update(room);
      user.Rooms.Remove(room);
      userRepo.Update(user);
    }

    private void AddUserToRoom(User user, Room room)
    {
      room.Users.Add(user);
      roomRepo.Update(room);
      user.Rooms.Add(room);
      userRepo.Update(user);
    }

    private User GetUserByName(string name)
    {
      return userRepo.GetAll().FirstOrDefault(u => u.Name == name);
    }

    private async Task AddConnectionIdToGroup(string username, long roomId)
    {
      string connectionId = userService.GetConnectionIdNyName(username);
      await hubContext.Groups.AddToGroupAsync(connectionId, $"room{roomId}");
    }

    private async Task RemoveConnectionFromGroup(string username, long roomId)
    {
      string connectionId = userService.GetConnectionIdNyName(username);
      await hubContext.Groups.RemoveFromGroupAsync(connectionId, $"room{roomId}");
    }

    private async Task SendUpdateRoomToClients(long roomId)
    {
      await hubContext.Clients.Group($"room{roomId}").SendAsync("UpdateUsersInRoom");
    }

    private Timer CreateTimer(int minutes, long discussionResultId, string loggedUser)
    {
      Timer timer = new Timer(1000 * 60 * minutes);
      timer.AutoReset = false;
      timer.Elapsed += (e, a) => EndCurrentDiscussion(discussionResultId, loggedUser, string.Empty);
      timer.Enabled = true;
      timer.Start();
      return timer;
    }
  }
}
