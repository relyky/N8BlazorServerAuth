using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace N8BlazorServerAuth.Services;

internal class CustomAuthenticationStateProvider(IHttpContextAccessor _http) : AuthenticationStateProvider
{
  public override Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    var userIdentity = _http.HttpContext?.User.Identity as ClaimsIdentity;
    if (userIdentity == null)
    {
      var anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
      return Task.FromResult(anonymous);
    }

    // 加值
    userIdentity.AddClaim(new Claim(ClaimTypes.Role, "MYFORM02"));

    // Success
    return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(userIdentity)));
  }
}
