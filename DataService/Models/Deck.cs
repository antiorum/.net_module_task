using System;
using System.Collections.Generic;

namespace DataService.Models
{
  /// <summary>
  /// Сущность - колода карт.
  /// </summary>
  public class Deck : BaseEntity
  {
    /// <summary>
    /// Конструктор без параметров, нужен для ОРМ.
    /// </summary>
    public Deck()
    {
      Cards = new HashSet<Card>();
    }

    /// <summary>
    /// Имя владельца колоды.
    /// Может быть null, если колода общего использования.
    /// </summary>
    /// <value>Строка с именем владельца.</value>
    public virtual string Owner { get; set; }

    /// <summary>
    /// Название колоды.
    /// </summary>
    /// <value>Строка с названием.</value>
    public virtual string Name { get; set; }

    /// <summary>
    /// Карты, которые входят в данную колоду.
    /// </summary>
    /// <value>Сет из карт.</value>
    public virtual ISet<Card> Cards { get; set; }

    /// <summary>
    /// Переопределение для эквивалентности колод.
    /// </summary>
    /// <param name="obj">Объект сравнения.</param>
    /// <returns>Равны ли колоды.</returns>
    public override bool Equals(object obj)
    {
      return obj is Deck deck &&
             Id == deck.Id &&
             Owner == deck.Owner &&
             Name == deck.Name;
    }

    /// <summary>
    /// Переопределение для хэш-кода.
    /// </summary>
    /// <returns>Целочисленный хэш-код колоды.</returns>
    public override int GetHashCode()
    {
      return HashCode.Combine(Id, Owner, Name);
    }
  }
}
