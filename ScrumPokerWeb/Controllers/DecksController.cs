using System.Collections.Generic;
using DataService;
using DataService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumPokerWeb.DTO;
using ScrumPokerWeb.Services;

namespace ScrumPokerWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DecksController : ControllerBase
    {
        DeckService service;
        private string LoggedUser => User.Identity.Name;

        public DecksController(DeckService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IEnumerable<DeckDto> GetAvailable()
        {
            return service.GetAvailable(LoggedUser);
        }

        [HttpGet("{id}")]
        public DeckDto Get(long id)
        {
            return service.Get(id);
        }

        [HttpPost]
        public void Post()
        {
            string name = Request.Form["name"];
            string cards = Request.Form["cards"];
            service.Create(LoggedUser, name, cards);
        }

        [HttpPut("{id}")]
        public void Put(long id)
        {
            string name = Request.Form["name"];
            string cards = Request.Form["cards"];
            service.Update(id, LoggedUser, name, cards);
        }

        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            service.Delete(id, LoggedUser);
        }
    }
}
