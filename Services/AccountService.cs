using N8BlazorServerAuth.Models;

namespace N8BlazorServerAuth.Services;

public class AccountService
{
  internal AuthUser? Authenticate(string userCredential)
  {
    var parser = userCredential.Split(':');
    string email = parser[0].Trim();
    string password = parser[1].Trim();
    bool rememberMe = "True".Equals(parser[2]);

    if (password != "user credential")
      return null;

    if(email == "smart@mail.server")
    {
      return new AuthUser
      {
        UserId = "smart",
        UserName = "郝聰明",
        AuthGuid = Guid.NewGuid(),
        IssuedUtc = DateTime.UtcNow,
        ExpiresUtc = DateTime.UtcNow.AddMonths(5),
        Roles = ["Admin"],
        RememberMe = rememberMe,
      };
    }
    else if(email == "beauty@mail.server")
    {
      return new AuthUser
      {
        UserId = "beauty",
        UserName = "甄美麗",
        AuthGuid = Guid.NewGuid(),
        IssuedUtc = DateTime.UtcNow,
        ExpiresUtc = DateTime.UtcNow.AddMonths(5),
        Roles = ["Admin"],
        RememberMe = rememberMe,
      };
    }

    // FAIL!
    return null;
  }
}
