using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using N8BlazorServerAuth.Components;
using N8BlazorServerAuth.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

//# for BLAZOR COOKIE Auth
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddHttpContextAccessor();
/// �� ���n�b�e�ݤ���ϥ� IHttpContextAccesser �]�� HttpContext �����A�ëD�Y�ɤ������A
/// ���]�O SignalR �P HTTP �O���P���q�T��w�B��h�W����@�s�C
/// �� Blazor Server App �S�䴩 Cookie Authentication�C
/// ��� AuthenticationStateProvider ������n�J���A�C

//## for BLAZOR COOKIE Auth
/// ref �� https://blazorhelpwebsite.com/ViewBlogPost/36
builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.MinimumSameSitePolicy = SameSiteMode.Strict;
  options.CheckConsentNeeded = context => true;
  options.ConsentCookie.Name = "GDPRConsent";
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(cfg =>
    {
      cfg.LoginPath = "/Accout/Login"; // default: /Accout/Login
      cfg.Cookie.Name = ".N8BlazorServerAuth.Cookies"; //default:.AspNetCore.Cookies
    });

//���� ���U�G�Ȼs�A��
builder.Services.AddSingleton<AccountService>();

var app = builder.Build(); //--------------------------------------------------

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

//# for BLAZOR COOKIE Auth
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

//���� HTTP 400-599 ���~�B�z 
/// �ھڹw�]�AASP.NET Core ���ε{�����|���� HTTP ���~���A�X (�Ҧp�u404 - �䤣��v) �����A�X�����C 
/// �����ε{���]�w�S�����媺 HTTP 400-599 ���~���A�X�ɡA���|�Ǧ^�Ӫ��A�X�M�@�Ӫťժ��^������C 
/// ref��https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0#usestatuscodepages
app.UseStatusCodePagesWithRedirects("/ErrorStatus/{0}");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
