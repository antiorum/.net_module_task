using System;
using System.Collections.Generic;
using DataService.Models;

namespace DataService.InitData
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
      Name = "TestRoom1",
      TimerDuration = TimeSpan.Parse("0:00:00:10"),
      Password = string.Empty
    };

    /// <summary>
    /// Тестовая комната №2.
    /// </summary>
    /// <value>Сущность-комната.</value>
    public Room RoomToInsert { get; } = new Room()
    {
      Id = 2,
      Name = "RoomToInsert",
      TimerDuration = TimeSpan.Parse("10"),
      Password = "pass",
    };

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    public Rooms()
    {
      Decks decks = new Decks();
      Users users = new Users();
      DiscussionResults results = new DiscussionResults();

      this.TestRoom1.Owner = users.BorkaUser;
      this.TestRoom1.Deck = decks.BaseDeck;
      this.TestRoom1.Users = new HashSet<User>() { users.BorkaUser, users.JohnUser };
      this.TestRoom1.DiscussionResults = new HashSet<DiscussionResult>() { results.TestDiscussionResult1 };

      this.RoomToInsert.Owner = users.BorkaUser;
      this.RoomToInsert.Deck = decks.BaseDeck;
      this.RoomToInsert.Users = new HashSet<User>() { users.BorkaUser };
      this.RoomToInsert.DiscussionResults = new HashSet<DiscussionResult>();
    }
  }
}
