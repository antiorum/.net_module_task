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
      References(x => x.Owner).Column("Owner_id");
      References(x => x.Deck).Column("Deck_id");
      HasMany(x => x.DiscussionResults);
      HasManyToMany(x => x.Users)
          .Cascade.SaveUpdate()
          .Inverse()
          .Table("UsersInRooms");
    }
  }
}
