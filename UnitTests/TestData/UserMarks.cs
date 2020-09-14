using DataService.Models;

namespace UnitTests.TestData
{
  /// <summary>
  /// Тестовые оценки пользователей.
  /// </summary>
  public class UserMarks
    {
        /// <summary>
        /// Тестовая оценка №1.
        /// </summary>
        public UserCard Mark1 = new UserCard()
        {
            Id = 1,
            DiscussionResult = null,
        };

        /// <summary>
        /// Тестовая оценка №2.
        /// </summary>
        public UserCard Mark2 = new UserCard()
        {
            Id = 2,
            DiscussionResult = null,
        };

        /// <summary>
        /// Тестовая оценка №3.
        /// </summary>
        public UserCard Mark3 = new UserCard()
        {
            Id = 3,
            DiscussionResult = null,
        };

        /// <summary>
        /// Конструктор с инициализацией тестовых данных.
        /// </summary>
        public UserMarks()
        {
            Cards cards = new Cards();
            Users users = new Users();
            this.Mark1.Card = cards.CardOne;
            this.Mark1.User = users.JohnUser;
            this.Mark2.Card = cards.CardFive;
            this.Mark2.User = users.ValeraUser;
            this.Mark3.Card = cards.CardCoffee;
            this.Mark3.User = users.BorkaUser;
        }
    }
}