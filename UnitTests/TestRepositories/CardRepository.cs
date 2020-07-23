using DataService.Models;
using UnitTests.TestData;

namespace UnitTests.TestRepositories
{
  /// <summary>
  /// Тестовый репозиторий карт.
  /// </summary>
  public class CardRepository : BaseRepository<Card>
  {
    /// <summary>
    /// Конструктор репозитория с заполнением тестовым данными.
    /// </summary>
    public CardRepository()
    {
      Cards cards = new Cards();
      this.Save(cards.CardOne);
      this.Save(cards.CardFive);
      this.Save(cards.CardSeven);
      this.Save(cards.CardEight);
      this.Save(cards.CardCoffee);
      this.Save(cards.CardTen);
    }
  }
}
