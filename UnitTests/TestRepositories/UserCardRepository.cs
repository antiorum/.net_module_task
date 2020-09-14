using DataService.Models;
using UnitTests.TestData;

namespace UnitTests.TestRepositories
{
  /// <summary>
  /// Репозиторий оценок пользователей (тестовый).
  /// </summary>
  public class UserCardRepository : BaseRepository<UserCard>
  {
    /// <summary>
    /// Конструктор, который заполняет репоизиторий тестовыми данными.
    /// </summary>
    public UserCardRepository()
    {
      var marks = new UserMarks();
      this.Save(marks.Mark1);
      this.Save(marks.Mark2);
      this.Save(marks.Mark3);
    }
  }
}