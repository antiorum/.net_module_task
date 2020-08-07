using System;
using System.Collections.Generic;
using System.Linq;

namespace ScrumPokerWeb.DTO
{
  /// <summary>
  /// Отображение результатов дискуссий.
  /// </summary>
  public class DiscussionResultDto : BaseDto
  {
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="beginning">Дата начала.</param>
    /// <param name="ending">Дата окончания.</param>
    /// <param name="theme">Тема.</param>
    /// <param name="resume">Итог.</param>
    /// <param name="usersMarks">Оценки пользователей.</param>
    /// <param name="id">ИД.</param>
    public DiscussionResultDto(DateTime beginning, DateTime ending, string theme, string resume, IDictionary<string, CardDto> usersMarks, long id) : base(id)
    {
      Beginning = beginning;
      Ending = ending;
      Theme = theme;
      Resume = resume;
      UsersMarks = usersMarks;
    }

    /// <summary>
    /// Дата начала.
    /// </summary>
    /// <value>Дата и вермя.</value>
    public virtual DateTime Beginning { get; }

    /// <summary>
    /// Дата окончания.
    /// </summary>
    /// <value>Дата и вермя.</value>
    public virtual DateTime Ending { get; set; }

    /// <summary>
    /// Тема обсуждения.
    /// </summary>
    /// <value>Строка с темой.</value>
    public virtual string Theme { get; set; }

    /// <summary>
    /// Итог обсуждения или комментарий.
    /// </summary>
    /// <value>Строка с результатом.</value>
    public virtual string Resume { get; set; }

    /// <summary>
    /// Оценки пользователей.
    /// </summary>
    /// <value>Коллекция оценок.</value>
    public virtual IDictionary<string, CardDto> UsersMarks { get; set; }

    /// <summary>
    /// Вычисляет продолжительность обсуждения.
    /// </summary>
    /// <value>Временной интервал.</value>
    public virtual TimeSpan Duration
    {
      get
      {
        return this.Ending - this.Beginning;
      }
    }

    /// <summary>
    /// Вычисляет среднюю оценку участников.
    /// </summary>
    /// <value>Среднее значение с плавающй точкой.</value>
    public virtual double? AverageMark
    {
      get
      {
        return this.UsersMarks.Values.Select(card => card.Value).Where(v => v != null).Average();
      }
    }

    public virtual bool isCompleted => !Ending.Equals(DateTime.MinValue);

    /// <summary>
    /// Переопределение эквивалентности для объектов одного класса.
    /// </summary>
    /// <param name="other">Объект для сравнения.</param>
    /// <returns>Одинаковы ли объекты.</returns>
    protected bool Equals(DiscussionResultDto other)
    {
      return Beginning.Equals(other.Beginning) && Ending.Equals(other.Ending) && Theme == other.Theme && Resume == other.Resume;
    }

    /// <summary>
    /// Переопределение эквивалентности.
    /// </summary>
    /// <param name="obj">Объект для сравнения.</param>
    /// <returns>Одинаковы ли объекты.</returns>
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((DiscussionResultDto)obj);
    }

    /// <summary>
    /// Переопределение хэш-функции.
    /// </summary>
    /// <returns>Целочисленный хэш-код.</returns>
    public override int GetHashCode()
    {
      return HashCode.Combine(Beginning, Ending, Theme, Resume);
    }
  }
}
