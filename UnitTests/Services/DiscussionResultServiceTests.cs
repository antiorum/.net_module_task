using System;
using System.Collections.Generic;
using DataService.Models;
using NUnit.Framework;
using ScrumPokerWeb.DTO;

namespace UnitTests.Services
{
  /// <summary>
  /// Тесты сервиса.
  /// </summary>
  [TestFixture]
  public class DiscussionResultServiceTests : BaseTest
  {
    /// <summary>
    /// Тест добавления оценки.
    /// </summary>
    [Test]
    public void AddMark()
    {
      var newDiscussionId = discussionResultService.Create("test theme");
      discussionResultService.AddOrChangeMark(newDiscussionId, TestOwner, "1");
      var expectedMarksSize = 1;
      var actualMarksSize = discussionResultService.Get(newDiscussionId).UsersMarks.Count;
      Assert.AreEqual(expectedMarksSize, actualMarksSize);
    }

    /// <summary>
    /// Тест добавления оценки в завершенную дискуссию.
    /// </summary>
    [Test]
    public void AddMarkToCompletedDiscussion()
    {
      Assert.Throws(typeof(AccessViolationException),
        delegate { discussionResultService.AddOrChangeMark(1, TestOwner, "1"); });
    }

    /// <summary>
    /// Тест создания результата обсуждения.
    /// </summary>
    [Test]
    public void Create()
    {
      discussionResultService.Create("test");
      var actual = discussionResultService.Get(3).Theme;
      var expected = "test";
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест завершения обсуждения.
    /// </summary>
    [Test]
    public void EndDiscussion()
    {
      var newDiscussionId = discussionResultService.Create("test theme");
      discussionResultService.EndDiscussion(3, "test resume");
      var actualResume = discussionResultService.Get(3).Resume;
      var expectedResume = "test resume";
      var actualEnding = discussionResultService.Get(3).Ending;

      Assert.AreEqual(expectedResume, actualResume);
      Assert.AreNotEqual(DateTime.MinValue, actualEnding);
    }

    /// <summary>
    /// Тест поиска результат обсуждения по ИД.
    /// </summary>
    [Test]
    public void Get()
    {
      var actual = discussionResultService.Get(1);
      var expected = DtoConverters.GetDiscussionResultDto(discussionResults.TestDiscussionResult1);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест показа всех результатов обсуждений.
    /// </summary>
    [Test]
    public void GetAll()
    {
      var expected = DtoConverters.GetDiscussionResultsDtos(new List<DiscussionResult> { discussionResults.TestDiscussionResult1, discussionResults.TestDiscussionResult2 });
      var actual = discussionResultService.GetAll();
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест показа результатов обсуждений одного из пользователей.
    /// </summary>
    [Test]
    public void GetByName()
    {
      var expected = DtoConverters.GetDiscussionResultsDtos(new List<DiscussionResult> { discussionResults.TestDiscussionResult1, discussionResults.TestDiscussionResult2 });
      var actual = discussionResultService.GetByName(TestOwner);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест сброса результатов обсуждения.
    /// </summary>
    [Test]
    public void ResetDiscussionResult()
    {
      var newDiscussionId = discussionResultService.Create("test theme");
      discussionResultService.EndDiscussion(3, "test resume");
      discussionResultService.ResetDiscussionResult(3);
      var actualEnding = discussionResultService.Get(3).Ending;
      var expectedEnding = DateTime.MinValue;
      var actualResume = discussionResultService.Get(3).Resume;
      var expectedResume = string.Empty;

      Assert.AreEqual(expectedEnding, actualEnding);
      Assert.AreEqual(expectedResume, actualResume);
    }
  }
}