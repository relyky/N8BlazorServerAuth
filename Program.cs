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
/// 參考：[針對錯誤進行疑難排解](https://learn.microsoft.com/zh-tw/aspnet/core/blazor/security/?view=aspnetcore-8.0#troubleshoot-errors)
/// 在.NET 8 或更新版本中，請勿使用 CascadingAuthenticationState 元件
builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>(); // F5刷新UI時會無效！
builder.Services.AddHttpContextAccessor();
/// ※ 不要在前端元件使用 IHttpContextAccesser 因為 HttpContext 的狀態並非即時反應的，
/// 成因是 SignalR 與 HTTP 是不同的通訊協定且原則上不能共存。
/// 但 Blazor Server App 又支援 Cookie Authentication。
/// 改用 AuthenticationStateProvider 機制取登入狀態。

//## for BLAZOR COOKIE Auth
/// ref → https://blazorhelpwebsite.com/ViewBlogPost/36
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

//§§ 註冊：客製服務
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

//§§ HTTP 400-599 錯誤處理 
/// 根據預設，ASP.NET Core 應用程式不會提供 HTTP 錯誤狀態碼 (例如「404 - 找不到」) 的狀態碼頁面。 
/// 當應用程式設定沒有本文的 HTTP 400-599 錯誤狀態碼時，它會傳回該狀態碼和一個空白的回應本文。 
/// ref→https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0#usestatuscodepages
app.UseStatusCodePagesWithRedirects("/ErrorStatus/{0}");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
