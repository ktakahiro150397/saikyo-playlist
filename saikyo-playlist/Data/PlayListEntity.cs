using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace saikyo_playlist.Data
{

    /// <summary>
    /// PlayListHeadersに対応するエンティティクラス。
    /// </summary>
    public class PlayListHeadersEntity
    {
        /// <summary>
        /// プレイリストごとに採番されるユニークなID。
        /// </summary>
        [Comment("プレイリストごとに採番されるユニークなID。")]
        public string PlayListHeadersEntityId { get; set; }

        /// <summary>
        /// このプレイリストを作成したユーザーのID。
        /// </summary>
        [Comment("このプレイリストを作成したユーザーのID。")]
        public string AspNetUserdId { get; set; }

        /// <summary>
        /// プレイリストの名前。
        /// </summary>
        [Comment("プレイリストの名前。")]
        public string Name { get; set; }

        /// <summary>
        /// プレイリストが最後に再生された日付。
        /// 新規作成時は作成日と同じ。
        /// </summary>
        [Comment("プレイリストが最後に再生された日付。")]
        public DateTime LastPlayedDate { get; set; }

        /// <summary>
        /// タイムスタンプ。
        /// </summary>
        [Comment("タイムスタンプ。")]
        [Timestamp]
        public byte[] TimeStamp { get; set; }

        /// <summary>
        /// このプレイリストに紐づくプレイリスト詳細エンティティ。
        /// </summary>
        public IList<PlayListDetailsEntity> Details { get; set; }

        public PlayListHeadersEntity()
        {
            PlayListHeadersEntityId = "";
            AspNetUserdId = "";
            Name = "";
            TimeStamp = new byte[0];
            Details = new List<PlayListDetailsEntity>();
        }
    }


    public class PlayListDetailsEntity
    {

        /// <summary>
        /// プレイリスト詳細ごとに採番されるユニークなID。
        /// </summary>
        [Comment("プレイリスト詳細ごとに採番されるユニークなID。")]
        public string PlayListDetailsEntityId { get; set; }

        /// <summary>
        /// 0から始まるプレイリストの連番。
        /// </summary>
        [Comment("0から始まるプレイリストの連番。")]
        public int ItemSeq { get; set; }

        /// <summary>
        /// タイムスタンプ。
        /// </summary>
        [Comment("タイムスタンプ。")]
        [Timestamp]
        public byte[] TimeStamp { get; set; }

        /// <summary>
        /// 外部キー
        /// </summary>
        public string PlayListHeadersEntityId { get; set; }

        /// <summary>
        /// 外部キー
        /// </summary>
        public string ItemLibrariesEntityId { get; set; }

        /// <summary>
        /// 逆ナビゲーションプロパティ
        /// </summary>
        public PlayListHeadersEntity PlayListHeadersEntity { get; set; }

        public ItemLibrariesEntity ItemLibrariesEntity { get; set; }

        public PlayListDetailsEntity()
        {
            PlayListDetailsEntityId = "";
            ItemSeq = 0;
            TimeStamp = new byte[0];
            PlayListHeadersEntityId = "";
            PlayListHeadersEntity = new PlayListHeadersEntity();
            ItemLibrariesEntityId = "";
            ItemLibrariesEntity = new ItemLibrariesEntity();
        }
    }
}
