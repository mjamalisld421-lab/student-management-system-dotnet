using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StudentManagementSystem.Data;

public sealed class SqlServerAppDbContextFactory : IDesignTimeDbContextFactory<SqlServerAppDbContext>
{
    public SqlServerAppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<SqlServerAppDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=StudentManagementSystemDesign;Trusted_Connection=True;")
            .Options;

        return new SqlServerAppDbContext(options);
    }
}
