using System;
using System.Collections.Generic;
using System.Text;
using DataService.Models;

namespace UnitTests.TestData
{
  public class DiscussionResults
  {
    public  UserCard Mark1 = new UserCard()
    {
      Id = 1,
      DiscussionResult = null,
    };

    public  UserCard Mark2 = new UserCard()
    {
      Id = 2,
      DiscussionResult = null,
    };
    public  UserCard Mark3 = new UserCard()
    {
      Id = 3,
      DiscussionResult = null,
    };

    public DiscussionResult TestDiscussionResult1 = new DiscussionResult()
    {
      Id = 1,
      Beginning = DateTime.MinValue,
      Ending = DateTime.MaxValue,
      Resume = "resume",
      Theme = "theme",
    };

    public DiscussionResult TestDiscussionResult2 = new DiscussionResult()
    {
      Id = 2,
      Beginning = new DateTime(2020, 1 , 1, 1, 0, 0),
      Ending = new DateTime(2020, 1, 1, 1, 30, 0),
      Resume = "resume",
      Theme = "theme",
    };

    public DiscussionResults()
    {
      Cards cards = new Cards();
      Users users = new Users();
      Mark1.Card = cards.CardOne;
      Mark1.User = users.JohnUser;
      Mark2.Card = cards.CardFive;
      Mark2.User = users.ValeraUser;
      Mark3.Card = cards.CardCoffee;
      Mark3.User = users.BorkaUser;

      TestDiscussionResult1.UsersCards = new HashSet<UserCard>() {this.Mark1, this.Mark2, this.Mark3};
      TestDiscussionResult2.UsersCards = new HashSet<UserCard>() {this.Mark1, this.Mark3};
    }
  }
}
