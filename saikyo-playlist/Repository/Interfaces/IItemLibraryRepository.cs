using Microsoft.AspNetCore.Identity;
using saikyo_playlist.Data;
using saikyo_playlist.Repository.Implements;
using System.Collections.Generic;
using System.Web;

namespace saikyo_playlist.Repository.Interfaces
{
    public interface IItemLibraryRepository
    {

        /// <summary>
        /// ItemLibrariesテーブルにデータを登録し、インサートしたデータを返します。
        /// すでにデータが存在する場合、そのデータを取得します。
        /// </summary>
        /// <param name="platform">アイテムのプラットフォーム。</param>
        /// <param name="itemId">アイテムのID。</param>
        /// <param name="title">アイテムのタイトル。</param>
        [Obsolete("これ要る?")]
        public Task<ItemLibrariesEntity> InsertOrRetrieveAsync(LibraryItemPlatform platform, string itemId, string title);

        /// <summary>
        /// ItemLibrariesテーブルにデータを登録し、その結果を返します。
        /// </summary>
        /// <param name="platform">アイテムのプラットフォーム。</param>
        /// <param name="itemId">アイテムのID。</param>
        /// <param name="title">アイテムのタイトル。</param>
        /// <param name="user">登録するユーザー。</param>
        /// <returns></returns>
        public Task<ItemLibraryOperationResult> InsertAsync(LibraryItemPlatform platform, string itemId, string title, IdentityUser user);

        /// <summary>
        /// ログインユーザーのライブラリをすべて取得します。
        /// </summary>
        /// <param name="user">登録するユーザー。</param>
        /// <returns></returns>
        public Task<IEnumerable<ItemLibrariesEntity>> GetAllAsync(IdentityUser user);

        /// <summary>
        /// 指定したIDのアイテムをライブラリから削除します。
        /// </summary>
        /// <param name="libraryEntityId">削除するライブラリID。</param>
        /// <param name="user">データを削除するユーザー。</param>
        /// <returns></returns>
        public Task<ItemLibraryOperationResult> DeleteAsync(string libraryEntityId, IdentityUser user);


    }
}
