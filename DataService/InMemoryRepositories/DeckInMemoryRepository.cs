using DataService.InitData;
using DataService.Models;
using UnitTests.TestRepositories;

namespace DataService.InMemoryRepositories
{
  /// <summary>
  /// Тестовый репозиторий колод.
  /// </summary>
   public class DeckInMemoryRepository : BaseInMemoryRepository<Deck>
  {
    private long idCounter = 3;

    /// <summary>
    /// Конструктор репозитория с заполнением тестовым данными.
    /// </summary>
    public DeckInMemoryRepository()
    {
      Decks decks = new Decks();
      this.Save(decks.BaseDeck);
      this.Save(decks.PrivateDeck);
    }

    public override void Save(Deck item)
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
