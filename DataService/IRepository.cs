using System.Collections.Generic;
using DataService.Models;

namespace DataService
{
  /// <summary>
  /// Интерфейс репозитория.
  /// </summary>
  /// <typeparam name="T">Хранящиеся сущности должны быть унаследованы от <see cref="BaseEntity"/>.</typeparam>
  public interface IRepository<T> where T : BaseEntity
  {
    /// <summary>
    /// Выбрать все элементы.
    /// </summary>
    /// <returns>Коллекцию элементов.</returns>
    IEnumerable<T> GetAll();

    /// <summary>
    /// Выбрать элемент по идентификатору.
    /// </summary>
    /// <param name="id">Целочисленный ИД.</param>
    /// <returns>Элемент типа Т.</returns>
    T Get(long id);

    /// <summary>
    /// Сохранить в базе элемент Т.
    /// </summary>
    /// <param name="item">Объект сохранения.</param>
    void Save(T item);

    /// <summary>
    /// Изменить элемент.
    /// </summary>
    /// <param name="item">Элемент, который заместит предыдущий.</param>
    void Update(T item);

    /// <summary>
    /// Удалить элемент из базы.
    /// </summary>
    /// <param name="id">ИД элемента.</param>
    void Delete(long id);
  }
}
