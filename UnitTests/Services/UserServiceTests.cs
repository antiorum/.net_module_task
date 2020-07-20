using System.Collections.Generic;
using DataService.Models;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NUnit.Framework;
using ScrumPokerWeb.DTO;
using UnitTests.TestData;

namespace UnitTests.Services
{
  [TestFixture()]
  public class UserServiceTests : BaseTest
  {
    [Test()]
    public void GetByName()
    {
      User expected = users.BorkaUser;
      User actual =  userService.GetByName(TestOwner);
      Assert.AreEqual(expected, actual);
    }

    [Test()]
    public void Create()
    {
      userService.Create(users.UserForInsert);
      User expected = users.UserForInsert;
      User actual = userService.GetByName(expected.Name);
      Assert.AreEqual(expected, actual);
    }

    [Test()]
    public void GetAll()
    {
      var expected = DtoUtil.GetUsersTOs(new List<User>() {users.JohnUser, users.ValeraUser, users.BorkaUser});
      var actual = userService.GetAll();
      Assert.AreEqual(expected, actual);
    }

    [Test()]
    public void AddUserToConnectionMap()
    {
      userService.AddUserToConnectionMap(TestOwner, "testID");
      var expected = "testID";
      var actual = userService.GetConnectionIdNyName(TestOwner);
      Assert.AreEqual(expected, actual);
    }

    [Test()]
    public void DeleteUserFromConnectionMap()
    {
      userService.DeleteUserFromConnectionMap("TestUser");
      string actual = userService.GetConnectionIdNyName("TestUser");
      Assert.Null(actual);
    }

    [Test()]
    public void GetConnectionIdNyName()
    {
      var expected = "TestConnection";
      var actual = userService.GetConnectionIdNyName("TestUser");
      Assert.AreEqual(expected, actual);
    }
  }
}