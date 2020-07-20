using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace UnitTests.Utils
{
  class ClientProxy : IClientProxy
  {
    public Task SendCoreAsync(string method, object[] args, CancellationToken cancellationToken = new CancellationToken())
    {
      return Task.Run(() =>
      {
        BaseTest.InvokedSignalRMethod = method;
      });
    }
  }
}
