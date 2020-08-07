using System.Collections.Generic;
using DataService.Models;

namespace DataService.InitData
{
  /// <summary>
  /// Содержит тестовых пользователей.
  /// </summary>
  public class Users
  {
    /// <summary>
    /// Тестовый пользователь №1.
    /// </summary>
    /// <value>Сущность-пользователь.</value>
    public User JohnUser { get; } = new User { Id = 1, Name = "John Dou", Rooms = new HashSet<Room>() };

    /// <summary>
    /// Тестовый пользователь №2.
    /// </summary>
    /// <value>Сущность-пользователь.</value>
    public User ValeraUser { get; } = new User { Id = 2, Name = "Valera Kotov", Rooms = new HashSet<Room>() };
    
    /// <summary>
    /// Тестовый пользователь №3.
    /// </summary>
    /// <value>Сущность-пользователь.</value>
    public User BorkaUser { get; } = new User { Id = 3, Name = "Bor'ka", Rooms = new HashSet<Room>() };
  }
}
