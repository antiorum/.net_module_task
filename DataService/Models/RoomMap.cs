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
      Map(x => x.TimerMinutes);
      Map(x => x.CurrentDiscussionResultId);
      References(x => x.Owner).Column("Owner_id");
      References(x => x.Deck).Column("Deck_id");
      HasManyToMany(x => x.Users)
          .Cascade.SaveUpdate()
          .Inverse()
          .Table("UsersInRooms");
    }
  }
}
