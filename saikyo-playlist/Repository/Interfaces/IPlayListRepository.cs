using saikyo_playlist.Repository.Implements;
using saikyo_playlist.Data;
using Microsoft.AspNetCore.Identity;

namespace saikyo_playlist.Repository.Interfaces
{

    /// <summary>
    /// プレイリストの取得に関する操作を提供します。
    /// </summary>
    public interface IPlayListRepository
    {
        /// <summary>
        /// ログインユーザーのプレイリストヘッダーをすべて取得します。
        /// </summary>
        /// <param name="user">ログインユーザー</param>
        /// <returns></returns>
        public Task<IEnumerable<PlayListHeadersEntity>> GetPlayListHeaderAll(IdentityUser user);

        /// <summary>
        /// プレイリストの新規作成を行います。
        /// </summary>
        /// <param name="playListName">プレイリストの名前。</param>
        /// <param name="user">ログインユーザー</param>
        /// <returns></returns>
        public Task<PlayListOperationResult> CreateNewPlayListAsync(string playListName, IdentityUser user);

        /// <summary>
        /// プレイリストに新しくアイテムを追加します。
        /// </summary>
        /// <param name="header">追加対象のプレイリストヘッダーエンティティ。</param>
        /// <param name="detail">新しく追加するプレイリスト詳細エンティティ。</param>
        /// <param name="user">ログインユーザー</param>
        /// <returns></returns>
        public Task<PlayListOperationResult> AddItemToPlayListAsync(PlayListHeadersEntity header, PlayListDetailsEntity detail, IdentityUser user);

        /// <summary>
        /// プレイリストの末尾に新しくアイテムを追加します。
        /// </summary>
        /// <param name="header">追加対象のプレイリストヘッダーエンティティ。</param>
        /// <param name="detail">新しく追加するプレイリスト詳細エンティティのリスト。</param>
        /// <param name="user">ログインユーザー</param>
        /// <returns></returns>
        public Task<PlayListOperationResult> AddItemToPlayListAsync(PlayListHeadersEntity header, IEnumerable<PlayListDetailsEntity> detailList, IdentityUser user);

        /// <summary>
        /// プレイリストから、対象のアイテムを削除します。
        /// </summary>
        /// <param name="header">削除対象のプレイリストヘッダーエンティティ。</param>
        /// <param name="playListDetailId">削除を行うプレイリスト詳細のID。</param>
        /// <param name="user">ログインユーザー</param>
        /// <returns></returns>
        public Task<PlayListOperationResult> RemoveItemFromPlayListAsync(PlayListHeadersEntity header, string playListDetailId, IdentityUser user);

        /// <summary>
        /// プレイリストの対象のアイテムを更新します。
        /// </summary>
        /// <param name="header">更新対象のプレイリストヘッダーエンティティ。</param>
        /// <param name="playListDetailId">更新するプレイリスト詳細のエンティティ。</param>
        /// <param name="user">ログインユーザー</param>
        /// <returns></returns>
        public Task<PlayListOperationResult> UpdatePlayListItemAsync(PlayListHeadersEntity header, PlayListDetailsEntity detail, IdentityUser user);

        /// <summary>
        /// 対象のプレイリスト情報を取得します。
        /// </summary>
        /// <param name="headerEntityId"></param>
        /// <param name="user">ログインユーザー</param>
        /// <returns></returns>
        public Task<GetPlayListOperationResult> GetPlayListAsync(string headerEntityId, IdentityUser user);


    }
}
