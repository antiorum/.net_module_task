﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;

namespace DataService.Models
{
  /// <summary>
  /// Настройка маппинга для сущности <see cref="UserCard"/>.
  /// </summary>
  public class UserCardMap : ClassMap<UserCard>
  {
    /// <summary>
    /// Конструктор-конфигуратор.
    /// </summary>
    public UserCardMap()
    {
      Table("UsersCards");
      Id(x => x.Id).GeneratedBy.Identity();
      References(x => x.User).Column("User_id");
      References(x => x.Card).Column("Card_id");
      References(x => x.DiscussionResult).Cascade.SaveUpdate();
    }
  }
}
