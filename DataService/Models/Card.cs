using System;
using System.Collections.Generic;

namespace DataService.Models
{
  /// <summary>
  /// Класс сущности - карты.
  /// </summary>
  public class Card : BaseEntity
  {
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Card"/>.
    /// Требуется для корректной работы ОРМ.
    /// </summary>
    public Card()
    {
    }

    /// <summary>
    /// Получает или задает имя владельца карты.
    /// Равняется null если карта общего использования.
    /// </summary>
    /// <value>Имя владельца.</value>
    public virtual string Owner { get; set; }

    /// <summary>
    /// Название карты.
    /// </summary>
    /// <value>Название.</value>
    public virtual string Name { get; set; }

    /// <summary>
    /// Значение карты. Может быть null для некоторых карт.
    /// </summary>
    /// <value>Значение.</value>
    public virtual int? Value { get; set; }

    /// <summary>
    /// Путь к файлу-изображению карты.
    /// </summary>
    /// <value>Путь.</value>
    public virtual string Image { get; set; }

    /// <summary>
    /// Колоды, в которые входит эта карта.
    /// </summary>
    /// <value>Набор колод.</value>
    public virtual ISet<Deck> Decks { get; set; }

    /// <summary>
    /// Переопределение для эквивалентности карт.
    /// </summary>
    /// <param name="obj">Сравниваемый объект.</param>
    /// <returns>Равны ли объекты.</returns>
    public override bool Equals(object obj)
    {
      return obj is Card card &&
             Id == card.Id &&
             Owner == card.Owner &&
             Name == card.Name &&
             Value == card.Value &&
             Image == card.Image;
    }

    /// <summary>
    /// Переопределение для хэш-кода карты.
    /// </summary>
    /// <returns>Целочисленный хэш-код.</returns>
    public override int GetHashCode()
    {
      return HashCode.Combine(Id, Owner, Name, Value, Image);
    }
  }
}
