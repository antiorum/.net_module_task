using System;
using System.Collections.Generic;

namespace DataService.Models
{
  /// <summary>
  /// Сущность - результат обсуждения.
  /// </summary>
  public class DiscussionResult : BaseEntity
  {
    /// <summary>
    /// Конструктор без параметров, нужен для работы ОРМ.
    /// </summary>
    public DiscussionResult()
    {
      UsersCards = new HashSet<UserCard>();
    }

    /// <summary>
    /// Дата начала обсуждения.
    /// </summary>
    /// <value>Объект даты-времени.</value>
    public virtual DateTime Beginning { get; set; }

    /// <summary>
    /// Дата конца обсуждения.
    /// </summary>
    /// <value>Объект даты-времени.</value>
    public virtual DateTime Ending { get; set; }

    /// <summary>
    /// Тема обсуждения.
    /// </summary>
    /// <value>Строка.</value>
    public virtual string Theme { get; set; }

    /// <summary>
    /// Итог обсуждения или комментарий к нему.
    /// </summary>
    /// <value>Строка.</value>
    public virtual string Resume { get; set; }

    /// <summary>
    /// Карты, показанные участниками.
    /// </summary>
    /// <value>Набор показанных карт.</value>
    public virtual ISet<UserCard> UsersCards { get; set; }

    /// <summary>
    /// Переопределение для эквивалентности.
    /// </summary>
    /// <param name="obj">Объект сравнения.</param>
    /// <returns>Равны ли результаты.</returns>
    public override bool Equals(object obj)
    {
      return obj is DiscussionResult result &&
             Id == result.Id &&
             Beginning == result.Beginning &&
             Ending == result.Ending &&
             Theme == result.Theme &&
             Resume == result.Resume;
    }

    /// <summary>
    /// Переопределение для хэш-кода.
    /// </summary>
    /// <returns>Целочисленный хэш-код.</returns>
    public override int GetHashCode()
    {
      return HashCode.Combine(Id, Beginning, Ending, Theme, Resume);
    }
  }
}
