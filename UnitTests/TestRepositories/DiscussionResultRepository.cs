using DataService.Models;
using UnitTests.TestData;

namespace UnitTests.TestRepositories
{
  /// <summary>
  /// Тестовый репозиторий результатов дискуссий.
  /// </summary>
  public class DiscussionResultRepository : BaseRepository<DiscussionResult>
  {
    /// <summary>
    /// Конструктор репозитория с заполнением тестовым данными.
    /// </summary>
    public DiscussionResultRepository()
    {
      DiscussionResults results = new DiscussionResults();
      this.Create(results.TestDiscussionResult1);
      this.Create(results.TestDiscussionResult2);
    }
  }
}
