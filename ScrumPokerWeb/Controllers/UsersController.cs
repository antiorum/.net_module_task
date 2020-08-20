#nullable enable
using System;
using System.Collections.Generic;
using System.Security.Claims;
using DataService.Models;
using FluentNHibernate.Conventions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumPokerWeb.DTO;
using ScrumPokerWeb.Services;

namespace ScrumPokerWeb.Controllers
{
  /// <summary>
  /// Контроллер пользователей.
  /// </summary>
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    /// <summary>
    /// Сервис пользователей.
    /// </summary>
    private readonly UserService service;

    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="service">Сервис-синглтон.</param>
    public UsersController(UserService service)
    {
      this.service = service;
    }

    /// <summary>
    /// Запрос аутентификации.
    /// </summary>
    [HttpPost("auth")]
    [AllowAnonymous]
    public async void Auth()
    {
      string name = Request.Form["name"];
      if (name.IsEmpty())
      {
        throw new ArgumentException("Имя не должно быть пустым");
      }
      var user = service.GetByName(name);
      if (user == null)
      {
        user = new User();
        user.Name = name;
        service.Create(user);
        user = service.GetByName(name);
      }

      var claims = new[]
      {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Name)
      };
      var id = new ClaimsIdentity(claims,
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

    /// <summary>
    /// Показать всех пользователей.
    /// </summary>
    /// <returns>Коллекцию отображений пользователей.</returns>
    [HttpGet]
    public IEnumerable<UserDto> GetAll()
    {
      return service.GetAll();
    }

    /// <summary>
    /// Логаут.
    /// </summary>
    [HttpGet("logout")]
    public async void Logout()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    /// <summary>
    /// Показать имя текущего пользователя.
    /// </summary>
    /// <returns>Строка с именем.</returns>
    [AllowAnonymous]
    [HttpGet("currentUser")]
    public string? GetCurrentUser()
    {
      return User.Identity.Name;
    }
  }
}