using System.Collections.Generic;

namespace ScrumPokerWeb.Services
{
  /// <summary>
  ///   Класс с утилитными методами для сервисов.
  /// </summary>
  public static class ServiceUtils
  {
    /// <summary>
    ///   Парсит строку.
    /// </summary>
    /// <param name="source">Строка-источник.</param>
    /// <returns>Коллекцию целых чисел.</returns>
    public static IEnumerable<long> ParseIds(string source)
    {
      var result = new List<long>();
      var ids = source.Split(", ");
      foreach (var id in ids) result.Add(long.Parse(id));
      return result;
    }
  }
}