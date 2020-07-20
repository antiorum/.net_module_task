namespace DataService.Models
{
  /// <summary>
  /// Класс базовой сущности.
  /// </summary>
  public class BaseEntity
  {
    /// <summary>
    /// Получает или задает идентификатор сущности.
    /// </summary>
    /// <value>Значение ИД.</value>
    public virtual long Id { get; set; }
  }
}
