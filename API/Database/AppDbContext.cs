using Microsoft.EntityFrameworkCore;
using QTip.Api.Database.Entities;

namespace QTip.API.Database;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
        
    }
    
    public DbSet<Vault> PiiVault => Set<Vault>();
    public DbSet<Submission> Submissions => Set<Submission>();
}  