using Microsoft.AspNetCore.Identity;
using saikyo_playlist.Data;
using System.Web;

namespace saikyo_playlist.Repository
{
    public class ItemLibraryRepository
    {

        private ApplicationDbContext dbContext;
        private IdentityUser user;

        public ItemLibraryRepository(ApplicationDbContext dbContext, IdentityUser user)
        {
            this.dbContext = dbContext;
            this.user = user;
        }


        /// <summary>
        /// ItemLibrariesテーブルにデータを登録し、インサートしたデータを返します。
        /// すでにデータが存在する場合、そのデータを取得します。
        /// </summary>
        /// <param name="platform">アイテムのプラットフォーム。</param>
        /// <param name="itemId">アイテムのID。</param>
        /// <param name="title">アイテムのタイトル。</param>
        /// <returns></returns>
        public ItemLibrariesEntity InsertOrRetrieve(LibraryItemPlatform platform,string itemId,string title)
        {
            throw new NotImplementedException();
        }



    }
}
