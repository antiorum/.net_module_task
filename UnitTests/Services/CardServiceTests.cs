using System;
using System.Collections.Generic;
using System.Linq;
using DataService.Models;
using NUnit.Framework;
using ScrumPokerWeb.DTO;

namespace UnitTests.Services
{
  /// <summary>
  /// Тесты сервиса карт.
  /// </summary>
  [TestFixture]
  public class CardServiceTests : BaseTest
  {
    /// <summary>
    /// Тест удаления карты.
    /// </summary>
    [Test]
    public void Delete()
    {
      cardService.Delete(6, TestOwner);
      var expected = 5;
      var actual = cardService.GetAvailable(TestOwner).Count();

      var expectedMethod = "UpdateCards";
      var actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expectedMethod, actualMethod);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест удаления без прав.
    /// </summary>
    [Test]
    public void DeleteWithNoPermission()
    {
      Assert.Throws(typeof(AccessViolationException), delegate { cardService.Delete(1, TestOwner); });
    }

    /// <summary>
    /// Тест выдачи общих карт.
    /// </summary>
    [Test]
    public void GetAvailableOnlyCommon()
    {
      var actual = cardService.GetAvailable(null);
      var expected = DtoUtil.GetCardsDtos(new List<Card> { cards.CardOne, cards.CardFive, cards.CardSeven, cards.CardCoffee });
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест выдачи всех доступных карт.
    /// </summary>
    [Test]
    public void GetAvailableWithPrivate()
    {
      var actual = cardService.GetAvailable(TestOwner);
      var expected = DtoUtil.GetCardsDtos(new List<Card> { cards.CardOne, cards.CardFive, cards.CardSeven, cards.CardCoffee, cards.CardEight, cards.CardTen });
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест нахождения карты по ИД.
    /// </summary>
    [Test]
    public void GetById()
    {
      var actual = cardService.Get(1);
      var expected = DtoUtil.GetCardDto(cards.CardOne);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест создания новой карты.
    /// </summary>
    [Test]
    public void Save()
    {
      cardService.Save(cards.CardTwo, TestOwner);
      var expected = DtoUtil.GetCardDto(cards.CardTwo);
      var actual = cardService.Get(7);

      var expectedMethod = "UpdateCards";
      var actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expectedMethod, actualMethod);
      Assert.AreEqual(expected, actual);
    }
  }
}