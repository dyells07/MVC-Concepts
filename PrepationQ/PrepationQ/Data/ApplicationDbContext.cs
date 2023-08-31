
using Microsoft.EntityFrameworkCore;
using PrepationQ.Models;

namespace PrepationQ.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
}