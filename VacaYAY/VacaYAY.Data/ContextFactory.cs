using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using VacaYAY.Data;

namespace VacaYAY.Business;

public class ContextFactory : IDesignTimeDbContextFactory<Context>
{
    public Context CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<Context>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=VacaYAY;Trusted_Connection=True;Encrypt=False;");

        return new Context(optionsBuilder.Options);
    }
}
