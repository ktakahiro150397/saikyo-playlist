using System.Web;

namespace saikyo_playlist.Helpers
{
    public static class YoutubeHelpers
    {
        /// <summary>
        /// Youtubeの動画URLから、"v="パラメータに設定されている動画IDを返します。
        /// </summary>
        /// <param name="youtubeUrl"></param>
        /// <returns>youtubeのURLではない場合、または動画URLではない場合はnull</returns>
        public static string? GetVideoIdFromUrl(string youtubeUrl)
        {
            if (!youtubeUrl.Contains("www.youtube.com"))
            {
                return null;
            }

            return GetParameter(youtubeUrl, "v");
        }

        /// <summary>
        /// Youtubeの動画URLから、"list="パラメータに設定されている動画IDを返します。
        /// </summary>
        /// <param name="youtubeUrl"></param>
        /// <returns>youtubeのURLではない場合、または再生リストURLではない場合はnull</returns>
        public static string? GetListIdFromUrl(string youtubeUrl)
        {
            if (!youtubeUrl.Contains("www.youtube.com"))
            {
                return null;
            }

            return GetParameter(youtubeUrl, "list");
        }

        /// <summary>
        /// 対象URLから指定のクエリパラメータの値を取得
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static string? GetParameter(string url,string param)
        {
            var urlQueries = HttpUtility.ParseQueryString((new Uri(url)).Query);
            var itemIdFromUrl = "";
            if (urlQueries.AllKeys.Contains(param) && urlQueries.GetValues(param) is not null)
            {
                itemIdFromUrl = urlQueries.GetValues(param)[0];
                return itemIdFromUrl;
            }
            else
            {
                //指定IDが取得できなかった場合
                return null;
            }
        }




    }
}
