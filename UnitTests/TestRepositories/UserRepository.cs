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
      this.Create(users.JohnUser);
      this.Create(users.ValeraUser);
      this.Create(users.BorkaUser);
    }
  }
}
