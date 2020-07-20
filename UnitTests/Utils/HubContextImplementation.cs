using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using ScrumPokerWeb.SignalR;

namespace UnitTests.Utils
{
  public class HubContextImplementation : IHubContext<RoomsHub>
  {
    private HubContextImplementation()
    {
      Groups = GroupManager.GetGroupManager;
      Clients = HubClientsImplementation.GetHubClients;
    }

    public static IHubContext<RoomsHub> GetContext { get; } = new HubContextImplementation();

    public IHubClients Clients { get; }
    public IGroupManager Groups { get; }
  }
}
