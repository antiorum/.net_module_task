using DataService;
using DataService.Models;
using Microsoft.AspNetCore.SignalR;
using NUnit.Framework;
using ScrumPokerWeb.Services;
using ScrumPokerWeb.SignalR;
using UnitTests.TestData;
using UnitTests.TestRepositories;
using UnitTests.Utils;

namespace UnitTests
{
  /// <summary>
  /// Базовый тест, родитель всех тестов.
  /// </summary>
  public class BaseTest
  {
    /// <summary>
    /// Тестовый пользователь - владелец.
    /// </summary>
    protected const string TestOwner = "Bor'ka";

    /// <summary>
    /// Тестовый пользователь без прав.
    /// </summary>
    protected const string WrongUser = "Valera Kotov";

    /// <summary>
    /// Поле, в котором появляется название метода, вызванного сигнал р.
    /// </summary>
    public static string InvokedSignalRMethod = string.Empty;

    /// <summary>
    /// Контекст хаба комнат.
    /// </summary>
    protected IHubContext<RoomsHub> context;

    /// <summary>
    /// Репозиторий карт.
    /// </summary>
    protected IRepository<Card> cardRepository;

    /// <summary>
    /// Репозиторий колод.
    /// </summary>
    protected IRepository<Deck> deckRepository;

    /// <summary>
    /// Репозиторий результатов обсуждений.
    /// </summary>
    protected IRepository<DiscussionResult> discussionResultRepository;

    /// <summary>
    /// Репозиторий комнат.
    /// </summary>
    protected IRepository<Room> roomRepository;

    /// <summary>
    /// Репозиторий пользователей.
    /// </summary>
    protected IRepository<User> userRepository;

    /// <summary>
    /// Сервис пользователей.
    /// </summary>
    protected UserService userService;

    /// <summary>
    /// Сервис карт.
    /// </summary>
    protected CardService cardService;

    /// <summary>
    /// Сервис колод.
    /// </summary>
    protected DeckService deckService;

    /// <summary>
    /// Сервис результатов дискуссий.
    /// </summary>
    protected DiscussionResultService discussionResultService;

    /// <summary>
    /// Сервис комнат.
    /// </summary>
    protected RoomService roomService;

    /// <summary>
    /// Тестовые данные карт.
    /// </summary>
    protected Cards cards;

    /// <summary>
    /// Тестовые данные колод.
    /// </summary>
    protected Decks decks;

    /// <summary>
    /// Тестовые данные результатов обсуждений.
    /// </summary>
    protected DiscussionResults discussionResults;

    /// <summary>
    /// Тестовые данные комнат.
    /// </summary>
    protected Rooms rooms;

    /// <summary>
    /// Тестовые данные пользователей.
    /// </summary>
    protected Users users;

    /// <summary>
    /// Конфигурирует тестовое окружение перед каждым тестом.
    /// </summary>
    [SetUp]
    public void Setup()
    {
      this.context = HubContextImplementation.GetContext;

      this.cardRepository = new CardRepository();
      this.deckRepository = new DeckRepository();
      this.discussionResultRepository = new DiscussionResultRepository();
      this.roomRepository = new RoomRepository();
      this.userRepository = new UserRepository();

      this.userService = new UserService(this.userRepository, this.context);
      this.cardService = new CardService(this.cardRepository, this.context, this.userService);
      this.deckService = new DeckService(this.deckRepository, this.cardRepository, this.context, this.userService);
      this.discussionResultService = new DiscussionResultService(this.discussionResultRepository, this.cardRepository, this.userRepository);
      this.roomService = new RoomService(this.roomRepository, this.userRepository, this.deckRepository, this.context, this.userService, this.discussionResultService);

      this.userService.AddUserToConnectionMap("TestUser", "TestConnection");

      this.cards = new Cards();
      this.decks = new Decks();
      this.discussionResults = new DiscussionResults();
      this.rooms = new Rooms();
      this.users = new Users();

      InvokedSignalRMethod = string.Empty;
    }
  }
}