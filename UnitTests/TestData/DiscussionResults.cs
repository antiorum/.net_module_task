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
      var marks = new UserMarks();
      this.TestDiscussionResult1.UsersCards = new HashSet<UserCard>() { marks.Mark1, marks.Mark2, marks.Mark3 };
      this.TestDiscussionResult2.UsersCards = new HashSet<UserCard>() { marks.Mark1, marks.Mark3 };
    }
  }
}
