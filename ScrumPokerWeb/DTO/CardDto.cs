using System;

namespace ScrumPokerWeb.DTO
{
  /// <summary>
  /// Отображение для Карты.
  /// </summary>
  public class CardDto : BaseDto
  {
    /// <summary>
    /// Конструктор с параметрами.
    /// </summary>
    /// <param name="isCommon">Общая ли карта.</param>
    /// <param name="name">Название карты.</param>
    /// <param name="value">Значение карты.</param>
    /// <param name="image">Картинка для карты.</param>
    /// <param name="id">ИД карты.</param>
    public CardDto(bool isCommon, string name, int? value, string image, long id) : base(id)
    {
      IsCommon = isCommon;
      Name = name;
      Value = value;
      Image = image;
    }

    /// <summary>
    /// Общая ли карта.
    /// </summary>
    /// <value>True если общая.</value>
    public virtual bool IsCommon { get; }

    /// <summary>
    /// Название карты.
    /// </summary>
    /// <value>Строка с названием.</value>
    public virtual string Name { get; }

    /// <summary>
    /// Значение карты.
    /// </summary>
    /// <value>Целочисленное значение.</value>
    public virtual int? Value { get; }

    /// <summary>
    /// Картинка для карты.
    /// </summary>
    /// <value>Путь к картинке.</value>
    public virtual string Image { get; }

    /// <summary>
    /// Переопредление для эквивалентности объектов одного класса.
    /// </summary>
    /// <param name="other">Объект сравнения.</param>
    /// <returns>Равенство объектов.</returns>
    protected bool Equals(CardDto other)
    {
      return IsCommon == other.IsCommon && Name == other.Name && Value == other.Value && Image == other.Image;
    }

    /// <summary>
    /// Переопредление для эквивалентности объектов.
    /// </summary>
    /// <param name="obj">Объект сравнения.</param>
    /// <returns>Равенство объектов.</returns>
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((CardDto)obj);
    }

    /// <summary>
    /// Переопределение для хэш-функции.
    /// </summary>
    /// <returns>Целочисленный хэш-код.</returns>
    public override int GetHashCode()
    {
      return HashCode.Combine(IsCommon, Name, Value, Image);
    }
  }
}