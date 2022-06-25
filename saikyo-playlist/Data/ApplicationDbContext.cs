using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace saikyo_playlist.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        /// <summary>
        /// アイテムライブラリ。
        /// </summary>
        public virtual DbSet<ItemLibrariesEntity> ItemLibraries => Set<ItemLibrariesEntity>();
        
        /// <summary>
        /// プレイリストヘッダー。
        /// </summary>
        public virtual DbSet<PlayListHeadersEntity> PlayListHeaders => Set<PlayListHeadersEntity>();
        
        /// <summary>
        /// プレイリスト詳細。
        /// </summary>
        public virtual DbSet<PlayListDetailsEntity> PlayListDetails => Set<PlayListDetailsEntity>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext():base()
        {
        }

    }
}