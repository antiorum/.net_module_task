using System.Collections.Generic;
using System.Linq;
using DataService.Models;

namespace DataService.InMemoryRepositories
{
  /// <summary>
  /// Тестовый базовый репозиторий.
  /// </summary>
  /// <typeparam name="T">Сущность, наследуемая от <see cref="BaseEntity"/></typeparam>
  public abstract class BaseInMemoryRepository<T> : IRepository<T> where T : BaseEntity
  {
    private static IList<T> innerVault;

    /// <summary>
    /// Конструктор репозитория.
    /// </summary>
    protected BaseInMemoryRepository()
    {
      innerVault = new List<T>();
    }

    /// <summary>
    /// Все элементы репозитория.
    /// </summary>
    /// <returns>Коллекция элементов.</returns>
    public virtual IEnumerable<T> GetAll()
    {
      return innerVault;
    }

    /// <summary>
    /// Элемент по ИД.
    /// </summary>
    /// <param name="id">ИД элемента.</param>
    /// <returns>Экземпляр Т.</returns>
    public virtual T Get(long id)
    {
      return innerVault.FirstOrDefault(t => t.Id == id);
    }

    /// <summary>
    /// Создание нового элемента.
    /// </summary>
    /// <param name="item">Экземпляр Т, который будет сохранен.</param>
    public virtual void Save(T item)
    {
      if (item.Id == 0)
      {
        item.Id = innerVault.Last().Id + 1;
      }
      innerVault.Add(item);
    }

    /// <summary>
    /// Изменение элмента.
    /// </summary>
    /// <param name="item">Экземпляр Т, который будет изменен.</param>
    public virtual void Update(T item)
    {
      this.Delete(item.Id);
      this.Save(item);
    }

    /// <summary>
    /// Удалить элемент.
    /// </summary>
    /// <param name="id">ИД элемента.</param>
    public virtual void Delete(long id)
    {
      innerVault.Remove(this.Get(id));
    }
  }
}
