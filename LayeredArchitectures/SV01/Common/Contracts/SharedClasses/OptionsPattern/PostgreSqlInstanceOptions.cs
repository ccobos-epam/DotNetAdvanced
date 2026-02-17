namespace SharedClasses.OptionsPattern;

public class PostgreSqlInstanceOptions
{
  public string Host { get; set; } = null!;
  public int Port { get; set; }
  public string Database { get; set; }  = null!;
}