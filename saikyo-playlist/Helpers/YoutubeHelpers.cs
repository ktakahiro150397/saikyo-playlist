using System.Web;

namespace saikyo_playlist.Helpers
{
    public static class YoutubeHelpers
    {
        /// <summary>
        /// Youtubeの動画URLから、"v="パラメータに設定されている動画IDを返します。
        /// </summary>
        /// <param name="youtubeUrl"></param>
        /// <returns></returns>
        public static string? GetVideoIdFromUrl(string youtubeUrl)
        {
            var urlQueries = HttpUtility.ParseQueryString((new Uri(youtubeUrl)).Query);
            var itemIdFromUrl = "";
            if (urlQueries.AllKeys.Contains("v") && urlQueries.GetValues("v") is not null)
            {
                itemIdFromUrl = urlQueries.GetValues("v")[0];
                return itemIdFromUrl;
            }
            else
            {
                //動画IDが取得できなかった場合
                return null;
            }
        }


    }
}
