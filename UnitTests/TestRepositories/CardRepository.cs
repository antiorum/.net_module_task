using System;
using System.Collections.Generic;
using System.Text;
using DataService.Models;
using UnitTests.TestData;

namespace UnitTests.TestRepositories
{
    class CardRepository : BaseRepository<Card>
    {
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
