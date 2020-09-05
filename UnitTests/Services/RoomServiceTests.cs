using System;
using System.Collections.Generic;
using System.Linq;
using DataService.Models;
using NUnit.Framework;
using ScrumPokerWeb.DTO;

namespace UnitTests.Services
{
  /// <summary>
  /// Тесты сервиса комнат.
  /// </summary>
  [TestFixture]
  public class RoomServiceTests : BaseTest
  {
    /// <summary>
    /// Заполнение тестовыми данными словаря соединений.
    /// </summary>
    [SetUp]
    public void Before()
    {
      userService.AddUserToConnectionMap(TestOwner, "test");
      userService.AddUserToConnectionMap(WrongUser, "test2");
    }

    /// <summary>
    /// Тест добавления оценки в текущее обсуждение.
    /// </summary>
    [Test]
    public void AddMarkInCurrentDiscussion()
    {
      roomService.StartNewDiscussion(1, TestOwner, "test");
      roomService.AddOrChangeMarkInCurrentDiscussion(1, TestOwner, "1", 3);
      var expected = 1;
      var actual = discussionResultService.Get(3).UsersMarks[TestOwner].Value;
      var expectedMethod = "UserVoted";
      var actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expected, actual);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    /// <summary>
    /// Тест добавления оценки в обсуждение без прав.
    /// </summary>
    [Test]
    public void AddMarkInCurrentDiscussionNoPermitted()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        roomService.StartNewDiscussion(1, TestOwner, "test");
        roomService.AddOrChangeMarkInCurrentDiscussion(1, WrongUser, "1", 3);
      });
    }

    /// <summary>
    /// Тест логина в комнату.
    /// </summary>
    [Test]
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

    /// <summary>
    /// Тест логина с неверными паролем.
    /// </summary>
    [Test]
    public void AddUserToRoomAndGetWithWrongPassword()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        roomService.Create(TestOwner, "666", "test", "10", "1");
        roomService.AddUserToRoomAndGet(2, WrongUser, "42");
      });
    }

    /// <summary>
    /// Тест изменения пароля комнаты.
    /// </summary>
    [Test]
    public void ChangeRoomPassword()
    {
      var newPassword = "666";
      roomService.ChangeRoomPassword(1, TestOwner, newPassword);
      var expectedMethod = "UpdateRooms";
      var actualMethod = InvokedSignalRMethod;
      Assert.AreEqual(expectedMethod, actualMethod);

      roomService.AddUserToRoomAndGet(1, users.ValeraUser.Name, newPassword);
      var expected = 3;
      var actual = roomService.Get(1, TestOwner).Users.Count;
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест изменения пароля без прав.
    /// </summary>
    [Test]
    public void ChangeRoomPasswordNoPermitted()
    {
      Assert.Throws(typeof(AccessViolationException), delegate { roomService.ChangeRoomPassword(1, WrongUser, "42"); });
    }

    /// <summary>
    /// Тест создания новой комнаты.
    /// </summary>
    [Test]
    public void Create()
    {
      roomService.Create(TestOwner, "pass", "RoomToInsert", "10", "1");
      var expectedMethod = "UpdateRooms";
      var actualMethod = InvokedSignalRMethod;
      Assert.AreEqual(expectedMethod, actualMethod);

      var expected = DtoConverters.GetRoomDto(rooms.RoomToInsert);
      var actual = roomService.Get(2, TestOwner);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест удаления комнаты.
    /// </summary>
    [Test]
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

    /// <summary>
    /// Тест удаления комнаты без прав.
    /// </summary>
    [Test]
    public void DeleteNoPermitted()
    {
      Assert.Throws(typeof(AccessViolationException), delegate { roomService.Delete(1, WrongUser); });
    }

    /// <summary>
    /// Тест удаления пользователя из комнаты.
    /// </summary>
    [Test]
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

    /// <summary>
    /// Тест удаления пользователя из комнаты без прав.
    /// </summary>
    [Test]
    public void DeleteUserFromRoomNoPermitted()
    {
      Assert.Throws(typeof(AccessViolationException), delegate { roomService.DeleteUserFromRoom(1, 1, WrongUser); });
    }

    /// <summary>
    /// Тест завершения дискуссии.
    /// </summary>
    [Test]
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

    /// <summary>
    /// Тест завершения дискуссии без прав.
    /// </summary>
    [Test]
    public void EndCurrentDiscussionNoPermitted()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        roomService.StartNewDiscussion(1, TestOwner, "test");
        roomService.EndCurrentDiscussion(1, WrongUser, "test resume");
      });
    }

    /// <summary>
    /// Тест выхода из комнаты.
    /// </summary>
    [Test]
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

    /// <summary>
    /// Тест получения комнаты по ИД.
    /// </summary>
    [Test]
    public void Get()
    {
      var expected = DtoConverters.GetRoomDto(rooms.TestRoom1);
      var actual = roomService.Get(1, TestOwner);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест получения всех комнат.
    /// </summary>
    [Test]
    public void GetAll()
    {
      var expected = DtoConverters.GetRoomsDtos(new List<Room> { rooms.TestRoom1 });
      var actual = roomService.GetAll();
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест получения комнаты без прав.
    /// </summary>
    [Test]
    public void GetNotPermitted()
    {
      Assert.Throws(typeof(AccessViolationException), delegate { roomService.Get(1, WrongUser); });
    }

    /// <summary>
    /// Тест сброса дискуссии.
    /// </summary>
    [Test]
    public void RestartDiscussion()
    {
      roomService.StartNewDiscussion(1, TestOwner, "test");
      roomService.EndCurrentDiscussion(1, TestOwner, "test resume");
      roomService.RestartDiscussion(1, TestOwner, 3);
      var expectedResume = string.Empty;
      var actualResume = discussionResultService.Get(3).Resume;
      var expectedEnding = DateTime.MinValue;
      var actualEnding = discussionResultService.Get(3).Ending;
      var expectedMethod = "StartDiscussion";
      var actualMethod = InvokedSignalRMethod;

      Assert.AreEqual(expectedResume, actualResume);
      Assert.AreEqual(expectedEnding, actualEnding);
      Assert.AreEqual(expectedMethod, actualMethod);
    }

    /// <summary>
    /// Тест сброса дискуссии без прав.
    /// </summary>
    [Test]
    public void RestartDiscussionNoPermitted()
    {
      Assert.Throws(typeof(AccessViolationException), delegate
      {
        roomService.StartNewDiscussion(1, TestOwner, "test");
        roomService.EndCurrentDiscussion(1, TestOwner, "test resume");
        roomService.RestartDiscussion(1, WrongUser, 3);
      });
    }

    /// <summary>
    /// Тест начала новой дискуссии.
    /// </summary>
    [Test]
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

    /// <summary>
    /// Тест начала дискуссии без прав.
    /// </summary>
    [Test]
    public void StartNewDiscussionNoPermitted()
    {
      Assert.Throws(typeof(AccessViolationException),
        delegate { roomService.StartNewDiscussion(1, WrongUser, "test"); });
    }
  }
}