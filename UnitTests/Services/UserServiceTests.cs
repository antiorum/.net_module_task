using System.Collections.Generic;
using DataService.Models;
using NUnit.Framework;
using ScrumPokerWeb.DTO;

namespace UnitTests.Services
{
  /// <summary>
  /// Тесты сервиса пользователей.
  /// </summary>
  [TestFixture]
  public class UserServiceTests : BaseTest
  {
    /// <summary>
    /// Тест добавления соответствия пользователя с его соединением.
    /// </summary>
    [Test]
    public void AddUserToConnectionMap()
    {
      userService.AddUserToConnectionMap(TestOwner, "testID");
      var expected = "testID";
      var actual = userService.GetConnectionIdByName(TestOwner);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест создания нового пользователя.
    /// </summary>
    [Test]
    public void Create()
    {
      userService.Create(users.UserForInsert);
      var expected = users.UserForInsert;
      var actual = userService.GetByName(expected.Name);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест удаления соответствия пользователя с его соединением.
    /// </summary>
    [Test]
    public void DeleteUserFromConnectionMap()
    {
      userService.DeleteUserFromConnectionMap("TestUser");
      var actual = userService.GetConnectionIdByName("TestUser");
      Assert.Null(actual);
    }

    /// <summary>
    /// Тест поиска всех пользователей.
    /// </summary>
    [Test]
    public void GetAll()
    {
      var expected = DtoConverters.GetUsersDtos(new List<User> { users.JohnUser, users.ValeraUser, users.BorkaUser });
      var actual = userService.GetAll();
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест поиска результатов обсуждения по имени пользователя.
    /// </summary>
    [Test]
    public void GetByName()
    {
      var expected = users.BorkaUser;
      var actual = userService.GetByName(TestOwner);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Тест поиска соединения по имени пользователя.
    /// </summary>
    [Test]
    public void GetConnectionIdNyName()
    {
      var expected = "TestConnection";
      var actual = userService.GetConnectionIdByName("TestUser");
      Assert.AreEqual(expected, actual);
    }
  }
}