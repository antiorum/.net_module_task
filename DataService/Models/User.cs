using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Models
{
  /// <summary>
  /// Сущность - пользователь.
  /// </summary>
  public class User : BaseEntity
  {
    /// <summary>
    /// Пустой конструктор, нужен для ОРМ.
    /// </summary>
    public User()
    {
      Rooms = new HashSet<Room>();
    }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    /// <value>Имя в виде строки.</value>
    public virtual string Name { get; set; }

    /// <summary>
    /// Комнаты, в которые вошел пользователь.
    /// </summary>
    /// <value>Коллекция комнат.</value>
    public virtual ISet<Room> Rooms { get; set; }

    /// <summary>
    /// Переопределение для эквивалентности пользователей.
    /// </summary>
    /// <param name="obj">Объект сравнения.</param>
    /// <returns>Равенство с текущим пользователем.</returns>
    public override bool Equals(object obj)
    {
      return obj is User user &&
             Id == user.Id &&
             Name == user.Name;
    }

    /// <summary>
    /// Переопределение для хэш-функции.
    /// </summary>
    /// <returns>Целочисленный хэш-код.</returns>
    public override int GetHashCode()
    {
      return HashCode.Combine(Id, Name);
    }
  }
}
