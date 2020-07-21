using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ScrumPokerWeb.Services;

namespace ScrumPokerWeb.SignalR
{
  /// <summary>
  /// Сигнал Р хаб комнат.
  /// </summary>
  [Authorize]
  public class RoomsHub : Hub
  {
    private readonly UserService service;

    /// <summary>
    /// Конструктор хаба.
    /// </summary>
    /// <param name="service">Сервис пользователей.</param>
    public RoomsHub(UserService service)
    {
      this.service = service;
    }

    /// <summary>
    /// Переопределение события присоединения нового пользователя.
    /// Имя пользователя с его соединенем заносится в сервис.
    /// </summary>
    /// <returns>Асинхронный результат работы базовой реализации.</returns>
    public override Task OnConnectedAsync()
    {
      service.AddUserToConnectionMap(Context.User.Identity.Name, Context.ConnectionId);
      return base.OnConnectedAsync();
    }

    /// <summary>
    /// Переопределение события отсоединения пользователя.
    /// Удаляет соответствие залогиненного пользователя с соединением.
    /// </summary>
    /// <param name="exception">Возникшее исключение.</param>
    /// <returns>Асинхронный результат работы базовой реализации.</returns>
    public override Task OnDisconnectedAsync(Exception exception)
    {
      service.DeleteUserFromConnectionMap(Context.User.Identity.Name);
      return base.OnDisconnectedAsync(exception);
    }
  }
}