using DataService;
using DataService.Models;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;
using ScrumPokerWeb.Services;
using ScrumPokerWeb.SignalR;
using UnitTests.TestData;
using UnitTests.TestRepositories;
using UnitTests.Utils;

namespace UnitTests
{
  public class BaseTest
  {
    protected const string TestOwner = "Bor'ka";
    protected const string wrongUser = "Valera Kotov";
    public static string InvokedSignalRMethod = string.Empty;

    protected IHubContext<RoomsHub> context;
      
    protected IRepository<Card> cardRepository;
    protected IRepository<Deck> deckRepository;
    protected IRepository<DiscussionResult> discussionResultRepository;
    protected IRepository<Room> roomRepository;
    protected IRepository<User> userRepository;
     
    protected UserService userService;
    protected CardService cardService;
    protected DeckService deckService;
    protected DiscussionResultService discussionResultService;
    protected RoomService roomService;

    protected Cards cards;
    protected Decks decks;
    protected DiscussionResults discussionResults;
    protected Rooms rooms;
    protected Users users;

    [SetUp]
    public void Setup()
    {
      context = HubContextImplementation.GetContext;

      cardRepository = new CardRepository();
      deckRepository  = new DeckRepository();
      discussionResultRepository = new DiscussionResultRepository();
      roomRepository = new RoomRepository();
      userRepository = new UserRepository();

      userService = new UserService(userRepository, context);
      cardService = new CardService(cardRepository, context, userService);
      deckService = new DeckService(deckRepository, cardRepository, context, userService);
      discussionResultService = new DiscussionResultService(discussionResultRepository, cardRepository, userRepository);
      roomService = new RoomService(roomRepository, userRepository, deckRepository, context, userService, discussionResultService);

      userService.AddUserToConnectionMap("TestUser", "TestConnection");

      cards = new Cards();
      decks = new Decks();
      discussionResults = new DiscussionResults();
      rooms = new Rooms();
      users = new Users();

      InvokedSignalRMethod = string.Empty;
    }
  }
}