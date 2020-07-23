using DataService.Models;
using UnitTests.TestData;

namespace UnitTests.TestRepositories
{
  /// <summary>
  /// Тестовый репозиторий комнат.
  /// </summary>
  public class RoomRepository : BaseRepository<Room>
  {
    /// <summary>
    /// Конструктор репозитория с заполнением тестовым данными.
    /// </summary>
    public RoomRepository()
    {
      Rooms testData = new Rooms();
      this.Save(testData.TestRoom1);
    }
  }
}
