using System;
using System.Collections.Generic;
using System.Text;
using DataService.Models;

namespace UnitTests.TestData
{
  public class Decks
  {
    public  Deck BaseDeck = new Deck()
    {
      Id= 1,
      Owner = null,
      Name = "BaseDeck"
    };

    public  Deck PrivateDeck = new Deck()
    {
      Id = 2,
      Owner = "Bor'ka",
      Name = "PrivateDeck"
    };

    public Decks()
    {
      Cards cards = new Cards();
      BaseDeck.Cards = new HashSet<Card>() {cards.CardOne, cards.CardFive, cards.CardSeven, cards.CardCoffee};
      PrivateDeck.Cards = new HashSet<Card>() {cards.CardSeven, cards.CardCoffee, cards.CardEight, cards.CardTen};
    }
  }
}
