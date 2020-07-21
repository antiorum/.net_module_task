using System;
using System.Collections.Generic;
using DataService.Models;

namespace UnitTests.TestData
{
  /// <summary>
  /// Содержит тестовые данные результатов обсуждений.
  /// </summary>
  public class DiscussionResults
  {
    /// <summary>
    /// Тестовая оценка №1.
    /// </summary>
    public UserCard Mark1 = new UserCard()
    {
      Id = 1,
      DiscussionResult = null,
    };

    /// <summary>
    /// Тестовая оценка №2.
    /// </summary>
    public UserCard Mark2 = new UserCard()
    {
      Id = 2,
      DiscussionResult = null,
    };

    /// <summary>
    /// Тестовая оценка №3.
    /// </summary>
    public UserCard Mark3 = new UserCard()
    {
      Id = 3,
      DiscussionResult = null,
    };

    /// <summary>
    /// Тестовый результат обсуждения №1.
    /// </summary>
    public DiscussionResult TestDiscussionResult1 = new DiscussionResult()
    {
      Id = 1,
      Beginning = DateTime.MinValue,
      Ending = DateTime.MaxValue,
      Resume = "resume",
      Theme = "theme",
    };

    /// <summary>
    /// Тестовый результат обсуждения №2.
    /// </summary>
    public DiscussionResult TestDiscussionResult2 = new DiscussionResult()
    {
      Id = 2,
      Beginning = new DateTime(2020, 1, 1, 1, 0, 0),
      Ending = new DateTime(2020, 1, 1, 1, 30, 0),
      Resume = "resume",
      Theme = "theme",
    };

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    public DiscussionResults()
    {
      Cards cards = new Cards();
      Users users = new Users();
      this.Mark1.Card = cards.CardOne;
      this.Mark1.User = users.JohnUser;
      this.Mark2.Card = cards.CardFive;
      this.Mark2.User = users.ValeraUser;
      this.Mark3.Card = cards.CardCoffee;
      this.Mark3.User = users.BorkaUser;

      this.TestDiscussionResult1.UsersCards = new HashSet<UserCard>() { this.Mark1, this.Mark2, this.Mark3 };
      this.TestDiscussionResult2.UsersCards = new HashSet<UserCard>() { this.Mark1, this.Mark3 };
    }
  }
}
