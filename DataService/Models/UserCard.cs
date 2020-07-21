using System;
using System.Collections.Generic;

namespace DataService.Models
{
  /// <summary>
  /// Класс, обозначающий оценку пользователя.
  /// </summary>
  public class UserCard : BaseEntity
  {
    /// <summary>
    /// Пользователь, поставивщий оценку.
    /// </summary>
    /// <value>См. <see cref="User"/></value>
    public virtual User User { get; set; }

    /// <summary>
    /// Карта, показанная пользователем.
    /// </summary>
    /// <value>См. <see cref="Card"/></value>
    public virtual Card Card { get; set; }

    /// <summary>
    /// Результат дискуссии, в ходе которой была показана оценка.
    /// </summary>
    /// <value>См. <see cref="DiscussionResult"/></value>
    public virtual DiscussionResult DiscussionResult { get; set; }

    /// <summary>
    /// Переопределение для эквивалентности.
    /// </summary>
    /// <param name="obj">Объект сравнения.</param>
    /// <returns>Равны ли оценки.</returns>
    public override bool Equals(object obj)
    {
      return obj is UserCard card &&
             Id == card.Id &&
             EqualityComparer<User>.Default.Equals(User, card.User) &&
             EqualityComparer<Card>.Default.Equals(Card, card.Card) &&
             EqualityComparer<DiscussionResult>.Default.Equals(DiscussionResult, card.DiscussionResult);
    }

    /// <summary>
    /// Переопределение хэш-функции.
    /// </summary>
    /// <returns>Целочисленный хэш-код.</returns>
    public override int GetHashCode()
    {
      return HashCode.Combine(Id, User, Card, DiscussionResult);
    }
  }
}
