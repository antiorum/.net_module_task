using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace UnitTests.Utils
{
  /// <summary>
  /// Моковая реализация интерфейса IGroupManager.
  /// </summary>
  public class GroupManager : IGroupManager
  {
    private GroupManager()
    {
      this.groupsConnections = new Dictionary<string, ISet<string>>();
    }

    /// <summary>
    /// Создаёт или возвращает инстанс этого класса.
    /// </summary>
    /// <value>Объект этого класса.</value>
    public static GroupManager GetGroupManager { get; } = new GroupManager();

    /// <summary>
    /// Хранилище групп соединений.
    /// </summary>
    private Dictionary<string, ISet<string>> groupsConnections;

    /// <summary>
    /// Добавление соединения к группе.
    /// </summary>
    /// <param name="connectionId">ИД соединения.</param>
    /// <param name="groupName">Имя группы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Асинхронное действие.</returns>
    public Task AddToGroupAsync(string connectionId, string groupName,
        CancellationToken cancellationToken = new CancellationToken())
    {
      return Task.Run(() =>
      {
        if (!groupsConnections.ContainsKey(groupName))
        {
          groupsConnections.Add(groupName, new HashSet<string>());
        }

        ISet<string> connections = groupsConnections.GetValueOrDefault(groupName);
        if (!connections.Contains(connectionId))
        {
          connections.Add(connectionId);
        }
      });
    }

    /// <summary>
    /// Удаление соединения из группы.
    /// </summary>
    /// <param name="connectionId">ИД соединения.</param>
    /// <param name="groupName">Имя группы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Асинхронное действие.</returns>
    public Task RemoveFromGroupAsync(string connectionId, string groupName,
        CancellationToken cancellationToken = new CancellationToken())
    {
      return Task.Run(() =>
      {
        if (groupsConnections.ContainsKey(groupName))
        {
          this.groupsConnections[groupName].Remove(connectionId);
        }
      });
    }
  }
}
