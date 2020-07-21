using System;
using System.Collections.Generic;

namespace ScrumPokerWeb.DTO
{
  /// <summary>
  /// Отображение для колод.
  /// </summary>
  public class DeckDto : BaseDto
  {
    /// <summary>
    /// Конструктор отображения колоды.
    /// </summary>
    /// <param name="isCommon">Общая или частная.</param>
    /// <param name="name">Название.</param>
    /// <param name="cards">Карты в колоде.</param>
    /// <param name="id">ИД колоды.</param>
    public DeckDto(bool isCommon, string name, ISet<CardDto> cards, long id) : base(id)
    {
      IsCommon = isCommon;
      Name = name;
      Cards = cards;
    }

    /// <summary>
    /// Общая ли колода.
    /// </summary>
    /// <value>Правда если колода общая.</value>
    public virtual bool IsCommon { get; }

    /// <summary>
    /// Название колоды.
    /// </summary>
    /// <value>Строка с названием.</value>
    public virtual string Name { get; }

    /// <summary>
    /// Карты в колоде.
    /// </summary>
    /// <value>Коллекция ДТО карт.</value>
    public virtual ISet<CardDto> Cards { get; }

    /// <summary>
    /// Переопределение эквивалентности для объектов одного класса.
    /// </summary>
    /// <param name="other">Объект для сравнения.</param>
    /// <returns>Одинаковы ли объекты.</returns>
    protected bool Equals(DeckDto other)
    {
      return IsCommon == other.IsCommon && Name == other.Name;
    }

    /// <summary>
    /// Переопределение эквивалентности для.
    /// </summary>
    /// <param name="obj">Объект для сравнения.</param>
    /// <returns>Одинаковы ли объекты.</returns>
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((DeckDto)obj);
    }

    /// <summary>
    /// Переопределение хэш-функции.
    /// </summary>
    /// <returns>Целочисленный хэш-код.</returns>
    public override int GetHashCode()
    {
      return HashCode.Combine(IsCommon, Name);
    }
  }
}