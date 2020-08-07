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
  }
}
