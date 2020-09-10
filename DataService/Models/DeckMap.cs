using FluentNHibernate.Mapping;

namespace DataService.Models
{
  /// <summary>
  /// Настройка маппинга для колод.
  /// </summary>
  public class DeckMap : ClassMap<Deck>
  {
    /// <summary>
    /// Конструктор-конфигуратор для колод.
    /// </summary>
    public DeckMap()
    {
      Table("Decks");
      Id(x => x.Id).GeneratedBy.Identity();
      Map(x => x.Name);
      Map(x => x.Owner);
      HasManyToMany(x => x.Cards)
          .Cascade.SaveUpdate()
          .Inverse()
          .Table("CardsDecks").Not.LazyLoad();
    }
  }
}
