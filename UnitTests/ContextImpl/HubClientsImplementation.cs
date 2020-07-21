using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;

namespace UnitTests.Utils
{
  /// <summary>
  /// Моковая реализация интерфейса IHubClients.
  /// </summary>
  public class HubClientsImplementation : IHubClients
  {
    private HubClientsImplementation()
    {
      this.All = new ClientProxy();
    }

    /// <summary>
    /// Создаёт или возвращает объект этого класса.
    /// </summary>
    /// <value>Объект интерйеса IHubClients.</value>
    public static IHubClients GetHubClients { get; } = new HubClientsImplementation();

    /// <summary>
    /// Послать сообщение всем, кроме опрделенных.
    /// </summary>
    /// <param name="excludedConnectionIds">Исключенные соединения.</param>
    /// <returns>Объект интерфейса IClientProxy.</returns>
    public IClientProxy AllExcept(IReadOnlyList<string> excludedConnectionIds)
    {
      return new ClientProxy();
    }

    /// <summary>
    /// Послать сообщение по ИД.
    /// </summary>
    /// <param name="connectionId">ИД клиента.</param>
    /// <returns>Объект интерфейса IClientProxy.</returns>
    public IClientProxy Client(string connectionId)
    {
      return new ClientProxy();
    }

    /// <summary>
    /// Послать сообщения нескольким клиентам.
    /// </summary>
    /// <param name="connectionIds">ИД клиентов.</param>
    /// <returns>Объект интерфейса IClientProxy.</returns>
    public IClientProxy Clients(IReadOnlyList<string> connectionIds)
    {
      return new ClientProxy();
    }

    /// <summary>
    /// Послать сообщение группе.
    /// </summary>
    /// <param name="groupName">Имя группы.</param>
    /// <returns>Объект интерфейса IClientProxy.</returns>
    public IClientProxy Group(string groupName)
    {
      return new ClientProxy();
    }

    /// <summary>
    /// Послать сообщение всем в группе кроме определенных ИД.
    /// </summary>
    /// <param name="groupName">Имя группы.</param>
    /// <param name="excludedConnectionIds">Исключенные ИД.</param>
    /// <returns>Объект интерфейса IClientProxy.</returns>
    public IClientProxy GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds)
    {
      return new ClientProxy();
    }

    /// <summary>
    /// Послать сообщение нескольким группам.
    /// </summary>
    /// <param name="groupNames">Имена групп.</param>
    /// <returns>Объект интерфейса IClientProxy.</returns>
    public IClientProxy Groups(IReadOnlyList<string> groupNames)
    {
      return new ClientProxy();
    }

    /// <summary>
    /// Послать определенному пользователю.
    /// </summary>
    /// <param name="userId">ИД пользователя.</param>
    /// <returns>Объект интерфейса IClientProxy.</returns>
    public IClientProxy User(string userId)
    {
      return new ClientProxy();
    }

    /// <summary>
    /// Послать сообщение нескольким пользователям.
    /// </summary>
    /// <param name="userIds">ИД пользователей.</param>
    /// <returns>Объект интерфейса IClientProxy.</returns>
    public IClientProxy Users(IReadOnlyList<string> userIds)
    {
      return new ClientProxy();
    }

    /// <summary>
    /// Все клиенты.
    /// </summary>
    /// <value>Объект интерфейса IClientProxy.</value>
    public IClientProxy All { get; }
  }
}
