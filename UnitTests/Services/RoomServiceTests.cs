using System;
using System.Collections.Generic;
using System.Linq;
using DataService.Models;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NUnit.Framework;
using ScrumPokerWeb.DTO;
using UnitTests.TestData;

namespace UnitTests.Services
{
  [TestFixture()]
  public class RoomServiceTests : BaseTest
  {
    [SetUp]
    public void Before()
    {
      userService.AddUserToConnectionMap(TestOwner, "test");
      userService.AddUserToConnectionMap(wrongUser, "test2");
    }

    [Test()]
    public void GetAll()
    {
      var expected = DtoUtil.GetRoomsTOs(new List<Room>() {rooms.TestRoom1});
      var actual = roomService.GetAll();
      Assert.AreEqual(expected, actual);
    }

    [Test()]
    public void Get()
    {
      var expected = DtoUtil.GetRoomTO(rooms.TestRoom1);
      var actual = roomService.Get(1, TestOwner);
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetNotPermitted()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        roomService.Get(1, wrongUser);
      });
    }

    [Test()]
    public void Create()
    {
      
      roomService.Create(TestOwner, "pass", "10", "1");
      var expected = DtoUtil.GetRoomTO(rooms.RoomToInsert);
      var actual = roomService.Get(2, TestOwner);
      var expectedMethod = "UpdateRooms";
      var actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expected, actual);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    [Test()]
    public void AddUserToRoomAndGet()
    {
      roomService.AddUserToRoomAndGet(1, users.ValeraUser.Name, null);
      var expected = 3;
      var actual = roomService.Get(1, TestOwner).Users.Count;
      var expectedMethod = "UpdateUsersInRoom";
      var actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expected, actual);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    [Test]
    public void AddUserToRoomAndGetWithWrongPassword()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        roomService.Create(TestOwner, "666", "10", "1");
        roomService.AddUserToRoomAndGet(2, wrongUser, "42");
      });
    }

    [Test()]
    public void ChangeRoomPassword()
    {
      string newPassword = "666";
      roomService.ChangeRoomPassword(1, TestOwner, newPassword);
      var expectedMethod = "UpdateRooms";
      var actualMethod = InvokedSignalRMethod;
      Assert.AreEqual(expectedMethod, actualMethod);

      roomService.AddUserToRoomAndGet(1, users.ValeraUser.Name, newPassword);
      var expected = 3;
      var actual = roomService.Get(1, TestOwner).Users.Count;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ChangeRoomPasswordNoPermitted()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        roomService.ChangeRoomPassword(1, wrongUser, "42");
      });
    }

    [Test()]
    public void ExitFromRoom()
    {
      roomService.ExitFromRoom(1, users.JohnUser.Name);
      var expected = 1;
      var actual = roomService.Get(1, TestOwner).Users.Count;
      var expectedMethod = "UpdateUsersInRoom";
      var actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expected, actual);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    [Test()]
    public void DeleteUserFromRoom()
    {
      roomService.DeleteUserFromRoom(1, 1, TestOwner);
      var expected = 1;
      var actual = roomService.Get(1, TestOwner).Users.Count;
      var expectedMethod = "UpdateUsersInRoom";
      var actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expected, actual);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    [Test]
    public void DeleteUserFromRoomNoPermitted()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        roomService.DeleteUserFromRoom(1, 1, wrongUser);
      });
    }

    [Test()]
    public void Delete()
    {
      roomService.Delete(1, TestOwner);
      var expected = 0;
      var actual = roomService.GetAll().Count();
      var expectedMethod = "UpdateRooms";
      var actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expected, actual);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    [Test]
    public void DeleteNoPermitted()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        roomService.Delete(1, wrongUser);
      });
    }

    [Test()]
    public void StartNewDiscussion()
    {
      roomService.StartNewDiscussion(1, TestOwner, "test");
      var expected = "test";
      var actual = discussionResultService.Get(3).Theme;
      var expectedMethod = "StartDiscussion";
      var actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expected, actual);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    [Test]
    public void StartNewDiscussionNoPermitted()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        roomService.StartNewDiscussion(1, wrongUser, "test");
      });
    }

    [Test()]
    public void AddMarkInCurrentDiscussion()
    {
      roomService.StartNewDiscussion(1, TestOwner, "test");
      roomService.AddOrChangeMarkInCurrentDiscussion(1, TestOwner, "1");
      var expected = 1;
      var actual = discussionResultService.Get(3).UsersMarks[TestOwner];
      var expectedMethod = "UserVoted";
      var actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expected, actual);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    [Test]
    public void AddMarkInCurrentDiscussionNoPermitted()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        roomService.StartNewDiscussion(1, TestOwner, "test");
        roomService.AddOrChangeMarkInCurrentDiscussion(1, wrongUser, "1");
      });
    }

    [Test()]
    public void EndCurrentDiscussion()
    {
      roomService.StartNewDiscussion(1, TestOwner, "test");
      roomService.EndCurrentDiscussion(1, TestOwner, "test resume");
      var discussionResult = discussionResultService.Get(3);
      var expectedResume = "test resume";
      var actualResume = discussionResult.Resume;
      var expectedMethod = "EndDiscussion";
      var actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expectedResume, actualResume);
      Assert.AreNotEqual(DateTime.MinValue, discussionResult.Ending);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    [Test]
    public void EndCurrentDiscussionNoPermitted()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        roomService.StartNewDiscussion(1, TestOwner, "test");
        roomService.EndCurrentDiscussion(1, wrongUser, "test resume");
      });
    }

    [Test()]
    public void RestartDiscussion()
    {
      roomService.StartNewDiscussion(1, TestOwner, "test");
      roomService.EndCurrentDiscussion(1, TestOwner, "test resume");
      roomService.RestartDiscussion(1, TestOwner);
      string expectedResume = string.Empty;
      string actualResume = discussionResultService.Get(3).Resume;
      var expectedEnding = DateTime.MinValue;
      var actualEnding = discussionResultService.Get(3).Ending;
      var expectedMethod = "StartDiscussion";
      var actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expectedResume, actualResume);
      Assert.AreEqual(expectedEnding, actualEnding);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    [Test]
    public void RestartDiscussionNoPermitted()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        roomService.StartNewDiscussion(1, TestOwner, "test");
        roomService.EndCurrentDiscussion(1, TestOwner, "test resume");
        roomService.RestartDiscussion(1, wrongUser);
      });
    }
  }
}