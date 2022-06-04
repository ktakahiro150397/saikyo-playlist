using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace saikyo_playlist.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<PlayListHeadersEntity> PlayListHeaders => Set<PlayListHeadersEntity>();
        public DbSet<PlayListDetailsEntity> PlayListDetails => Set<PlayListDetailsEntity>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

    }
}