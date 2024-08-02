using Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext: DbContext
{
    public DbSet<User> Users { get; set; } = default!;

    public DataContext(DbContextOptions<DataContext> options) 
        : base(options)
    {
    }
}
