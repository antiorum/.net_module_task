using System.Collections.Generic;
using DataService.Models;

namespace DataService.InitData
{
  /// <summary>
  /// Содержит тестовые данные для колод.
  /// </summary>
  public class Decks
  {
    /// <summary>
    /// Тестовая колода №1.
    /// </summary>
    /// <value>Сущность-колода.</value>
    public Deck BaseDeck { get; } = new Deck()
    {
      Id = 1,
      Owner = null,
      Name = "BaseDeck"
    };

    /// <summary>
    /// Тестовая колода №2.
    /// </summary>
    /// <value>Сущность-колода.</value>
    public Deck PrivateDeck { get; } = new Deck()
    {
      Id = 2,
      Owner = "Bor'ka",
      Name = "PrivateDeck"
    };

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    public Decks()
    {
      Cards cards = new Cards();
      this.BaseDeck.Cards = new HashSet<Card>() { cards.CardOne, cards.CardFive, cards.CardSeven, cards.CardCoffee };
      this.PrivateDeck.Cards = new HashSet<Card>() { cards.CardSeven, cards.CardCoffee, cards.CardEight, cards.CardTen };
    }
  }
}
