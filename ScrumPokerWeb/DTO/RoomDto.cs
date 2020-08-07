using System;
using System.Collections.Generic;

namespace ScrumPokerWeb.DTO
{
  /// <summary>
  /// Отображение комнаты.
  /// </summary>
  public class RoomDto : BaseDto
  {
    /// <summary>
    /// Конструктор отображения.
    /// </summary>
    /// <param name="protected">Защищена ли комната паролем.</param>
    /// <param name="users">Пользователи комнаты.</param>
    /// <param name="owner">Владелец.</param>
    /// <param name="deck">Используемая колода.</param>
    /// <param name="id">ИД комнаты.</param>
    /// <param name="timerDuration">Количество минут таймера.</param>
    public RoomDto(bool @protected, ISet<UserDto> users, UserDto owner, DeckDto deck, long id, TimeSpan? timerDuration, string name, ISet<DiscussionResultDto> results) : base(id)
    {
      this.Protected = @protected;
      this.Users = users;
      this.Owner = owner;
      this.Deck = deck;
      this.TimerDuration = timerDuration;
      this.Name = name;
      this.DiscussionResults = results;
    }

    /// <summary>
    /// Защищенность паролем.
    /// </summary>
    /// <value>Правда, если есть пароль.</value>
    public bool Protected { get; }

    /// <summary>
    /// Пользователи в комнате.
    /// </summary>
    /// <value>Коллекция ДТО юзеров.</value>
    public ISet<UserDto> Users { get; }

    /// <summary>
    /// Владелец комнаты.
    /// </summary>
    /// <value>ДТО юзер.</value>
    public UserDto Owner { get; }

    /// <summary>
    /// Используемая колода.
    /// </summary>
    /// <value>ДТО колоды.</value>
    public DeckDto Deck { get; }

    /// <summary>
    /// Название комнаты.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Продолжительность таймера таймера.
    /// </summary>
    /// <value>Временной промежуток.</value>
    public TimeSpan? TimerDuration { get; }

    /// <summary>
    /// Результаты обсуждений комнаты.
    /// </summary>
    /// <value>Коллекция ДТО результатов обсуждений.</value>
    public ISet<DiscussionResultDto> DiscussionResults { get; set; }

    /// <summary>
    /// Переопределение эквивалентности для объектов одного класса.
    /// </summary>
    /// <param name="other">Объект для сравнения.</param>
    /// <returns>Равны ли объекты.</returns>
    protected bool Equals(RoomDto other)
    {
      return Protected == other.Protected && Equals(Owner, other.Owner) && Equals(Deck, other.Deck) && Name == other.Name && TimerDuration == other.TimerDuration;
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
      return Equals((RoomDto)obj);
    }

    /// <summary>
    /// Переопределение хэш-функции.
    /// </summary>
    /// <returns>Целочисленный хэш-код.</returns>
    public override int GetHashCode()
    {
      return HashCode.Combine(Protected, Owner, Deck, Name, TimerDuration);
    }
  }
}
