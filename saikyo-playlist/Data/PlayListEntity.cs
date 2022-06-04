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
        public string Id { get; set; }

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
            Id = "";
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
        public string Id { get; set; }

        /// <summary>
        /// 0から始まるプレイリストの連番。
        /// </summary>
        [Comment("0から始まるプレイリストの連番。")]
        public int ItemSeq { get; set; }

        /// <summary>
        /// プレイリストのプラットフォーム種別。
        /// </summary>
        [Comment("プレイリストのプラットフォーム種別。")]
        public string Type { get; set; }

        /// <summary>
        /// アイテムを特定するためのプラットフォームごとのID。
        /// </summary>
        [Comment("アイテムを特定するためのプラットフォームごとのID。")]
        public string ItemId { get; set; }

        /// <summary>
        /// アイテムの元々の名称。
        /// </summary>
        [Comment("アイテムの元々の名称。")]
        public string Title { get; set; }

        /// <summary>
        /// ユーザーによって付けられたアイテムの別名。
        /// </summary>
        [Comment("ユーザーによって付けられたアイテムの別名。")]
        public string TitleAlias { get; set; }

        /// <summary>
        /// アイテムがプレイリスト内で最後まで再生された数。
        /// </summary>
        [Comment("アイテムがプレイリスト内で最後まで再生された数。")]
        public int PlayCount { get; set; }

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
        /// 逆ナビゲーションプロパティ
        /// </summary>
        public PlayListHeadersEntity PlayListHeadersEntity { get; set; }

        public PlayListDetailsEntity()
        {
            Id = "";
            ItemSeq = 0;
            Type = "";
            ItemId = "";
            Title = "";
            TitleAlias = "";
            PlayCount = 0;
            TimeStamp = new byte[0];
            PlayListHeadersEntityId = "";
            PlayListHeadersEntity = new PlayListHeadersEntity();

        }
    }
}
