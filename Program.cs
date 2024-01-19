using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using N8BlazorServerAuth.Components;
using N8BlazorServerAuth.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//# for BLAZOR COOKIE Auth
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddHttpContextAccessor();
/// ※ 不要在前端元件使用 IHttpContextAccesser 因為 HttpContext 的狀態並非即時反應的，
/// 成因是 SignalR 與 HTTP 是不同的通訊協定且原則上不能共存。
/// 但 Blazor Server App 又支援 Cookie Authentication。
/// 改用 AuthenticationStateProvider 機制取登入狀態。

//## for BLAZOR COOKIE Auth
/// ref → https://blazorhelpwebsite.com/ViewBlogPost/36
builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => true;
  options.MinimumSameSitePolicy = SameSiteMode.Strict;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(cfg =>
    {
      cfg.LoginPath = "/Accout/Login"; // default: /Accout/Login
      cfg.Cookie.Name = ".N8BlazorServerAuth.Cookies"; //default:.AspNetCore.Cookies
    });

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
