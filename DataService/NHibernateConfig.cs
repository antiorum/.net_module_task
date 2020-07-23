using DataService.Models;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;

namespace DataService
{
  /// <summary>
  /// Конфигурирует NHibernate и создаёт сессию.
  /// </summary>
  public class NHibernateConfig
  {
    /// <summary>
    /// Конфигурация NHibernate.
    /// </summary>
    private Configuration configuration;

    /// <summary>
    /// Фабрика сессий NHibernate.
    /// </summary>
    private ISessionFactory factory;

    /// <summary>
    /// Сессия NHibernate.
    /// </summary>
    /// <value>Объект, реализующий ISession.</value>
    public ISession Session { get; }

    private NHibernateConfig()
    {
      this.configuration = Fluently.Configure().Database(
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
      this.factory = this.configuration.BuildSessionFactory();
      this.Session = this.factory.OpenSession();
    }

    /// <summary>
    /// Получить конфигурацию или создать, если еще не создана.
    /// </summary>
    /// <value>Объект текущего класса.</value>
    public static NHibernateConfig GetConfig { get; } = new NHibernateConfig();
  }
}
