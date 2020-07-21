using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ScrumPokerWeb
{
  /// <summary>
  /// Главный класс.
  /// </summary>
  public class Program
  {
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    /// <param name="args">Аргументы.</param>
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    /// <summary>
    /// Конфигурация хоста.
    /// </summary>
    /// <param name="args">Аргументы.</param>
    /// <returns>Билдер хоста.</returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
