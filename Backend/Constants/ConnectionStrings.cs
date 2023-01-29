using Microsoft.Extensions.Configuration;

namespace Constants;

public static class ConnectionStrings
{
    static ConnectionStrings()
    {
        var builder = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json");
        var config = builder.Build();
        SqlConnectionString = config.GetConnectionString("SqlConnectionString")!;
    }
    
    public static readonly string SqlConnectionString;
}