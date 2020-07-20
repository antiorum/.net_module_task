using System.Collections.Generic;
using DataService.Models;
using Microsoft.AspNetCore.Mvc;
using ScrumPokerWeb.Services;
using ScrumPokerWeb.DTO;
using Microsoft.AspNetCore.Authorization;

namespace ScrumPokerWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        CardService service;
        private string LoggedUser => User.Identity.Name.ToString();

        public CardsController(CardService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IEnumerable<CardDto> GetAvailable()
        {
            return service.GetAvailable(LoggedUser);
        }

        [HttpGet("byId/{id}")]
        public CardDto Get(long id)
        {
            return service.Get(id);
        }

        [HttpPost]
        public void Post([FromForm] Card card)
        {
            card.Owner = LoggedUser;
            service.Save(card, LoggedUser);
        }

        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            service.Delete(id, LoggedUser);
        }
    }
}
