using DataService.Models;
using UnitTests.TestData;

namespace UnitTests.TestRepositories
{
  /// <summary>
  /// Тестовый репозиторий колод.
  /// </summary>
   public class DeckRepository : BaseRepository<Deck>
  {
    /// <summary>
    /// Конструктор репозитория с заполнением тестовым данными.
    /// </summary>
    public DeckRepository()
    {
      Decks decks = new Decks();
      this.Save(decks.BaseDeck);
      this.Save(decks.PrivateDeck);
    }
  }
}
