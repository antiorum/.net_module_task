using System;
using System.Collections.Generic;
using System.Text;
using DataService.Models;
using NHibernate;

namespace DataService
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T Get(long id);
        void Create(T item);
        void Update(T item);
        void Delete(long id);        
    }
}
