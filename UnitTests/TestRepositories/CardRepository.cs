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
      this.Create(cards.CardOne);
      this.Create(cards.CardFive);
      this.Create(cards.CardSeven);
      this.Create(cards.CardEight);
      this.Create(cards.CardCoffee);
      this.Create(cards.CardTen);
    }
  }
}
