using System;
using System.Collections.Generic;
using System.Text;
using DataService.Models;

namespace UnitTests.TestData
{
  public class Rooms
  {
    public Room TestRoom1 = new Room()
    {
      Id = 1,
      CurrentDiscussionResultId = 0,
      TimerMinutes = 10,
    };

    public Room RoomToInsert = new Room()
    {
      Id = 2,
      CurrentDiscussionResultId = 0,
      TimerMinutes = 10,
      Password = "pass",
    };

    public Rooms()
    {
      Decks decks = new Decks();
      Users users = new Users();

      TestRoom1.Owner = users.BorkaUser;
      TestRoom1.Deck = decks.BaseDeck;
      TestRoom1.Users = new HashSet<User>(){users.BorkaUser, users.JohnUser};

      RoomToInsert.Owner = users.BorkaUser;
      RoomToInsert.Deck = decks.BaseDeck;
      RoomToInsert.Users = new HashSet<User>(){users.BorkaUser};
    }
  }
}
