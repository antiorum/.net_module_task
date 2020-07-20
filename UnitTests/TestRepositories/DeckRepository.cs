using System;
using System.Collections.Generic;
using System.Text;
using DataService.Models;
using UnitTests.TestData;

namespace UnitTests.TestRepositories
{
    class DeckRepository : BaseRepository<Deck>
    {
      public DeckRepository()
      {
        Decks decks = new Decks();
        this.Create(decks.BaseDeck);
        this.Create(decks.PrivateDeck);
      }
    }
}
