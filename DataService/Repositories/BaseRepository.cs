using System.Collections.Generic;
using DataService.Models;

namespace DataService.Repositories
{
  /// <summary>
  /// Базовая реализация репозитория.
  /// </summary>
  /// <typeparam name="T">Параметр, унаследованный от <see cref="BaseEntity"/>.</typeparam>
  public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
  {
    /// <summary>
    /// Выбрать все элементы из базы данных.
    /// </summary>
    /// <returns>Типизированная коллекция.</returns>
    public IEnumerable<T> GetAll()
    {
      using (NHibernateConfig.GetConfig.Session.BeginTransaction())
      {
        return NHibernateConfig.GetConfig.Session.QueryOver<T>().Fetch(a => a).Eager.List();
      }
    }

    /// <summary>
    /// Выбрать элемент по идентификатору.
    /// </summary>
    /// <param name="id">Целочисленный ИД.</param>
    /// <returns>Элемент типа Т.</returns>
    public T Get(long id)
    {
      using (NHibernateConfig.GetConfig.Session.BeginTransaction())
      {
        return NHibernateConfig.GetConfig.Session.Get<T>(id);
      }
    }

    /// <summary>
    /// Сохранить в базе элемент Т.
    /// </summary>
    /// <param name="item">Объект сохранения.</param>
    public void Save(T item)
    {
      using (NHibernateConfig.GetConfig.Session.BeginTransaction())
      {
        NHibernateConfig.GetConfig.Session.Save(item);
        NHibernateConfig.GetConfig.Session.Transaction.Commit();
      }
    }

    /// <summary>
    /// Изменить элемент в базе.
    /// </summary>
    /// <param name="item">Элемент, который заместит предыдущий.</param>
    public void Update(T item)
    {
      using (NHibernateConfig.GetConfig.Session.BeginTransaction())
      {
        NHibernateConfig.GetConfig.Session.Merge(item);
        NHibernateConfig.GetConfig.Session.Transaction.Commit();
      }
    }

    /// <summary>
    /// Удалить элемент из базы.
    /// </summary>
    /// <param name="id">ИД элемента.</param>
    public void Delete(long id)
    {
      using (NHibernateConfig.GetConfig.Session.BeginTransaction())
      {
        NHibernateConfig.GetConfig.Session.Delete(NHibernateConfig.GetConfig.Session.Get<T>(id));
        NHibernateConfig.GetConfig.Session.Transaction.Commit();
      }
    }
  }
}
