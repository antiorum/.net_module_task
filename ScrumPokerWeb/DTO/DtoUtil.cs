using System.Collections.Generic;
using System.Linq;
using DataService.Models;

namespace ScrumPokerWeb.DTO
{
  /// <summary>
  /// Содержит утилитные методы для ДТО.
  /// </summary>
  public static class DtoUtil
  {
    /// <summary>
    /// Создаёт отображение из карты.
    /// </summary>
    /// <param name="card">Сущность - карта.</param>
    /// <returns>Отображение карты.</returns>
    public static CardDto GetCardDto(Card card)
    {
      bool common = card.Owner == null ? true : false;
      return new CardDto(common, card.Name, card.Value, card.Image, card.Id);
    }

    /// <summary>
    /// Создаёт коллекцию отображений из коллекции карт.
    /// </summary>
    /// <param name="cards">Коллекция сущностей - карт.</param>
    /// <returns>Коллекция отображений.</returns>
    public static IEnumerable<CardDto> GetCardsDtos(IEnumerable<Card> cards)
    {
      return cards.Select(card => GetCardDto(card));
    }

    /// <summary>
    /// Создаёт ДТО из колоды.
    /// </summary>
    /// <param name="deck">Сущность-колода.</param>
    /// <returns>ДТО колоды.</returns>
    public static DeckDto GetDeckDto(Deck deck)
    {
      HashSet<CardDto> cards = new HashSet<CardDto>();
      foreach (Card c in deck.Cards)
      {
        cards.Add(GetCardDto(c));
      }
      bool isCommon = deck.Owner == null;
      return new DeckDto(isCommon, deck.Name, cards, deck.Id);
    }

    /// <summary>
    /// Создаёт коллекцию ДТО колод.
    /// </summary>
    /// <param name="decks">Коллекция сущностей - колод.</param>
    /// <returns>Коллекция ДТО.</returns>
    public static IEnumerable<DeckDto> GetDecksDtos(IEnumerable<Deck> decks)
    {
      return decks.Select(deck => GetDeckDto(deck));
    }

    /// <summary>
    /// Создёт ДТО комнаты из сущности.
    /// </summary>
    /// <param name="room">Сущность -комната.</param>
    /// <returns>ДТО комнаты.</returns>
    public static RoomDto GetRoomDto(Room room)
    {
      bool @protected = room.Password != string.Empty;

      return new RoomDto(
          @protected,
          GetUsersDtos(room.Users).ToHashSet(),
          GetUserDto(room.Owner),
          GetDeckDto(room.Deck),
          room.Id,
          room.TimerMinutes);
    }

    /// <summary>
    /// Создёт коллекцию ДТО комнат из сущностей.
    /// </summary>
    /// <param name="rooms">Коллекция сущностей-комнат.</param>
    /// <returns>Коллекция ДТО.</returns>
    public static IEnumerable<RoomDto> GetRoomsDtos(IEnumerable<Room> rooms)
    {
      return rooms.Select(room => GetRoomDto(room));
    }

    /// <summary>
    /// Создаёт ДТО результата дискуссии.
    /// </summary>
    /// <param name="result">Сущность - результат дискуссии.</param>
    /// <returns>Результат - ДТО.</returns>
    public static DiscussionResultDto GetDiscussionResultDto(DiscussionResult result)
    {
      return new DiscussionResultDto(
          result.Beginning, result.Ending, result.Theme, result.Resume,
          result.UsersCards.ToDictionary(uc => uc.User.Name, uc => uc.Card.Value),
          result.Id);
    }

    /// <summary>
    /// Создаёт коллекцию ДТО результатов дискуссии.
    /// </summary>
    /// <param name="discussionResults">Коллекция сущностей - результатов.</param>
    /// <returns>Коллекция ДТО.</returns>
    public static IEnumerable<DiscussionResultDto> GetDiscussionResultsDtos(IEnumerable<DiscussionResult> discussionResults)
    {
      return discussionResults.Select(dr => GetDiscussionResultDto(dr));
    }

    /// <summary>
    /// Создаёт ДТО юзера.
    /// </summary>
    /// <param name="user">Сущность - юзер.</param>
    /// <returns>ДТО - юзер.</returns>
    public static UserDto GetUserDto(User user)
    {
      return new UserDto(user.Name, user.Id);
    }

    /// <summary>
    /// Создаёт коллекцию ДТО юзеров.
    /// </summary>
    /// <param name="users">Коллекция сущностей - юзеров.</param>
    /// <returns>Коллекция ДТО.</returns>
    public static IEnumerable<UserDto> GetUsersDtos(IEnumerable<User> users)
    {
      return users.Select(u => GetUserDto(u));
    }
  }
}
