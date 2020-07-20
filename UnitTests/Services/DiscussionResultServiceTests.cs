using System;
using System.Collections.Generic;
using DataService.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NUnit.Framework;
using ScrumPokerWeb.DTO;
using UnitTests.TestData;

namespace UnitTests.Services
{
  [TestFixture()]
  public class DiscussionResultServiceTests : BaseTest
  {
    [Test()]
    public void Get()
    {
      DiscussionResultDto actual = discussionResultService.Get(1);
      DiscussionResultDto expected = DtoUtil.GetDiscussionResultTO(discussionResults.TestDiscussionResult1);
      Assert.AreEqual(expected, actual);
    }

    [Test()]
    public void GetAll()
    {
      var expected = DtoUtil.GetDiscussionResultsTOs(new List<DiscussionResult>(){discussionResults.TestDiscussionResult1, discussionResults.TestDiscussionResult2});
      var actual = discussionResultService.GetAll();
      Assert.AreEqual(expected, actual);
    }

    [Test()]
    public void Create()
    {
      discussionResultService.Create("test");
      var actual = discussionResultService.Get(3).Theme;
      var expected = "test";
      Assert.AreEqual(expected, actual);
    }

    [Test()]
    public void GetByName()
    {
      var expected = DtoUtil.GetDiscussionResultsTOs(new List<DiscussionResult>() { discussionResults.TestDiscussionResult1, discussionResults.TestDiscussionResult2 });
      var actual = discussionResultService.GetByName(TestOwner);
      Assert.AreEqual(expected, actual);
    }

    [Test()]
    public void EndDiscussion()
    {
      long newDiscussionId = discussionResultService.Create("test theme");
      discussionResultService.EndDiscussion(3, "test resume");
      var actualResume = discussionResultService.Get(3).Resume;
      var expectedResume = "test resume";
      var actualEnding = discussionResultService.Get(3).Ending;

      Assert.AreEqual(expectedResume, actualResume);
      Assert.AreNotEqual(DateTime.MinValue, actualEnding);
    }

    [Test()]
    public void ResetDiscussionResult()
    {
      long newDiscussionId = discussionResultService.Create("test theme");
      discussionResultService.EndDiscussion(3, "test resume");
      discussionResultService.ResetDiscussionResult(3);
      var actualEnding = discussionResultService.Get(3).Ending;
      var expectedEnding = DateTime.MinValue;
      var actualResume = discussionResultService.Get(3).Resume;
      var expectedResume = "";

      Assert.AreEqual(expectedEnding, actualEnding);
      Assert.AreEqual(expectedResume, actualResume);
    }

    [Test()]
    public void AddMark()
    {
      long newDiscussionId = discussionResultService.Create("test theme");
      discussionResultService.AddOrChangeMark(newDiscussionId, TestOwner, "1");
      var expectedMarksSize = 1;
      var actualMarksSize = discussionResultService.Get(newDiscussionId).UsersMarks.Count;
      Assert.AreEqual(expectedMarksSize, actualMarksSize);
    }

    [Test]
    public void AddMarkToCompletedDiscussion()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        discussionResultService.AddOrChangeMark(1, TestOwner, "1");
      });
    }
  }
}
