using System;
using System.Collections.Generic;
using System.Linq;
using DataService.Models;
using NUnit.Framework;
using ScrumPokerWeb.DTO;
using UnitTests.TestData;
using UnitTests.Utils;

namespace UnitTests.Services
{
  [TestFixture()]
  public class CardServiceTests : BaseTest
  {
    [Test()]
    public void GetAvailableOnlyCommon()
    {
      IEnumerable<CardDto> actual = cardService.GetAvailable(null);
      IEnumerable<CardDto> expected = DtoUtil.GetCardsTOs(new List<Card>() { cards.CardOne, cards.CardFive, cards.CardSeven, cards.CardCoffee });
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetAvailableWithPrivate()
    {
      IEnumerable<CardDto> actual = cardService.GetAvailable(TestOwner);
      IEnumerable<CardDto> expected = DtoUtil.GetCardsTOs(new List<Card>() { cards.CardOne, cards.CardFive, cards.CardSeven, cards.CardCoffee, cards.CardEight, cards.CardTen });
      Assert.AreEqual(expected, actual);
    }

    [Test()]
    public void GetById()
    {
      CardDto actual = cardService.Get(1);
      CardDto expected = DtoUtil.GetCardTO(cards.CardOne);
      Assert.AreEqual(expected, actual);
    }

    [Test()]
    public void Save()
    {
      cardService.Save(cards.CardTwo, TestOwner);
      CardDto expected = DtoUtil.GetCardTO(cards.CardTwo);
      CardDto actual = cardService.Get(7);

      string expectedMethod = "UpdateCards";
      string actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expectedMethod, actualMethod);
      Assert.AreEqual(expected, actual);
    }

    [Test()]
    public void Delete()
    {
      cardService.Delete(6, TestOwner);
      int expected = 5;
      int actual = cardService.GetAvailable(TestOwner).Count();

      string expectedMethod = "UpdateCards";
      string actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expectedMethod, actualMethod);
      Assert.AreEqual(expected, actual);
    }

    [Test()]
    public void DeleteWithNoPermission()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        cardService.Delete(1, TestOwner);
      });
    }
  }
}