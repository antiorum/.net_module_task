using System;
using System.Collections.Generic;
using System.Linq;
using DataService.Models;
using NUnit.Framework;
using ScrumPokerWeb.DTO;

namespace UnitTests.Services
{
  /// <summary>
  /// Тесты сервиса колод.
  /// </summary>
  [TestFixture]
  public class DeckServiceTests : BaseTest
  {
    /// <summary>
    /// Тест создания колоды.
    /// </summary>
    [Test]
    public void Create()
    {
      deckService.Create(TestOwner, "test", "1, 2, 5");
      var newDeck = deckService.Get(3);
      var expectedName = "test";
      var actualName = newDeck.Name;
      var expectedCardsInDeckCount = 3;
      var actualCardsInDeckCount = newDeck.Cards.Count;
      var expectedMethod = "UpdateDecks";
      var actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expectedName, actualName);
      Assert.AreEqual(expectedCardsInDeckCount, actualCardsInDeckCount);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    /// <summary>
    /// Тест удаления колоды.
    /// </summary>
    [Test]
    public void Delete()
    {
      deckService.Delete(2, TestOwner);
      var expected = 1;
      var actual = deckService.GetAvailable(TestOwner).Count();

      var expectedMethod = "UpdateDecks";
      var actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expected, actual);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    /// <summary>
    /// Тест удаления без прав.
    /// </summary>
    [Test]
    public void DeleteNoPermission()
    {
      Assert.Throws(typeof(AccessViolationException), delegate { deckService.Delete(1, TestOwner); });
    }

    /// <summary>
    /// Тест получения по ИД.
    /// </summary>
    [Test]
    public void Get()
    {
      var expected = DtoConverters.GetDeckDto(decks.PrivateDeck);
      var actual = deckService.Get(2);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест получения общих колод.
    /// </summary>
    [Test]
    public void GetAvailableOnlyCommon()
    {
      var expected = DtoConverters.GetDecksDtos(new List<Deck> { decks.BaseDeck });
      var actual = deckService.GetAvailable(null);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест получения всех доступных колод.
    /// </summary>
    [Test]
    public void GetAvailableWithPrivate()
    {
      var expected = DtoConverters.GetDecksDtos(new List<Deck> { decks.BaseDeck, decks.PrivateDeck });
      var actual = deckService.GetAvailable(TestOwner);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест изменения колоды.
    /// </summary>
    [Test]
    public void Update()
    {
      deckService.Update(2, TestOwner, "another deck", "1, 2, 3");
      var updatedDeck = deckService.Get(2);
      var expectedName = "another deck";
      var actualName = updatedDeck.Name;
      var expectedCardsInDeckCount = 3;
      var actualCardsInDeckCount = updatedDeck.Cards.Count;
      var expectedMethod = "UpdateDecks";
      var actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expectedName, actualName);
      Assert.AreEqual(expectedCardsInDeckCount, actualCardsInDeckCount);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    /// <summary>
    /// Тест изменения без нужных прав.
    /// </summary>
    [Test]
    public void UpdateNoPermission()
    {
      Assert.Throws(typeof(AccessViolationException),
        delegate { deckService.Update(1, TestOwner, "another deck", "1, 2, 3"); });
    }
  }
}