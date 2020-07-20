using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataService;
using DataService.Models;
using ScrumPokerWeb.DTO;

namespace ScrumPokerWeb.Services
{
  public class DiscussionResultService
  {
    private readonly IRepository<DiscussionResult> resultRepository;
    private readonly IRepository<Card> cardRepository;
    private readonly IRepository<User> userRepository;

    public DiscussionResultService(IRepository<DiscussionResult> resultRepository, IRepository<Card> cardRepository, IRepository<User> userRepository)
    {
      this.resultRepository = resultRepository;
      this.cardRepository = cardRepository;
      this.userRepository = userRepository;
    }

    public DiscussionResultDto Get(long id)
    {
      return DtoUtil.GetDiscussionResultTO(resultRepository.Get(id));
    }

    public IEnumerable<DiscussionResultDto> GetAll()
    {
      return DtoUtil.GetDiscussionResultsTOs(resultRepository.GetAll());
    }

    public long Create(string theme)
    {
      DiscussionResult result = new DiscussionResult();
      result.Beginning = DateTime.Now;
      result.Theme = theme;
      resultRepository.Create(result);
      return result.Id;
    }

    public IEnumerable<DiscussionResultDto> GetByName(string owner)
    {
      IEnumerable<DiscussionResult> discussionResults = resultRepository.GetAll()
          .Where(dr => dr.UsersCards.Where(uc => uc.User.Name == owner).ToArray().Length > 0);
      return DtoUtil.GetDiscussionResultsTOs(discussionResults);
    }

    public void EndDiscussion(long id, string resume)
    {
      DiscussionResult result = resultRepository.Get(id);
      result.Resume = resume;
      result.Ending = DateTime.Now;
      resultRepository.Update(result);
    }

    public void ResetDiscussionResult(long id)
    {
      DiscussionResult result = resultRepository.Get(id);
      result.Ending = DateTime.MinValue;
      result.Beginning = DateTime.Now;
      result.Resume = string.Empty;
      result.UsersCards = new HashSet<UserCard>();
      resultRepository.Update(result);
    }

    public void AddOrChangeMark(long id, string loggedUser, string cardId)
    {
      DiscussionResult result = resultRepository.Get(id);
      if (result.Ending == DateTime.MinValue)
      {
        User user = userRepository.GetAll().FirstOrDefault(u => u.Name == loggedUser);
        Card card = cardRepository.Get(long.Parse(cardId));
        UserCard userCard = new UserCard();
        userCard.User = user;
        userCard.Card = card;
        userCard.DiscussionResult = result;
        if (result.UsersCards.Count(uc => userCard.User.Equals(user)) > 0)
        {
          result.UsersCards = result.UsersCards.Where(uc => !uc.User.Equals(user)).ToHashSet();
        }
        result.UsersCards.Add(userCard);
        resultRepository.Update(result);
      }
      else
      {
        throw new AccessViolationException("Нельзя добавлять оценки в законченное обсуждение");
      }
    }
  }
}
