using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DataService;
using DataService.Models;
using Microsoft.AspNetCore.SignalR;
using ScrumPokerWeb.DTO;
using ScrumPokerWeb.SignalR;

namespace ScrumPokerWeb.Services
{
  /// <summary>
  /// Сервис пользователей.
  /// </summary>
  public class UserService
  {
    private readonly IRepository<User> repository;
    private readonly ConcurrentDictionary<string, string> userWithSignalRIds;

    /// <summary>
    /// Конструктор сервиса.
    /// </summary>
    /// <param name="repository">Репозиторий пользователей.</param>
    /// <param name="context">Контекст сигнал р.</param>
    public UserService(IRepository<User> repository, IHubContext<RoomsHub> context)
    {
      this.repository = repository;
      this.userWithSignalRIds = new ConcurrentDictionary<string, string>();
    }

    /// <summary>
    /// Поиск пользователя по имени.
    /// </summary>
    /// <param name="name">Имя пользователя.</param>
    /// <returns>Сущность - пользователя.</returns>
    public User GetByName(string name)
    {
      return this.repository.GetAll().FirstOrDefault(u => u.Name == name);
    }

    /// <summary>
    /// Создаёт нового пользователя в репозитории.
    /// </summary>
    /// <param name="user">Имя пользователя.</param>
    public void Create(User user)
    {
      this.repository.Save(user);
    }

    /// <summary>
    /// Выдает список всех пользователей.
    /// </summary>
    /// <returns>Коллекцию ДТО пользователей.</returns>
    public IEnumerable<UserDto> GetAll()
    {
      return DtoConverters.GetUsersDtos(this.repository.GetAll());
    }

    /// <summary>
    /// Сохраняет соответсвие пользователя с его соединением сигнал р.
    /// </summary>
    /// <param name="user">Имя пользователя.</param>
    /// <param name="signalRId">Соединение.</param>
    public void AddUserToConnectionMap(string user, string signalRId)
    {
      if (this.userWithSignalRIds.ContainsKey(user))
      {
        string oldConnection;
        this.userWithSignalRIds.TryRemove(user, out oldConnection);
      }

      this.userWithSignalRIds.TryAdd(user, signalRId);
    }

    /// <summary>
    /// Удаляет соответсвие пользователя с его соединением.
    /// </summary>
    /// <param name="identityName">Имя пользователя.</param>
    public void DeleteUserFromConnectionMap(string identityName)
    {
      string oldConnection;
      this.userWithSignalRIds.TryRemove(identityName, out oldConnection);
    }

    /// <summary>
    /// Находит Соединение пользователя по его имени.
    /// </summary>
    /// <param name="username">Имя пользователя.</param>
    /// <returns>Строку с соединеием.</returns>
    public string GetConnectionIdByName(string username)
    {
      return this.userWithSignalRIds
        .Where(pair => pair.Key == username)
        .Select(pair => pair.Value)
        .FirstOrDefault();
    }
  }
}