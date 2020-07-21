using DataService;
using DataService.Models;
using DataService.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScrumPokerWeb.Services;
using ScrumPokerWeb.SignalR;

namespace ScrumPokerWeb
{
  /// <summary>
  /// ������������ ���-����������.
  /// </summary>
  public class Startup
  {
    /// <summary>
    /// ����������� ������.
    /// </summary>
    /// <param name="configuration">������ ���������� ������������.</param>
    public Startup(IConfiguration configuration)
    {
      this.Configuration = configuration;
    }

    /// <summary>
    /// ������ ���������� ������������.
    /// </summary>
    /// <value>���������� ����������.</value>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// ����������� ��������.
    /// </summary>
    /// <param name="services">��������� ��������.</param>
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddTransient<IRepository<Card>, CardRepository>();
      services.AddTransient<IRepository<Deck>, DeckRepository>();
      services.AddTransient<IRepository<DiscussionResult>, DiscussionResultRepository>();
      services.AddTransient<IRepository<Room>, RoomRepository>();
      services.AddTransient<IRepository<User>, UserRepository>();
      services.AddSingleton(typeof(RoomService));
      services.AddSingleton(typeof(CardService));
      services.AddSingleton(typeof(DeckService));
      services.AddSingleton(typeof(DiscussionResultService));
      services.AddSingleton(typeof(UserService));

      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
          options.LoginPath = new PathString("/api/users/auth");
          options.Cookie = new CookieBuilder
          {
            HttpOnly = false,
            Path = "/",
            SameSite = SameSiteMode.Lax,
            SecurePolicy = CookieSecurePolicy.None
          };
          options.SlidingExpiration = true;
        });

      services.AddAuthorization();
      services.AddSignalR(r => { r.MaximumReceiveMessageSize = 102400000; });
    }

    /// <summary>
    /// ������������ HTTP.
    /// </summary>
    /// <param name="app">��������� �������.</param>
    /// <param name="env">��������� �����.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true)
        .AllowCredentials());

      app.UseAuthentication();

      app.UseAuthorization();

      app.UseCookiePolicy(new CookiePolicyOptions
      {
        HttpOnly = HttpOnlyPolicy.None,
        Secure = CookieSecurePolicy.None
      });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapHub<RoomsHub>("/roomsHub");
      });
    }
  }
}