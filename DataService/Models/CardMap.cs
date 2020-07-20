using FluentNHibernate.Mapping;

namespace DataService.Models
{
  /// <summary>
  /// Конфиг ОРМ маппинга для класса <see cref="Card"/>
  /// </summary>
  public class CardMap : ClassMap<Card>
  {
    /// <summary>
    /// Конструктор-конфигуратор.
    /// </summary>
    public CardMap()
    {
      Table("Cards");
      Id(x => x.Id).GeneratedBy.Identity();
      Map(x => x.Image);
      Map(x => x.Name);
      Map(x => x.Owner);
      Map(x => x.Value);
      HasManyToMany(x => x.Decks)
          .Cascade.All()
          .Table("CardsDecks");
    }
  }
}
