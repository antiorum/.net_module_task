using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataService;
using DataService.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScrumPokerWeb.DTO;
using ScrumPokerWeb.Services;

namespace ScrumPokerWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        UserService service;

        public UsersController(UserService service)
        {
            this.service = service;
        }

        [HttpPost("auth")]
        [AllowAnonymous]
        public async void Auth()
        {
            string name = Request.Form["name"];
            User user = service.GetByName(name);
            if (user == null)
            {
                user = new User();
                user.Name = name;
                service.Create(user);
                user = service.GetByName(name);
            }

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
            };
            ClaimsIdentity id = new ClaimsIdentity(claims,
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(id),
                new AuthenticationProperties
                {
                  IsPersistent = true
                });
        }



        [HttpGet]
        public IEnumerable<UserDto> GetAll()
        {
            return service.GetAll();
        }

        [HttpGet("logout")]
        public async void Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet("currentUser")]
        public string GetCurrentUser()
        {
            return User.Identity.Name;
        }
    }
}
