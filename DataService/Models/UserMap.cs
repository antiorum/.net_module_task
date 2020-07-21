using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;

namespace DataService.Models
{
  /// <summary>
  /// Настройка маппинга для сущности <see cref="User"/>.
  /// </summary>
  public class UserMap : ClassMap<User>
  {
    /// <summary>
    /// Конструктор-конфигуратор.
    /// </summary>
    public UserMap()
    {
      Table("Users");
      Id(x => x.Id).GeneratedBy.Identity();
      Map(x => x.Name);
      HasManyToMany(x => x.Rooms)
          .Cascade.All()
          .Table("UsersInRooms");
    }
  }
}
