using Microsoft.EntityFrameworkCore;
using QTip.Api.Database.Entities;

namespace QTip.API.Database;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
        
    }
    
    public DbSet<PiiVault> PiiVault => Set<PiiVault>();
    public DbSet<TokenSubmission> Submissions => Set<TokenSubmission>();
}  