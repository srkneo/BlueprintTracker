using Microsoft.EntityFrameworkCore;

namespace BlueprintApi;

public class BlueprintDbContext : DbContext
{
    public BlueprintDbContext(DbContextOptions<BlueprintDbContext> options) : base(options) { }

    public DbSet<StudyModule> StudyModules => Set<StudyModule>();
}