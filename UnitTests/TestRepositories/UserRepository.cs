using DataService.Models;
using UnitTests.TestData;

namespace UnitTests.TestRepositories
{
  /// <summary>
  /// Тестовый репозиторий пользователей.
  /// </summary>
  public class UserRepository : BaseRepository<User>
  {
    /// <summary>
    /// Конструктор репозитория с заполнением тестовым данными.
    /// </summary>
    public UserRepository()
    {
      Users users = new Users();
      this.Save(users.JohnUser);
      this.Save(users.ValeraUser);
      this.Save(users.BorkaUser);
    }
  }
}
