using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using saikyo_playlist.Data;
using saikyo_playlist.Repository.Interfaces;

namespace saikyo_playlist.Repository.Implements
{
    public class ItemLibraryRepository : RepositoryBase, IItemLibraryRepository
    {

        private ApplicationDbContext dbContext;
        //private IdentityUser user;

        public ItemLibraryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            //this.user = user;
        }

        public async Task<ItemLibraryOperationResult> AddPlayCount(string libraryEntityId, int addCount = 1)
        {
            var ret = new ItemLibraryOperationResult();

            try
            {
                var addTarget = dbContext.ItemLibraries
                    .Where(item => item.ItemLibrariesEntityId == libraryEntityId)
                    .FirstOrDefault();

                if (addTarget != null)
                {
                    //加算対象が存在
                    addTarget.PlayCount += addCount;
                    dbContext.Entry(addTarget).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();

                    ret.OperationResult = ItemLibraryOperationResultType.Success;
                    ret.PlayCount = addTarget.PlayCount;
                }
                else
                {
                    ret.OperationResult = ItemLibraryOperationResultType.NotFound;
                }
                

            }catch(Exception ex)
            {
                ret.OperationResult = ItemLibraryOperationResultType.UnExpectedError;
                ret.Exception = ex;
            }

            return ret;
        }

        public async Task<ItemLibraryOperationResult> DeleteAsync(string libraryEntityId, IdentityUser user)
        {
            var ret = new ItemLibraryOperationResult();

            try
            {
                var delTarget = await dbContext.ItemLibraries
                    .Where(item => item.ItemLibrariesEntityId == libraryEntityId && item.AspNetUserdId == user.Id)
                    .FirstOrDefaultAsync();

                if (delTarget != null)
                {
                    //削除対象が存在
                    dbContext.ItemLibraries.Remove(delTarget);
                    await dbContext.SaveChangesAsync();

                    ret.OperationResult = ItemLibraryOperationResultType.Success;
                }
                else
                {
                    ret.OperationResult = ItemLibraryOperationResultType.NotFound;
                }

            }
            catch (Exception ex)
            {
                ret.OperationResult = ItemLibraryOperationResultType.UnExpectedError;
                ret.Exception = ex;
            }

            return ret;
        }

        public async Task<IEnumerable<ItemLibrariesEntity>> GetAllAsync(IdentityUser user)
        {
            var ret = await dbContext.ItemLibraries
                .Where(item => item.AspNetUserdId == user.Id)
                .OrderByDescending(item => item.ItemAddDate)
                .ToListAsync();

            return ret;
        }

        public async Task<ItemLibraryOperationResult> InsertAsync(LibraryItemPlatform platform, string itemId, string title, IdentityUser user)
        {
            var ret = new ItemLibraryOperationResult();

            try
            {
                //ItemId・プラットフォームで重複があるかどうかを確認
                var dupItem = dbContext.ItemLibraries
                    .Where(item => item.ItemId == itemId && item.Platform == platform && item.AspNetUserdId == user.Id)
                    .FirstOrDefault();

                if(dupItem != null)
                {
                    //重複あり
                    ret.OperationResult = ItemLibraryOperationResultType.Duplicate;
                }
                else
                {
                    var addItem = new ItemLibrariesEntity()
                    {
                        ItemLibrariesEntityId = GetUniqueId(),
                        ItemId = itemId,
                        Title = title,
                        AspNetUserdId = user.Id,
                        Platform = platform,
                        PlayCount = 0,
                        ItemAddDate = DateTime.Now,
                    };

                    dbContext.ItemLibraries.Add(addItem);
                    dbContext.SaveChanges();

                    ret.OperationResult = ItemLibraryOperationResultType.Success;
                }

                
            }
            catch (Exception ex)
            {
                ret.OperationResult = ItemLibraryOperationResultType.UnExpectedError;
                ret.Exception = ex;
            }

            return ret;
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

        /// <summary>
        /// 加算後の再生回数。
        /// </summary>
        public int? PlayCount { get; set; }
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
