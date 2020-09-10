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
    private long idCounter = 3;

    /// <summary>
    /// Конструктор репозитория с заполнением тестовым данными.
    /// </summary>
    public RoomInMemoryRepository()
    {
      Rooms testData = new Rooms();
      this.Save(testData.TestRoom1);
    }

    /// <summary>
    /// Сохраняет сущность в репозиторий, при необходимости назначает ID.
    /// </summary>
    /// <param name="item">Сохраняемая сущность.</param>
    public override void Save(Room item)
    {
      if (item.Id == 0)
      {
        item.Id = idCounter;
        idCounter++;
      }
      base.Save(item);
    }
  }
}
