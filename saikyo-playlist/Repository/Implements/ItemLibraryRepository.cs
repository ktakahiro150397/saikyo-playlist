using Microsoft.AspNetCore.Identity;
using saikyo_playlist.Data;
using saikyo_playlist.Repository.Interfaces;
using System.Web;

namespace saikyo_playlist.Repository.Implements
{
    public class ItemLibraryRepository : IItemLibraryRepository
    {

        private ApplicationDbContext dbContext;
        private UserManager<IdentityUser> user;
        private IConfiguration configuration;

        public ItemLibraryRepository(ApplicationDbContext dbContext, UserManager<IdentityUser> user, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.user = user;
            this.configuration = configuration;
        }

        public Task<ItemLibraryOperationResult> DeleteAsync(string libraryEntityId, IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ItemLibrariesEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ItemLibraryOperationResult> InsertAsync(LibraryItemPlatform platform, string itemId, string title, IdentityUser user)
        {
            throw new NotImplementedException();
        }

        [Obsolete("これ要る?")]
        public Task<ItemLibrariesEntity> InsertOrRetrieveAsync(LibraryItemPlatform platform, string itemId, string title)
        {
            throw new NotImplementedException();
        }

    }

    /// <summary>
    /// ライブラリへの操作結果を表します。
    /// </summary>
    public class ItemLibraryOperationResult
    {

        /// <summary>
        /// 操作結果
        /// </summary>
        public ItemLibraryOperationResultType OperationResult { get; set; }

        /// <summary>
        /// エラーが発生している場合、その例外オブジェクト。
        /// </summary>
        public Exception? Exception { get; set; }
    }
    
    /// <summary>
    /// アイテムライブラリエンティティへの操作結果を表します。
    /// </summary>
    public enum ItemLibraryOperationResultType
    {
        /// <summary>
        /// 操作に成功
        /// </summary>
        Success,

        /// <summary>
        /// (追加時)アイテムが既に存在する
        /// </summary>
        Duplicate,

        /// <summary>
        /// (取得・削除時)対象アイテムが存在しない
        /// </summary>
        NotFound,

        /// <summary>
        /// 操作時に予期せぬエラー
        /// </summary>
        UnExpectedError,

    }

}
