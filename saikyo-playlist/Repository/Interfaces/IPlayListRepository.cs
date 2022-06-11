using saikyo_playlist.Repository.Implements;
using saikyo_playlist.Data;

namespace saikyo_playlist.Repository.Interfaces
{

    /// <summary>
    /// プレイリストの取得に関する操作を提供します。
    /// </summary>
    public interface IPlayListRepository
    {

        /// <summary>
        /// プレイリストの新規作成を行います。
        /// </summary>
        /// <param name="playListName">プレイリストの名前。</param>
        /// <returns></returns>
        public Task<PlayListOperationResult> CreateNewPlayListAsync(string playListName);

        /// <summary>
        /// プレイリストに新しくアイテムを追加します。
        /// </summary>
        /// <param name="header">追加対象のプレイリストヘッダーエンティティ。</param>
        /// <param name="detail">新しく追加するプレイリスト詳細エンティティ。</param>
        /// <returns></returns>
        public Task<PlayListOperationResult> AddItemToPlayListAsync(PlayListHeadersEntity header, PlayListDetailsEntity detail);

        /// <summary>
        /// プレイリストの末尾に新しくアイテムを追加します。
        /// </summary>
        /// <param name="header">追加対象のプレイリストヘッダーエンティティ。</param>
        /// <param name="detail">新しく追加するプレイリスト詳細エンティティのリスト。</param>
        /// <returns></returns>
        public Task<PlayListOperationResult> AddItemToPlayListAsync(PlayListHeadersEntity header, IEnumerable<PlayListDetailsEntity> detailList);

        /// <summary>
        /// プレイリストから、対象のアイテムを削除します。
        /// </summary>
        /// <param name="header">削除対象のプレイリストヘッダーエンティティ。</param>
        /// <param name="playListDetailId">削除を行うプレイリスト詳細のID。</param>
        /// <returns></returns>
        public Task<PlayListOperationResult> RemoveItemFromPlayList(PlayListHeadersEntity header, string playListDetailId);

        /// <summary>
        /// プレイリストの対象のアイテムを更新します。
        /// </summary>
        /// <param name="header">更新対象のプレイリストヘッダーエンティティ。</param>
        /// <param name="playListDetailId">更新するプレイリスト詳細のエンティティ。</param>
        /// <returns></returns>
        public Task<PlayListOperationResult> RemoveItemFromPlayList(PlayListHeadersEntity header, PlayListDetailsEntity detail);


    }
}
