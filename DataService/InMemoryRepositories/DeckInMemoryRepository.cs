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
    /// <summary>
    /// Конструктор репозитория с заполнением тестовым данными.
    /// </summary>
    public DeckInMemoryRepository()
    {
      Decks decks = new Decks();
      this.Save(decks.BaseDeck);
      this.Save(decks.PrivateDeck);
    }
  }
}
