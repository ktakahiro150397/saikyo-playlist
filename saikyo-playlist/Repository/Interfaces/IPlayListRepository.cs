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
        public IEnumerable<PlayListHeadersEntity> GetPlayListHeaderAll(IdentityUser user);

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
        public Task<PlayListOperationResult> AddItemToPlayListAsync(string headerEntityId, PlayListDetailsEntity detail, IdentityUser user);

        /// <summary>
        /// プレイリストの末尾に新しくアイテムを追加します。
        /// </summary>
        /// <param name="header">追加対象のプレイリストヘッダーエンティティ。</param>
        /// <param name="detail">新しく追加するプレイリスト詳細エンティティのリスト。</param>
        /// <param name="user">ログインユーザー</param>
        /// <returns></returns>
        public Task<PlayListOperationResult> AddItemToPlayListAsync(string headerEntityId, IEnumerable<PlayListDetailsEntity> detailList, IdentityUser user);

        /// <summary>
        /// プレイリストから、対象のアイテムを削除します。
        /// </summary>
        /// <param name="header">削除対象のプレイリストヘッダーエンティティ。</param>
        /// <param name="playListDetailId">削除を行うプレイリスト詳細のID。</param>
        /// <param name="user">ログインユーザー</param>
        /// <returns></returns>
        public Task<PlayListOperationResult> RemoveItemFromPlayListAsync(string headerEntityId, string playListDetailId, IdentityUser user);

        /// <summary>
        /// 指定したプレイリスト詳細アイテムの順序を、プレイリスト間で更新します。
        /// 同一順序のアイテムが存在する場合、入れ替えます。
        /// 最大値以上が指定された場合、末尾に移動します。
        /// </summary>
        /// <param name="headerEntityId">更新対象のプレイリストヘッダーエンティティID。</param>
        /// <param name="playListDetailId">更新するプレイリスト詳細のエンティティID。</param>
        /// <param name="user">ログインユーザー</param>
        /// <param name="itemSeq">更新後のアイテム順序。0から始まります。</param>
        /// <returns></returns>
        public Task<PlayListOperationResult> UpdatePlayListItemSeqAsync(string headerEntityId, string playListDetailId, int itemSeq, IdentityUser user);

        /// <summary>
        /// 対象のプレイリスト情報を取得します。
        /// </summary>
        /// <param name="headerEntityId"></param>
        /// <param name="user">ログインユーザー</param>
        /// <returns></returns>
        public Task<PlayListOperationResult> GetPlayListAsync(string headerEntityId, IdentityUser user);


    }
}
