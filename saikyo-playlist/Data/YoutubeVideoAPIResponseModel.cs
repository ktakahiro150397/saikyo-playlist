namespace saikyo_playlist.Data.Video
{

    /// <summary>
    /// YoutubeからAPI経由でデータを取得した結果を表します。
    /// </summary>
    public class YoutubeVideoRetrieveOperationResult
    {

        /// <summary>
        /// 取得したオブジェクト。
        /// </summary>
        public IList<YoutubeVideoRetrieveResult>? RetrieveResult;

        /// <summary>
        /// APIから取得した結果。
        /// </summary>
        public YoutubeAPIRetrieveOperationResultType OperationResult;

        /// <summary>
        /// エラーが発生している場合、その例外オブジェクト。
        /// </summary>
        public Exception? Exception { get; set; }
    }


    /// <summary>
    /// YoutubeからAPI経由で取得したデータ。
    /// </summary>
    public class YoutubeVideoRetrieveResult
    {
        /// <summary>
        /// Youtube動画のID。
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// タイトル。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 動画へのURL。
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 再生リスト内のインデックス。
        /// </summary>
        public int? ItemSeq { get; set; }

        public YoutubeVideoRetrieveResult()
        {
            ItemId = "";
            Title = "";
            Url = "";
        }

    }

    /// <summary>
    /// YoutubeAPIへの操作結果を表します。
    /// </summary>
    public enum YoutubeAPIRetrieveOperationResultType
    {
        /// <summary>
        /// 操作に成功
        /// </summary>
        Success,

        /// <summary>
        /// (取得時)対象アイテムが存在しない
        /// </summary>
        NotFound,

        /// <summary>
        /// 正しくないURLが入力されている
        /// </summary>
        InvalidUrl,

        /// <summary>
        /// 操作時に予期せぬエラー
        /// </summary>
        UnExpectedError,

    }




    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class YoutubeVideoAPIResponseModel
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public List<Item> items { get; set; }
        public PageInfo pageInfo { get; set; }
    }

    public class Default
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class High
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Item
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public Snippet snippet { get; set; }
    }

    public class Localized
    {
        public string title { get; set; }
        public string description { get; set; }
    }

    public class Maxres
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Medium
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class PageInfo
    {
        public int totalResults { get; set; }
        public int resultsPerPage { get; set; }
    }



    public class Snippet
    {
        public DateTime publishedAt { get; set; }
        public string channelId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Thumbnails thumbnails { get; set; }
        public string channelTitle { get; set; }
        public List<string> tags { get; set; }
        public string categoryId { get; set; }
        public string liveBroadcastContent { get; set; }
        public Localized localized { get; set; }
    }

    public class Standard
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Thumbnails
    {
        public Default @default { get; set; }
        public Medium medium { get; set; }
        public High high { get; set; }
        public Standard standard { get; set; }
        public Maxres maxres { get; set; }
    }
}
