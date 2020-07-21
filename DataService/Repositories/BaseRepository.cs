using System.Collections.Generic;
using DataService.Models;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;

namespace DataService.Repositories
{
  /// <summary>
  /// Базовая реализация репозитория.
  /// </summary>
  /// <typeparam name="T">Параметр, унаследованный от <see cref="BaseEntity"/>.</typeparam>
  public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
  {
    /// <summary>
    /// Конфигурация NHibernate.
    /// </summary>
    protected static Configuration configuration;

    /// <summary>
    /// Фабрика сессий NHibernate.
    /// </summary>
    protected static ISessionFactory factory;

    /// <summary>
    /// Сессия NHibernate.
    /// </summary>
    protected static ISession session;

    /// <summary>
    /// Статический конструктор с конфигурацией.
    /// </summary>
    static BaseRepository()
    {
      configuration = Fluently.Configure().Database(
              MsSqlConfiguration
                  .MsSql2008
                  .ShowSql()
                  .ConnectionString("MultipleActiveResultSets=False; Data Source=.\\SQLEXPRESS; Initial Catalog=PokerDB; Integrated Security=True")
                  .UseReflectionOptimizer())
          .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CardMap>())
          .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DeckMap>())
          .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DiscussionResultMap>())
          .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
          .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserCardMap>())
          .Mappings(m => m.FluentMappings.AddFromAssemblyOf<RoomMap>())
          .BuildConfiguration();

      // new SchemaExport(configuration).Create(true, true);
      factory = configuration.BuildSessionFactory();
      session = factory.OpenSession();
    }

    /// <summary>
    /// Выбрать все элементы из базы данных.
    /// </summary>
    /// <returns>Типизированная коллекция.</returns>
    public IEnumerable<T> GetAll()
    {
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
      using (session.BeginTransaction())
      {
        return session.Get<T>(id);
      }
    }

    /// <summary>
    /// Сохранить в базе элемент Т.
    /// </summary>
    /// <param name="item">Объект сохранения.</param>
    public void Create(T item)
    {
      using (session.BeginTransaction())
      {
        session.Save(item);
        session.Transaction.Commit();
      }
    }

    /// <summary>
    /// Изменить элемент в базе.
    /// </summary>
    /// <param name="item">Элемент, который заместит предыдущий.</param>
    public void Update(T item)
    {
      using (session.BeginTransaction())
      {
        session.Merge(item);
        session.Transaction.Commit();
      }
    }

    /// <summary>
    /// Удалить элемент из базы.
    /// </summary>
    /// <param name="id">ИД элемента.</param>
    public void Delete(long id)
    {
      using (session.BeginTransaction())
      {
        session.Delete(session.Get<T>(id));
        session.Transaction.Commit();
      }
    }
  }
}
