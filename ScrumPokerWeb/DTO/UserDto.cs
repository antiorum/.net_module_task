namespace ScrumPokerWeb.DTO
{
  /// <summary>
  /// Отображение пользователя.
  /// </summary>
  public class UserDto : BaseDto
  {
    /// <summary>
    /// Конструктор ДТО.
    /// </summary>
    /// <param name="name">Имя пользователя.</param>
    /// <param name="id">ИД пользователя.</param>
    public UserDto(string name, long id) : base(id)
    {
      Name = name;
    }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    /// <value>Строка с именем.</value>
    public string Name { get; }

    /// <summary>
    /// Переопределение эквивалентности для объектов одного класса.
    /// </summary>
    /// <param name="other">Объект для сравнения.</param>
    /// <returns>Равны ли объекты.</returns>
    protected bool Equals(UserDto other)
    {
      return Name == other.Name;
    }

    /// <summary>
    /// Переопределение эквивалентности.
    /// </summary>
    /// <param name="obj">Объект для сравнения.</param>
    /// <returns>Одинаковы ли объекты.</returns>
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((UserDto)obj);
    }

    /// <summary>
    /// Переопределение хэш-функции.
    /// </summary>
    /// <returns>Целочисленный хэш-код.</returns>
    public override int GetHashCode()
    {
      return Name != null ? Name.GetHashCode() : 0;
    }
  }
}
