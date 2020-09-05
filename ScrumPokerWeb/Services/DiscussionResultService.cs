using System;
using System.Collections.Generic;
using System.Linq;
using DataService;
using DataService.Models;
using ScrumPokerWeb.DTO;

namespace ScrumPokerWeb.Services
{
  /// <summary>
  /// Сервис результатов обсуждений.
  /// </summary>
  public class DiscussionResultService
  {
    private readonly IRepository<Card> cardRepository;
    private readonly IRepository<DiscussionResult> resultRepository;
    private readonly IRepository<User> userRepository;
    private readonly IRepository<UserCard> userCardRepository;

    /// <summary>
    /// Конструктор сервиса.
    /// </summary>
    /// <param name="resultRepository">Репозиторий результатов.</param>
    /// <param name="cardRepository">Репозиторий карт.</param>
    /// <param name="userRepository">Репозиторий пользователей.</param>
    public DiscussionResultService(IRepository<DiscussionResult> resultRepository, IRepository<Card> cardRepository,
      IRepository<User> userRepository, IRepository<UserCard> userCardRepository)
    {
      this.resultRepository = resultRepository;
      this.cardRepository = cardRepository;
      this.userRepository = userRepository;
      this.userCardRepository = userCardRepository;
    }

    /// <summary>
    /// Ищет результат обсуждения по ИД.
    /// </summary>
    /// <param name="id">ИД результата.</param>
    /// <returns>ДТО результата.</returns>
    public DiscussionResultDto Get(long id)
    {
      return DtoConverters.GetDiscussionResultDto(this.resultRepository.Get(id));
    }

    /// <summary>
    /// Все результаты обсуждения.
    /// </summary>
    /// <returns>Коллекцию ДТО результатов.</returns>
    public IEnumerable<DiscussionResultDto> GetAll()
    {
      return DtoConverters.GetDiscussionResultsDtos(this.resultRepository.GetAll());
    }

    /// <summary>
    /// Создаёт новый результат обсуждения.
    /// </summary>
    /// <param name="theme">Тема обсуждения.</param>
    /// <returns>Cозданный результата.</returns>
    public DiscussionResult Create(string theme)
    {
      var result = new DiscussionResult();
      result.Beginning = DateTime.Now;
      result.Theme = theme;
      this.resultRepository.Save(result);
      return result;
    }

    /// <summary>
    /// Находит результаты обсуждений по имени пользователя.
    /// </summary>
    /// <param name="owner">Имя пользователя.</param>
    /// <returns>Коллекцию ДТО результатов.</returns>
    public IEnumerable<DiscussionResultDto> GetByName(string owner)
    {
      var discussionResults = this.resultRepository.GetAll()
        .Where(dr => dr.UsersCards.Where(uc => uc.User.Name == owner).ToArray().Length > 0);
      return DtoConverters.GetDiscussionResultsDtos(discussionResults);
    }

    /// <summary>
    /// Завершает обсуждение.
    /// </summary>
    /// <param name="id">ИД результата обсуждения.</param>
    /// <param name="resume">Итог или комментарий обсуждения.</param>
    public void EndDiscussion(long id, string resume)
    {
      var result = this.resultRepository.Get(id);
      result.Resume = resume;
      result.Ending = DateTime.Now;
      this.resultRepository.Update(result);
    }

    /// <summary>
    /// Сбрасывает результат обсуждения и оно начинается заново.
    /// </summary>
    /// <param name="id">ИД результата обсуждения.</param>
    public void ResetDiscussionResult(long id)
    {
      var result = this.resultRepository.Get(id);
      result.Ending = DateTime.MinValue;
      result.Beginning = DateTime.Now;
      result.Resume = string.Empty;
      result.UsersCards = new HashSet<UserCard>();
      this.resultRepository.Update(result);
    }

    /// <summary>
    /// Добавляет или заменяет в результате обсуждения оценку пользователя. 
    /// </summary>
    /// <param name="id">ИД результата.</param>
    /// <param name="loggedUser">Пользователь.</param>
    /// <param name="cardId">ИД показанной карты.</param>
    public void AddOrChangeMark(long id, string loggedUser, string cardId)
    {
      var result = this.resultRepository.Get(id);
      if (result.Ending != DateTime.MinValue)
      {
        throw new AccessViolationException("Нельзя добавлять оценки в законченное обсуждение");
      }
      var user = this.userRepository.GetAll().FirstOrDefault(u => u.Name == loggedUser);
      var card = this.cardRepository.Get(long.Parse(cardId));
      var userCard = new UserCard();
      userCard.User = user;
      userCard.Card = card;
      if (result.UsersCards.Count(userCard => userCard.User.Equals(user)) > 0)
      {
        //result.UsersCards = result.UsersCards.Where(uc => !uc.User.Equals(user)).ToHashSet();
        UserCard oldUserCard = result.UsersCards.FirstOrDefault(uc => uc.User.Id == user.Id);
        result.UsersCards.Remove(oldUserCard);
        userCardRepository.Delete(oldUserCard.Id);
        this.resultRepository.Update(result);
      }
      userCard.DiscussionResult = result;
      result.UsersCards.Add(userCard);
      this.resultRepository.Update(result);
    }

    public void Delete(in long id)
    {
      resultRepository.Delete(id);
    }

    public void Rename(long id, string newName)
    {
      var discussionResult = resultRepository.Get(id);
      discussionResult.Theme = newName;
      resultRepository.Update(discussionResult);
    }
  }
}