using Microsoft.AspNetCore.SignalR;
using ScrumPokerWeb.SignalR;

namespace UnitTests.Utils
{
  /// <summary>
  /// Моковая реализация интерфейса IHubContext.
  /// </summary>
  public class HubContextImplementation : IHubContext<RoomsHub>
  {
    private HubContextImplementation()
    {
      this.Groups = GroupManager.GetGroupManager;
      this.Clients = HubClientsImplementation.GetHubClients;
    }

    /// <summary>
    /// Создаёт или возвращает экземпляр класса.
    /// </summary>
    /// <value>Экземпляр класса, реализующий интерфейс IHubContext.</value> 
    public static IHubContext<RoomsHub> GetContext { get; } = new HubContextImplementation();

    /// <summary>
    /// Клиенты.
    /// </summary>
    /// <value>Экземпляр класса, реализующий интерфейс IHubClients.</value>
    public IHubClients Clients { get; }

    /// <summary>
    /// Группы.
    /// </summary>
    /// <value>Экземпляр класса, реализующий интерфейс IGroupManager.</value>
    public IGroupManager Groups { get; }
  }
}
