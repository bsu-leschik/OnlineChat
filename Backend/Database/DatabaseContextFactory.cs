using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Database;
public class DatabaseContextFactory : IDesignTimeDbContextFactory<Database>
{
    public Database CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<Database>();
        optionsBuilder.UseSqlServer(@"Server=.;Encrypt=False;Database=ChatDb;Trusted_Connection=True;MultipleActiveResultSets=True");

        return new Database(optionsBuilder.Options);
    }
}