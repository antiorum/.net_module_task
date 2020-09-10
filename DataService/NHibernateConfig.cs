using System;
using DataService.Models;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Configuration = NHibernate.Cfg.Configuration;

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
    public ISessionFactory Factory;

    private NHibernateConfig()
    {
      IConfigurationRoot conf = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .Build();

      this.configuration = Fluently.Configure().Database(
          MsSqlConfiguration
            .MsSql2008
            .ShowSql()
            .ConnectionString(conf.GetConnectionString("DefaultConnection"))
            .UseReflectionOptimizer())
        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CardMap>())
        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DeckMap>())
        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DiscussionResultMap>())
        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserCardMap>())
        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<RoomMap>())
        .BuildConfiguration();

      // new SchemaExport(configuration).Drop(true, true);
      // new SchemaExport(configuration).Create(true, true);
      this.Factory = this.configuration.BuildSessionFactory();
    }

    /// <summary>
    /// Получить конфигурацию или создать, если еще не создана.
    /// </summary>
    /// <value>Объект текущего класса.</value>
    public static NHibernateConfig GetConfig { get; } = new NHibernateConfig();
  }
}
