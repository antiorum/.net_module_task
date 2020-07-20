using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataService;
using DataService.Models;

namespace UnitTests.TestRepositories
{
  public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
  {
    private static IList<T> innerVault;

    protected BaseRepository()
    {
      innerVault = new List<T>();
    }

    public virtual IEnumerable<T> GetAll()
    {
      return innerVault;
    }

    public virtual T Get(long id)
    {
      return innerVault.FirstOrDefault(t => t.Id == id);
    }

    public virtual void Create(T item)
    {
      if (item.Id == 0)
      {
        item.Id = innerVault.Last().Id + 1;
      }
      innerVault.Add(item);
    }

    public virtual void Update(T item)
    {
      Delete(item.Id);
      Create(item);
    }

    public virtual void Delete(long id)
    {
      innerVault.Remove(Get(id));
    }
  }
}
