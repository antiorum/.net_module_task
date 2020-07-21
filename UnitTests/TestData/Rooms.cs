using System.Collections.Generic;
using DataService.Models;

namespace UnitTests.TestData
{
  /// <summary>
  /// Содержит тестовые данные комнат.
  /// </summary>
  public class Rooms
  {
    /// <summary>
    /// Тестовая комната №1.
    /// </summary>
    /// <value>Сущность-комната.</value>
    public Room TestRoom1 { get; } = new Room()
    {
      Id = 1,
      CurrentDiscussionResultId = 0,
      TimerMinutes = 10,
    };

    /// <summary>
    /// Тестовая комната №2.
    /// </summary>
    /// <value>Сущность-комната.</value>
    public Room RoomToInsert { get; } = new Room()
    {
      Id = 2,
      CurrentDiscussionResultId = 0,
      TimerMinutes = 10,
      Password = "pass",
    };

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    public Rooms()
    {
      Decks decks = new Decks();
      Users users = new Users();

      this.TestRoom1.Owner = users.BorkaUser;
      this.TestRoom1.Deck = decks.BaseDeck;
      this.TestRoom1.Users = new HashSet<User>() { users.BorkaUser, users.JohnUser };

      this.RoomToInsert.Owner = users.BorkaUser;
      this.RoomToInsert.Deck = decks.BaseDeck;
      this.RoomToInsert.Users = new HashSet<User>() { users.BorkaUser };
    }
  }
}
