using DataService.InitData;
using DataService.Models;
using UnitTests.TestRepositories;

namespace DataService.InMemoryRepositories
{
  /// <summary>
  /// Тестовый репозиторий результатов дискуссий.
  /// </summary>
  public class DiscussionResultInMemoryRepository : BaseInMemoryRepository<DiscussionResult>
  {
    private long idCounter = 3;

    /// <summary>
    /// Конструктор репозитория с заполнением тестовым данными.
    /// </summary>
    public DiscussionResultInMemoryRepository()
    {
      DiscussionResults results = new DiscussionResults();
      this.Save(results.TestDiscussionResult1);
      this.Save(results.TestDiscussionResult2);
    }

    public override void Save(DiscussionResult item)
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
