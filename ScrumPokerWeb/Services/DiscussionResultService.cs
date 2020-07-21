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

    /// <summary>
    /// Конструктор сервиса.
    /// </summary>
    /// <param name="resultRepository">Репозиторий результатов.</param>
    /// <param name="cardRepository">Репозиторий карт.</param>
    /// <param name="userRepository">Репозиторий пользователей.</param>
    public DiscussionResultService(IRepository<DiscussionResult> resultRepository, IRepository<Card> cardRepository,
      IRepository<User> userRepository)
    {
      this.resultRepository = resultRepository;
      this.cardRepository = cardRepository;
      this.userRepository = userRepository;
    }

    /// <summary>
    /// Ищет результат обсуждения по ИД.
    /// </summary>
    /// <param name="id">ИД результата.</param>
    /// <returns>ДТО результата.</returns>
    public DiscussionResultDto Get(long id)
    {
      return DtoUtil.GetDiscussionResultDto(this.resultRepository.Get(id));
    }

    /// <summary>
    /// Все результаты обсуждения.
    /// </summary>
    /// <returns>Коллекцию ДТО результатов.</returns>
    public IEnumerable<DiscussionResultDto> GetAll()
    {
      return DtoUtil.GetDiscussionResultsDtos(this.resultRepository.GetAll());
    }

    /// <summary>
    /// Создаёт новый результат обсуждения.
    /// </summary>
    /// <param name="theme">Тема обсуждения.</param>
    /// <returns>ИД созданного результата.</returns>
    public long Create(string theme)
    {
      var result = new DiscussionResult();
      result.Beginning = DateTime.Now;
      result.Theme = theme;
      this.resultRepository.Create(result);
      return result.Id;
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
      return DtoUtil.GetDiscussionResultsDtos(discussionResults);
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
      if (result.Ending == DateTime.MinValue)
      {
        var user = this.userRepository.GetAll().FirstOrDefault(u => u.Name == loggedUser);
        var card = this.cardRepository.Get(long.Parse(cardId));
        var userCard = new UserCard();
        userCard.User = user;
        userCard.Card = card;
        userCard.DiscussionResult = result;
        if (result.UsersCards.Count(uc => userCard.User.Equals(user)) > 0)
          result.UsersCards = result.UsersCards.Where(uc => !uc.User.Equals(user)).ToHashSet();
        result.UsersCards.Add(userCard);
        this.resultRepository.Update(result);
      }
      else
      {
        throw new AccessViolationException("Нельзя добавлять оценки в законченное обсуждение");
      }
    }
  }
}