using DataService.InitData;
using DataService.InMemoryRepositories;
using DataService.Models;

namespace UnitTests.TestRepositories
{
  /// <summary>
  /// Тестовый репозиторий пользователей.
  /// </summary>
  public class UserInMemoryRepository : BaseInMemoryRepository<User>
  {
    private long idCounter = 4;

    /// <summary>
    /// Конструктор репозитория с заполнением тестовым данными.
    /// </summary>
    public UserInMemoryRepository()
    {
      Users users = new Users();
      this.Save(users.JohnUser);
      this.Save(users.ValeraUser);
      this.Save(users.BorkaUser);
    }

    public override void Save(User item)
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
