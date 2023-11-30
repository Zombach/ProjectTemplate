using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext
{


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}