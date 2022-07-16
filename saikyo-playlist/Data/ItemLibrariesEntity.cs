using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace saikyo_playlist.Data
{

    /// <summary>
    /// ItemLibrariesに対応するエンティティクラス。
    /// </summary>
    public class ItemLibrariesEntity
    {
        /// <summary>
        /// ライブラリアイテムごとに採番されるユニークなID。
        /// </summary>
        [Comment("ライブラリアイテムごとに採番されるユニークなID。")]
        public string ItemLibrariesEntityId { get; set; }

        /// <summary>
        /// このアイテムを所有しているユーザーのID。
        /// </summary>
        [Comment("このアイテムを所有しているユーザーのID。")]
        public string AspNetUserdId { get; set; }

        /// <summary>
        /// プレイリストのプラットフォーム種別。
        /// </summary>
        [Comment("プレイリストのプラットフォーム種別。")]
        public LibraryItemPlatform Platform { get; set; }

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
        /// アイテムが最後まで再生された数。
        /// </summary>
        [Comment("アイテムが最後まで再生された数。")]
        public int PlayCount { get; set; }

        /// <summary>
        /// アイテムがライブラリに追加された日付。
        /// </summary>
        [Comment("アイテムがライブラリに追加された日付")]
        public DateTime ItemAddDate { get; set; }

        /// <summary>
        /// タイムスタンプ。
        /// </summary>
        [Comment("タイムスタンプ。")]
        [Timestamp]
        public byte[] TimeStamp { get; set; }

        /// <summary>
        /// このアイテムのサムネイル画像URL。
        /// </summary>
        public string ItemThumbNailUrl
        {
            get
            {
                switch (Platform)
                {
                    case LibraryItemPlatform.Youtube:
                        return $"https://i.ytimg.com/vi/{ItemId}/mqdefault.jpg";
                    default:
                        return "";
                }
            }
        }

        public ItemLibrariesEntity()
        {
            ItemLibrariesEntityId = "";
            AspNetUserdId = "";
            Platform = LibraryItemPlatform.NotSet;
            ItemId = "";
            Title = "";
            TitleAlias = "";
            PlayCount = 0;
            TimeStamp = Array.Empty<byte>();
        }


    }
}
