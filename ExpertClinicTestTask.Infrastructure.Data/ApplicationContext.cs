using ExpertClinicTestTask.Domain.Core.Account;
using Microsoft.EntityFrameworkCore;

namespace ExpertClinicTestTask.Infrastructure.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {            
        base.OnModelCreating(builder);
    }
    
    public DbSet<User> Users { get; set; }
}