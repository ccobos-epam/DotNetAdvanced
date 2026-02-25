namespace SharedClasses.OptionsPattern;

public class PostgreSqlUsersOptions
{
  private const string BasePath = "PostgreSqlUsers";
  public const string AdminPath = $"{BasePath}/Admin";
  public const string DocumentsPath = $"{BasePath}/DocumentsDb";
  public const string RelationalPath = $"{BasePath}/RelationalDb";
  public const string UsersPath = $"{BasePath}/UserManagement";
  
  public string UserId { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string Schema { get; set; } = null!;
}