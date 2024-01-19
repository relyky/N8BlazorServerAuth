namespace N8BlazorServerAuth.Models;

record AuthUser
{
  public string UserId { get; init; } = default!;
  public string UserName { get; init; } = default!;
  public string[] Roles { get; init; } = default!;
  public Guid AuthGuid { get; set; }
  public DateTimeOffset IssuedUtc { get; set; }
  public DateTimeOffset ExpiresUtc { get; set; }
  public bool RememberMe { get; set; }
}
