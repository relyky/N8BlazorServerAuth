using N8BlazorServerAuth.Models;

namespace N8BlazorServerAuth.Services;

public class AccountService
{
  internal AuthUser? Authenticate(string userCredential)
  {
    if (userCredential != "user credential")
      return null;

    return new AuthUser
    {
      UserId = "smart",
      UserName = "郝聰明",
      AuthGuid = Guid.NewGuid(),
      IssuedUtc = DateTime.UtcNow,
      ExpiresUtc = DateTime.UtcNow.AddMonths(5),
      Roles = ["Admin"],
      RememberMe = false,
    };
  }
}
