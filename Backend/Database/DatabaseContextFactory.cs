using Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Database;
public class DatabaseContextFactory : IDesignTimeDbContextFactory<Database>
{
    public Database CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<Database>();
        optionsBuilder.UseSqlServer(ConnectionStrings.SqlConnectionString);

        return new Database(optionsBuilder.Options);
    }
}