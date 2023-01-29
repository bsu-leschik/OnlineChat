using Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Database;
public class DatabaseContextFactory : IDesignTimeDbContextFactory<ChatDatabase>
{
    public ChatDatabase CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ChatDatabase>();
        optionsBuilder.UseSqlServer(ConnectionStrings.SqlConnectionString);

        return new ChatDatabase(optionsBuilder.Options);
    }
}