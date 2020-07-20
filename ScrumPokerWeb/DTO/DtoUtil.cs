using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataService.Models;

namespace ScrumPokerWeb.DTO
{
    public static class DtoUtil
    {
        public static CardDto GetCardTO(Card card)
        {
            bool common = card.Owner == null ? true : false;
            return new CardDto(common, card.Name, card.Value, card.Image, card.Id);
        }

        public static IEnumerable<CardDto> GetCardsTOs(IEnumerable<Card> cards)
        {
            return cards.Select(card => GetCardTO(card));
        }

        public static DeckDto GetDeckTO(Deck deck)
        {
            HashSet<CardDto> cards = new HashSet<CardDto>();
            foreach (Card c in deck.Cards)
            {
                cards.Add(GetCardTO(c));
            }
            bool isCommon = deck.Owner == null;
            return new DeckDto(isCommon, deck.Name, cards, deck.Id);
        }

        public static IEnumerable<DeckDto> GetDecksTOs(IEnumerable<Deck> decks)
        {
            return decks.Select(deck => GetDeckTO(deck));
        }

        public static RoomDto GetRoomTO(Room room)
        {
            bool @protected = room.Password != string.Empty;

            return new RoomDto(
                @protected,
                GetUsersTOs(room.Users).ToHashSet(),
                GetUserTO(room.Owner),
                GetDeckTO(room.Deck),
                room.Id,
                room.TimerMinutes);
        }

        public static IEnumerable<RoomDto> GetRoomsTOs(IEnumerable<Room> rooms)
        {
            return rooms.Select(room => GetRoomTO(room));
        }

        public static DiscussionResultDto GetDiscussionResultTO(DiscussionResult result)
        {
            return new DiscussionResultDto(
                result.Beginning, result.Ending, result.Theme, result.Resume,
                result.UsersCards.ToDictionary(uc => uc.User.Name, uc => uc.Card.Value),
                result.Id);
        }

        public static IEnumerable<DiscussionResultDto> GetDiscussionResultsTOs(IEnumerable<DiscussionResult> discussionResults)
        {
            return discussionResults.Select(dr => GetDiscussionResultTO(dr));
        }

        public static UserDto GetUserTO(User user)
        {
            return new UserDto(user.Name, user.Id);
        }

        public static IEnumerable<UserDto> GetUsersTOs(IEnumerable<User> users)
        {
            return users.Select(u => GetUserTO(u));
        }
    }
}
