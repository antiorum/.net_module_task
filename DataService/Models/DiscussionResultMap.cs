using FluentNHibernate.Mapping;

namespace DataService.Models
{
  /// <summary>
  /// Настройка маппинга для результатов дискуссий.
  /// </summary>
  public class DiscussionResultMap : ClassMap<DiscussionResult>
  {
    /// <summary>
    /// Конструктор-конфигуратор.
    /// </summary>
    public DiscussionResultMap()
    {
      Table("DiscussionResults");
      Id(x => x.Id).GeneratedBy.Identity();
      Map(x => x.Beginning);
      Map(x => x.Ending);
      Map(x => x.Theme);
      Map(x => x.Resume);
      HasMany(x => x.UsersCards).Inverse().Cascade.All().Not.LazyLoad();
    }
  }
}
