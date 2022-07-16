using System.Text.Json.Serialization;
using System.Text.Json;
using saikyo_playlist.Data;

namespace saikyo_playlist.Models
{
    public class PlayListModel
    {

        public string PlayListHeaderId { get; set; }

        public string PlayListName { get; set; }

        public IList<PlayListItem> PlayLists { get; set; }

        /// <summary>
        /// プレイリストの内容をクライアントサイドで解釈できるJSON形式の文字列で返します。
        /// </summary>
        public string PlayListJson
        {
            get
            {
                return JsonSerializer.Serialize(PlayLists);
            }
        }

        /// <summary>
        /// プレイリストを初期化します。
        /// </summary>
        public PlayListModel()
        {
            PlayLists = new List<PlayListItem>();

            //テスト用データの追加
            {
                var item1 = new PlayListItem();
                item1.Type = LibraryItemPlatform.Youtube;
                item1.ItemId = "kLLKnUP9gN4";
                item1.ItemSeq = 0;
                item1.Title = "カスタムメイド3D - Entrance to You";
                item1.TitleAlias = "Entrance to You";
                item1.PlayCount = 0;

                PlayLists.Add(item1);
            }
            {
                var item2 = new PlayListItem();
                item2.Type = LibraryItemPlatform.Youtube;
                item2.ItemId = "MgeIqstCGsw";
                item2.ItemSeq = 1;
                item2.Title = "少女マイノリティ OP 『Minority』 Full 夢乃ゆき";
                item2.TitleAlias = "Minority";
                item2.PlayCount = 0;

                PlayLists.Add(item2);
            }
            {
                var item3 = new PlayListItem();
                item3.Type = LibraryItemPlatform.Youtube;
                item3.ItemId = "ogwfFWFGiRI";
                item3.ItemSeq = 2;
                item3.Title = "Ruri Yakushi / 神様いつかこの想い - 鯨神のティアスティラ ED1【Full】";
                item3.TitleAlias = "神様いつかこの想い";
                item3.PlayCount = 0;

                PlayLists.Add(item3);
            }

        }

        public PlayListModel(ApplicationDbContext dbContext, string playListHeaderId)
        {

            PlayListHeaderId = playListHeaderId;

            var playLists = dbContext.PlayListHeaders
                    .Where(item => item.PlayListHeadersEntityId == PlayListHeaderId)
                    .Join(
                        dbContext.PlayListDetails,
                        header => header.PlayListHeadersEntityId,
                        detail => detail.PlayListHeadersEntityId,
                        (header, detail) =>
                            new
                            {
                                ItemLibraryId = detail.ItemLibrariesEntityId,
                                ItemSeq = detail.ItemSeq
                            }
                        )
                    .Join(
                        dbContext.ItemLibraries,
                        joined => joined.ItemLibraryId,
                        lib => lib.ItemLibrariesEntityId,
                        (joined, lib) =>
                        new PlayListItem()
                        {
                            Type = lib.Platform,
                            ItemLibraryEntityId = lib.ItemLibrariesEntityId,
                            ItemId = lib.ItemId,
                            Title = lib.Title,
                            TitleAlias = lib.TitleAlias,
                            PlayCount = lib.PlayCount,
                            ItemSeq = joined.ItemSeq,
                            ItemThumbnailUrl = lib.ItemThumbNailUrl
                        }
                    )
                    .OrderBy(item => item.ItemSeq)
                    .ToList();

            var header = dbContext.PlayListHeaders
                    .Where(item => item.PlayListHeadersEntityId == playListHeaderId)
                    .FirstOrDefault();

            if (header == null || playLists == null || playLists.Count == 0)
            {
                //対象IDでデータが見つからなかった
                throw new KeyNotFoundException($"プレイリスト情報を取得できませんでした。ID : {playListHeaderId}");
            }
            else
            {
                //取得したプレイリストを設定
                PlayListName = header.Name;
                PlayLists = playLists;
            }

        }

    }

    /// <summary>
    /// 単一のプレイリストアイテムを表します。
    /// </summary>
    public class PlayListItem
    {
        /// <summary>
        /// プラットフォーム種類
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("type")]
        public LibraryItemPlatform Type { get; set; }

        /// <summary>
        /// アイテムライブラリID
        /// </summary>
        [JsonPropertyName("itemLibraryId")]
        public string ItemLibraryEntityId { get; set; }

        /// <summary>
        /// アイテムID
        /// </summary>
        [JsonPropertyName("itemId")]
        public string ItemId { get; set; }

        /// <summary>
        /// アイテム連番。
        /// </summary>
        [JsonPropertyName("itemSeq")]
        public int ItemSeq { get; set; }

        /// <summary>
        /// アイテムの元々の名称。
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// アイテムの別名。
        /// </summary>
        [JsonPropertyName("titleAlias")]
        public string TitleAlias { get; set; }

        /// <summary>
        /// サムネイルの画像
        /// </summary>
        [JsonPropertyName("itemThumbnailUrl")]
        public string ItemThumbnailUrl { get; set; }

        /// <summary>
        /// 再生回数。
        /// </summary>
        [JsonPropertyName("playCount")]
        public int PlayCount { get; set; }

        public PlayListItem()
        {
            ItemId = string.Empty;
            ItemSeq = 0;
            Title = string.Empty;
            TitleAlias = string.Empty;
            PlayCount = 0;
        }
    }

}
