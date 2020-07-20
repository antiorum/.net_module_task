using System;
using System.Collections.Generic;
using System.Linq;
using DataService.Models;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using ScrumPokerWeb.DTO;
using UnitTests.TestData;

namespace UnitTests.Services
{
  [TestFixture()]
  public class DeckServiceTests : BaseTest
  {

    [Test()]
    public void Delete()
    {
      deckService.Delete(2, TestOwner);
      int expected = 1;
      int actual = deckService.GetAvailable(TestOwner).Count();

      string expectedMethod = "UpdateDecks";
      string actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expected, actual);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    [Test()]
    public void DeleteNoPermission()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        deckService.Delete(1, TestOwner);
      });
    }

    [Test()]
    public void Update()
    {
      deckService.Update(2, TestOwner, "another deck", "1, 2, 3");
      var updatedDeck = deckService.Get(2);
      string expectedName = "another deck";
      string actualName = updatedDeck.Name;
      int expectedCardsInDeckCount = 3;
      int actualCardsInDeckCount = updatedDeck.Cards.Count;
      string expectedMethod = "UpdateDecks";
      string actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expectedName, actualName);
      Assert.AreEqual(expectedCardsInDeckCount, actualCardsInDeckCount);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    [Test()]
    public void UpdateNoPermission()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        deckService.Update(1, TestOwner, "another deck", "1, 2, 3");
      });
    }

    [Test()]
    public void Create()
    {
      deckService.Create(TestOwner, "test", "1, 2, 5");
      DeckDto newDeck = deckService.Get(3);
      string expectedName = "test";
      string actualName = newDeck.Name;
      int expectedCardsInDeckCount = 3;
      int actualCardsInDeckCount = newDeck.Cards.Count;
      string expectedMethod = "UpdateDecks";
      string actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expectedName, actualName);
      Assert.AreEqual(expectedCardsInDeckCount, actualCardsInDeckCount);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    [Test()]
    public void Get()
    {
      var expected = DtoUtil.GetDeckTO(decks.PrivateDeck);
      var actual = deckService.Get(2);
      Assert.AreEqual(expected, actual);
    }

    [Test()]
    public void GetAvailableOnlyCommon()
    {
      IEnumerable<DeckDto> expected = DtoUtil.GetDecksTOs(new List<Deck>() {decks.BaseDeck});
      var actual = deckService.GetAvailable(null);
      Assert.AreEqual(expected, actual);
    }

    [Test()]
    public void GetAvailableWithPrivate()
    {
      IEnumerable<DeckDto> expected = DtoUtil.GetDecksTOs(new List<Deck>() { decks.BaseDeck, decks.PrivateDeck });
      var actual = deckService.GetAvailable(TestOwner);
      Assert.AreEqual(expected, actual);
    }
  }
}