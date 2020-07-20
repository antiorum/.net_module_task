using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class DiscussionResultsController : ControllerBase
    {
        DiscussionResultService service;
        private string LoggedUser => User.Identity.Name.ToString();

        public DiscussionResultsController(DiscussionResultService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IEnumerable<DiscussionResultDto> Get()
        {
            return service.GetAll();
        }

        [HttpGet("own")]
        public IEnumerable<DiscussionResultDto> GetOwnResults()
        {
            return service.GetByName(LoggedUser);
        }

        [HttpGet("{id}")]
        public DiscussionResultDto Get(long id)
        {
            return service.Get(id);
        }

        //[HttpPost]
        //public void Post()
        //{
        //    string beginig = Request.Form["beginig"];
        //    string ending = Request.Form["ending"];
        //    string theme = Request.Form["theme"];
        //    string resume = Request.Form["resume"];
        //    string usersCardsIds = Request.Form["usersCards"];
        //    service.Create(beginig, ending, theme, resume, usersCardsIds);
        //}

        //[HttpPut("end/{id}")]
        //public void EndDiscussion(long id)
        //{
        //    service.EndDiscussion(id);
        //}

        //[HttpPut("addMark/{id}")]
        //public void AddOrChangeMark(long id)
        //{
        //    string cardId = Request.Form["cardId"];
        //    service.AddOrChangeMark(id, LoggedUser, cardId);
        //}
    }
}
