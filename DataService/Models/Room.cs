using System;
using System.Collections.Generic;

namespace DataService.Models
{
  /// <summary>
  /// Класс сущности "комната".
  /// </summary>
  public class Room : BaseEntity
  {
    /// <summary>
    /// Конструктор комнаты.
    /// </summary>
    /// <param name="owner">Владелец комнаты.</param>
    /// <param name="password">Пароль для комнаты.</param>
    /// <param name="timerMinutes">Количество минут для обсуждения.</param>
    /// <param name="deck">Колода.</param>
    public Room(User owner, string password, int timerMinutes, Deck deck)
    {
      this.Owner = owner;
      Users = new HashSet<User>() { owner };
      this.Password = password;
      TimerMinutes = timerMinutes;
      this.Deck = deck;
    }

    /// <summary>
    /// Пустой конструктор без параметров, нужен для ОРМ.
    /// </summary>
    public Room()
    {
      Users = new HashSet<User>();
    }

    /// <summary>
    /// Пользователи, вошедшие в комнату.
    /// </summary>
    /// <value>Сет юзеров.</value>
    public virtual ISet<User> Users { get; set; }

    /// <summary>
    /// Пользователь - владелец комнаты.
    /// </summary>
    /// <value>Владелец.</value>
    public virtual User Owner { get; set; }

    /// <summary>
    /// Пароль для входа в комнату.
    /// </summary>
    /// <value>Строка с паролем.</value>
    public virtual string Password { get; set; }

    /// <summary>
    /// Количество минут, выделяемое на обсуждение в этой комнате.
    /// </summary>
    /// <value>Целочисленное число минут.</value>
    public virtual int TimerMinutes { get; set; }

    /// <summary>
    /// Колода, применяемая в этой комнате.
    /// </summary>
    /// <value>Колода.</value>
    public virtual Deck Deck { get; set; }

    /// <summary>
    /// Идентификатор текущего результата обсуждения.
    /// </summary>
    /// <value>Целочисленный ИД.</value>
    public virtual long CurrentDiscussionResultId { get; set; }

    /// <summary>
    /// Переопределение эквивалентности комнат.
    /// </summary>
    /// <param name="obj">Объект сравнения.</param>
    /// <returns>Результат проверки равенства.</returns>
    public override bool Equals(object obj)
    {
      return obj is Room room &&
             Id == room.Id &&
             EqualityComparer<User>.Default.Equals(Owner, room.Owner) &&
             Password == room.Password &&
             TimerMinutes == room.TimerMinutes;
    }

    /// <summary>
    /// Переопределение хэш-кода.
    /// </summary>
    /// <returns>Целочисленный хэш-код.</returns>
    public override int GetHashCode()
    {
      return HashCode.Combine(Id, Owner, Password, TimerMinutes);
    }
  }
}
