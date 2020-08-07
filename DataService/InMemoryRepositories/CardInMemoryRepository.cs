using DataService.InitData;
using DataService.Models;
using UnitTests.TestRepositories;

namespace DataService.InMemoryRepositories
{
  /// <summary>
  /// Тестовый репозиторий карт.
  /// </summary>
  public class CardInMemoryRepository : BaseInMemoryRepository<Card>
  {
    private long idCounter = 8;

    /// <summary>
    /// Конструктор репозитория с заполнением тестовым данными.
    /// </summary>
    public CardInMemoryRepository()
    {
      Cards cards = new Cards();
      this.Save(cards.CardOne);
      this.Save(cards.CardFive);
      this.Save(cards.CardSeven);
      this.Save(cards.CardEight);
      this.Save(cards.CardCoffee);
      this.Save(cards.CardTen);
    }

    public override void Save(Card item)
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
