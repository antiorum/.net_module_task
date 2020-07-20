using System.Collections.Generic;
using DataService.Models;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace DataService.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T: BaseEntity
    {
        protected Configuration configuration;
        protected ISessionFactory factory;
        private ISession session;

        protected BaseRepository()
        {
            configuration = Fluently.Configure().
                Database(
                    MsSqlConfiguration
                        .MsSql2008
                        .ShowSql()
                        .ConnectionString(x => x
                            .Server(@".\SQLEXPRESS")
                            .Database("PokerDB")
                            .TrustedConnection())
                            .UseReflectionOptimizer())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CardMap>())
                .Mappings(m=> m.FluentMappings.AddFromAssemblyOf<DeckMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DiscussionResultMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserCardMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<RoomMap>())
                .BuildConfiguration();


            //new SchemaExport(configuration).Create(true, true);
            factory = configuration.BuildSessionFactory();
            session = factory.OpenSession();
        }

        public IEnumerable<T> GetAll()
        {
            using (session.BeginTransaction())
            {
                return session.QueryOver<T>().List();
            }
        }

        public T Get(long id)
        {
            using (session.BeginTransaction())
            {
                return session.Get<T>(id);
            }
        }

        public void Create(T item)
        {
            using (session.BeginTransaction())
            {
                session.Save(item);
                session.Transaction.Commit();
            }
        }

        public void Update(T item)
        {
            using (session.BeginTransaction())
            {
                session.Merge(item);
                session.Transaction.Commit();
            }
        }

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
