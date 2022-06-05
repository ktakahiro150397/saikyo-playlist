using System.Text.Json.Serialization;
using System.Text.Json;
using saikyo_playlist.Data;

namespace saikyo_playlist.Repository
{
    public class YoutubeDataRepository
    {

        private string ApiKey = "";

        private HttpClient _httpClient;

        private const string videoInfoEndPointUrl = "https://www.googleapis.com/youtube/v3/videos";

        public YoutubeDataRepository(string apiKey)
        {
            ApiKey = apiKey;
            _httpClient = new HttpClient();
        }

        public async Task<YoutubeAPIResponseModel?> GetYoutubeVideoInfo(string videoId)
        {

            var requestUrl = GetYoutubeVideoInfoUrl(videoId);
            var msg = await _httpClient.GetStringAsync(requestUrl);

            YoutubeAPIResponseModel? myDeserializedClass;
            if (msg == null)
            {
                myDeserializedClass = null;
            }
            else
            {
                myDeserializedClass = JsonSerializer.Deserialize<YoutubeAPIResponseModel>(msg);
            }

            return myDeserializedClass;
        }

        private string GetYoutubeVideoInfoUrl(string videoId)
        {
            var url = $"{videoInfoEndPointUrl}?part=snippet&key={ApiKey}&id={videoId}";
            return url;
        }









    }












}
