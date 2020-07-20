using System.Collections.Generic;
using DataService;
using DataService.Models;
using FluentNHibernate.Conventions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumPokerWeb.DTO;
using ScrumPokerWeb.Services;

namespace ScrumPokerWeb.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class RoomsController : ControllerBase
  {
    private readonly RoomService service;
    private string LoggedUser => User.Identity.Name.ToString();

    public RoomsController(RoomService service)
    {
      this.service = service;
    }

    [HttpGet]
    public IEnumerable<RoomDto> GetAll()
    {
      return service.GetAll();
    }

    [HttpGet("{id}")]
    public RoomDto Get(long id)
    {
      return service.Get(id, LoggedUser);
    }

    [HttpPost("{id}")]
    public RoomDto LoginRoom(long id)
    {
      string password = Request.Form["password"];
      return service.AddUserToRoomAndGet(id, LoggedUser, password);
    }

    [HttpPost]
    public void Post()
    {
      string password = Request.Form["password"];
      string timerMinutes = Request.Form["timer"];
      if (timerMinutes.IsEmpty()) timerMinutes = "0";
      string deckId = Request.Form["deck"];
      service.Create(LoggedUser, password, timerMinutes, deckId);
    }

    [HttpDelete("{id}")]
    public void Delete(long id)
    {
      service.Delete(id, LoggedUser);
    }


    [HttpPut("{id}/changePassword")]
    public void Put(long id)
    {
      string newPassword = Request.Form["password"];
      service.ChangeRoomPassword(id, LoggedUser, newPassword);
    }

    [HttpPost("{id}/deleteUser/{userId}")]
    public void DeleteUserFromRoom(long id, long userId)
    {
      service.DeleteUserFromRoom(id, userId, LoggedUser);
    }

    [HttpPost("{id}/exit")]
    public void ExitFromRoom(long id)
    {
      service.ExitFromRoom(id, LoggedUser);
    }

    [HttpPut("{id}/startDiscussion")]
    public void StartDiscussion(long id)
    {
      string theme = Request.Form["theme"];
      service.StartNewDiscussion(id, LoggedUser, theme);
    }

    [HttpPut("{id}/addMark")]
    public void AddMarkInDiscussion(long id)
    {
      string cardId = Request.Form["cardId"];
      service.AddOrChangeMarkInCurrentDiscussion(id, LoggedUser, cardId);
    }

    [HttpPut("{id}/endDiscussion")]
    public void EndDiscussion(long id)
    {
      string resume = Request.Form["resume"];
      service.EndCurrentDiscussion(id, LoggedUser, resume);
    }

    [HttpPut("{id}/restartDiscussion")]
    public void RestartDiscussion(long id)
    {
      service.RestartDiscussion(id, LoggedUser);
    }
  }
}
