using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace UnitTests.Utils
{
  /// <summary>
  /// Моковая реализация интерфейса.
  /// </summary>
  public class ClientProxy : IClientProxy
  {
    /// <summary>
    /// Моковая реализация метода, который записывает название вызванного метода в тестовый класс.
    /// </summary>
    /// <param name="method">Вызываемый метод.</param>
    /// <param name="args">Аргументы метода.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Асинхронно выполняемое действие.</returns>
    public Task SendCoreAsync(string method, object[] args, CancellationToken cancellationToken = new CancellationToken())
    {
      return Task.Run(() =>
      {
        BaseTest.InvokedSignalRMethod = method;
      });
    }
  }
}
