using FluentNHibernate.Mapping;

namespace DataService.Models
{
  /// <summary>
  /// Настройка маппинга для комнат.
  /// </summary>
  public class RoomMap : ClassMap<Room>
  {
    /// <summary>
    /// Конструктор-конфигуратор.
    /// </summary>
    public RoomMap()
    {
      Table("Rooms");
      Id(x => x.Id).GeneratedBy.Identity();
      Map(x => x.Password);
      Map(x => x.Name);
      Map(x => x.TimerDuration);
      References(x => x.Owner).Column("Owner_id").Not.LazyLoad();
      References(x => x.Deck).Column("Deck_id").Not.LazyLoad();
      HasMany(x => x.DiscussionResults).Not.LazyLoad();
      HasManyToMany(x => x.Users)
          .Cascade.SaveUpdate()
          .Inverse()
          .Table("UsersInRooms").Not.LazyLoad();
    }
  }
}
