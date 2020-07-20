using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;

namespace DataService.Models
{
    class UserMap : ClassMap<User>
    {
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
