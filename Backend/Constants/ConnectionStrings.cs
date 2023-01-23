using Microsoft.Extensions.Configuration;

namespace Constants;

public static class ConnectionStrings
{
    public static readonly string SqlConnectionString = LoadConnectionString("SqlConnectionString")!;

    private static string? LoadConnectionString(string str)
    {
        var builder = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json");
        var config = builder.Build();
        return config.GetConnectionString(str);
    }
}