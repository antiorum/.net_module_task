using System;
using System.Collections.Generic;
using System.Linq;

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
    /// <param name="name">Название комнаты.</param>
    /// <param name="timerDuration">Количество минут для обсуждения.</param>
    /// <param name="deck">Колода.</param>
    public Room(User owner, string password, string name, TimeSpan timerDuration, Deck deck)
    {
      this.Owner = owner;
      Users = new HashSet<User>() { owner };
      this.Password = password;
      this.Name = name;
      TimerDuration = timerDuration;
      this.Deck = deck;
      this.DiscussionResults = new HashSet<DiscussionResult>();
    }

    /// <summary>
    /// Пустой конструктор без параметров, нужен для ОРМ.
    /// </summary>
    public Room()
    {
      Users = new HashSet<User>();
      this.DiscussionResults = new HashSet<DiscussionResult>();
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
    /// Название комнаты.
    /// </summary>
    /// <value>Строковое название.</value>
    public virtual string Name { get; set; }

    /// <summary>
    /// Количество времени, выделяемое на обсуждение в этой комнате.
    /// </summary>
    /// <value>Временной промежуток.</value>
    public virtual TimeSpan? TimerDuration { get; set; }

    /// <summary>
    /// Колода, применяемая в этой комнате.
    /// </summary>
    /// <value>Колода.</value>
    public virtual Deck Deck { get; set; }

    /// <summary>
    /// Результаты обсуждений, прошедших в комнате.
    /// </summary>
    /// <value>Коллекция результатов обсуждений.</value>
    public virtual ISet<DiscussionResult> DiscussionResults { get; set; }

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
             Name == room.Name &&
             TimerDuration == room.TimerDuration;
    }

    /// <summary>
    /// Переопределение хэш-кода.
    /// </summary>
    /// <returns>Целочисленный хэш-код.</returns>
    public override int GetHashCode()
    {
      return HashCode.Combine(Id, Owner, Password, Name, TimerDuration);
    }
  }
}
