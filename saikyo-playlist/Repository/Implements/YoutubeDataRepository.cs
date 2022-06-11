using System.Text.Json.Serialization;
using System.Text.Json;
using saikyo_playlist.Data.Video;
using saikyo_playlist.Data.PlayList;

namespace saikyo_playlist.Repository.Implements
{
    public class YoutubeDataRepository : IYoutubeDataRepository
    {
        private string ApiKey = "";

        private HttpClient _httpClient;

        private const string videoInfoEndPointUrl = "https://www.googleapis.com/youtube/v3/videos";

        private const string playListInfoEndPointUrl = "https://www.googleapis.com/youtube/v3/playlistItems";

        public YoutubeDataRepository(string apiKey)
        {
            ApiKey = apiKey;
            _httpClient = new HttpClient();
        }

        public async Task<YoutubeVideoRetrieveResult?> GetYoutubeVideoInfo(string videoId)
        {

            var requestUrl = GetYoutubeVideoInfoUrl(videoId);
            var msg = await _httpClient.GetStringAsync(requestUrl);

            YoutubeVideoAPIResponseModel? myDeserializedClass;
            if (msg == null)
            {
                myDeserializedClass = null;
                return null;
            }
            else
            {
                myDeserializedClass = JsonSerializer.Deserialize<YoutubeVideoAPIResponseModel>(msg);

                if (myDeserializedClass == null)
                {
                    return null;
                }
                else
                {
                    var ret = new YoutubeVideoRetrieveResult();
                    ret.Url = videoId;
                    ret.Title = myDeserializedClass.items[0].snippet.title;
                    ret.ItemId = myDeserializedClass.items[0].id;

                    return ret;
                }

            }
        }

        public YoutubePlayListAPIResponseModel? GetYoutubePlayListInfo(string playListId)
        {
            var requestUrl = GetYoutubePlayListInfoUrl(playListId, "");

            YoutubePlayListAPIResponseModel? myDeserializedClass = new YoutubePlayListAPIResponseModel();

            //再帰的にAPIへアクセス
            GetYoutubePlayListRecurrsive(ref myDeserializedClass, playListId, "");

            return myDeserializedClass;
        }

        private string GetYoutubeVideoInfoUrl(string videoId)
        {
            var url = $"{videoInfoEndPointUrl}?part=snippet&key={ApiKey}&id={videoId}";
            return url;
        }

        private string GetYoutubePlayListInfoUrl(string playListId, string nextPageToken)
        {
            var url = $"{playListInfoEndPointUrl}?part=snippet&key={ApiKey}&playlistId={playListId}";
            if (nextPageToken != "")
            {
                url += $"&pageToken={nextPageToken}";
            }

            return url;
        }

        private void GetYoutubePlayListRecurrsive(ref YoutubePlayListAPIResponseModel result, string playListId, string nextPageToken)
        {
            var requestUrl = GetYoutubePlayListInfoUrl(playListId, nextPageToken);

            var msg = _httpClient.GetStringAsync(requestUrl).Result;
            if (msg == null)
            {
                //応答なし、アクセス終了
                return;
            }
            else
            {
                YoutubePlayListAPIResponseModel? deserialized = JsonSerializer.Deserialize<YoutubePlayListAPIResponseModel>(msg);
                if (deserialized == null)
                {
                    return;
                }
                result.items.AddRange(deserialized.items);

                if (deserialized.nextPageToken != "")
                {
                    //次のページが存在する
                    GetYoutubePlayListRecurrsive(ref result, playListId, deserialized.nextPageToken);
                }
                else
                {
                    //次のページが存在しない
                    return;
                }

            }

        }
    }

}
