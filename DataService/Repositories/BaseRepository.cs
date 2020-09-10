using System.Collections.Generic;
using DataService.Models;
using NHibernate;

namespace DataService.Repositories
{
  /// <summary>
  /// Базовая реализация репозитория.
  /// </summary>
  /// <typeparam name="T">Параметр, унаследованный от <see cref="BaseEntity"/>.</typeparam>
  public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
  {
    private ISessionFactory factory = NHibernateConfig.GetConfig.Factory;

    /// <summary>
    /// Выбрать все элементы из базы данных.
    /// </summary>
    /// <returns>Типизированная коллекция.</returns>
    public IEnumerable<T> GetAll()
    {
      using var session = factory.OpenSession();
      using (session.BeginTransaction())
      {
        return session.QueryOver<T>().Fetch(a => a).Eager.List();
      }
    }

    /// <summary>
    /// Выбрать элемент по идентификатору.
    /// </summary>
    /// <param name="id">Целочисленный ИД.</param>
    /// <returns>Элемент типа Т.</returns>
    public T Get(long id)
    {
      using var session = factory.OpenSession();
      using (session.BeginTransaction())
      {
        return session.Get<T>(id, LockMode.Force);
      }
    }

    /// <summary>
    /// Сохранить в базе элемент Т.
    /// </summary>
    /// <param name="item">Объект сохранения.</param>
    public void Save(T item)
    {
      using var session = factory.OpenSession();
      using (session.BeginTransaction())
      {
        session.Save(item);
        session.Transaction.Commit();
        session.Flush();
      }
      
    }

    /// <summary>
    /// Изменить элемент в базе.
    /// </summary>
    /// <param name="item">Элемент, который заместит предыдущий.</param>
    public void Update(T item)
    {
      using var session = factory.OpenSession();
      using (session.BeginTransaction())
      {
        session.Merge(item);
        session.Transaction.Commit();
        session.Flush();
      }
      
    }

    /// <summary>
    /// Удалить элемент из базы.
    /// </summary>
    /// <param name="id">ИД элемента.</param>
    public void Delete(long id)
    {
      using var session = factory.OpenSession();
      using (session.BeginTransaction())
      {
        session.Delete(session.Get<T>(id));
        session.Transaction.Commit();
        session.Flush();
      }
      
    }
  }
}
