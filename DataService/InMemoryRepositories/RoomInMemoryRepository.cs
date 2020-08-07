using DataService.InitData;
using DataService.Models;
using UnitTests.TestRepositories;

namespace DataService.InMemoryRepositories
{
  /// <summary>
  /// Тестовый репозиторий комнат.
  /// </summary>
  public class RoomInMemoryRepository : BaseInMemoryRepository<Room>
  {
    /// <summary>
    /// Конструктор репозитория с заполнением тестовым данными.
    /// </summary>
    public RoomInMemoryRepository()
    {
      Rooms testData = new Rooms();
      this.Save(testData.TestRoom1);
    }
  }
}
