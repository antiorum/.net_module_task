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
  /// ������� ����, �������� ���� ������.
  /// </summary>
  public class BaseTest
  {
    /// <summary>
    /// �������� ������������ - ��������.
    /// </summary>
    protected const string TestOwner = "Bor'ka";

    /// <summary>
    /// �������� ������������ ��� ����.
    /// </summary>
    protected const string WrongUser = "Valera Kotov";

    /// <summary>
    /// ����, � ������� ���������� �������� ������, ���������� ������ �.
    /// </summary>
    public static string InvokedSignalRMethod = string.Empty;

    /// <summary>
    /// �������� ���� ������.
    /// </summary>
    protected IHubContext<RoomsHub> context;

    /// <summary>
    /// ����������� ����.
    /// </summary>
    protected IRepository<Card> cardRepository;

    /// <summary>
    /// ����������� �����.
    /// </summary>
    protected IRepository<Deck> deckRepository;

    /// <summary>
    /// ����������� ����������� ����������.
    /// </summary>
    protected IRepository<DiscussionResult> discussionResultRepository;

    /// <summary>
    /// ����������� ������.
    /// </summary>
    protected IRepository<Room> roomRepository;

    /// <summary>
    /// ����������� �������������.
    /// </summary>
    protected IRepository<User> userRepository;

    /// <summary>
    /// ������ �������������.
    /// </summary>
    protected UserService userService;

    /// <summary>
    /// ������ ����.
    /// </summary>
    protected CardService cardService;

    /// <summary>
    /// ������ �����.
    /// </summary>
    protected DeckService deckService;

    /// <summary>
    /// ������ ����������� ���������.
    /// </summary>
    protected DiscussionResultService discussionResultService;

    /// <summary>
    /// ������ ������.
    /// </summary>
    protected RoomService roomService;

    /// <summary>
    /// �������� ������ ����.
    /// </summary>
    protected Cards cards;

    /// <summary>
    /// �������� ������ �����.
    /// </summary>
    protected Decks decks;

    /// <summary>
    /// �������� ������ ����������� ����������.
    /// </summary>
    protected DiscussionResults discussionResults;

    /// <summary>
    /// �������� ������ ������.
    /// </summary>
    protected Rooms rooms;

    /// <summary>
    /// �������� ������ �������������.
    /// </summary>
    protected Users users;

    /// <summary>
    /// ������������� �������� ��������� ����� ������ ������.
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