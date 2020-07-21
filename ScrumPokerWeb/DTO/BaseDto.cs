namespace ScrumPokerWeb.DTO
{
  /// <summary>
  /// Базовый класс ДТО.
  /// </summary>
  public class BaseDto
  {
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="id">ИД ДТО.</param>
    public BaseDto(long id)
    {
      this.Id = id;
    }

    /// <summary>
    /// ИД ДТО.
    /// </summary>
    /// <value>Целочисленный ИД.</value>
    public virtual long Id { get; set; }
  }
}