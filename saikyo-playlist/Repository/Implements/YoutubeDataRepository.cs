using System.Text.Json.Serialization;
using System.Text.Json;
using saikyo_playlist.Data.Video;
using saikyo_playlist.Data.PlayList;
using saikyo_playlist.Repository.Interfaces;
using saikyo_playlist.Helpers;

namespace saikyo_playlist.Repository.Implements
{
    public class YoutubeDataRepository : IYoutubeDataRepository
    {
        private string ApiKey = "";

        private HttpClient _httpClient;

        private const string videoInfoEndPointUrl = "https://www.googleapis.com/youtube/v3/videos";

        private const string playListInfoEndPointUrl = "https://www.googleapis.com/youtube/v3/playlistItems";

        public YoutubeDataRepository(IConfiguration config)
        {
            ApiKey = config["YoutubeAPIKey"];
            _httpClient = new HttpClient();
        }

        private async Task<YoutubeVideoRetrieveResult?> GetYoutubeVideoInfo(string videoId)
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

                    if(myDeserializedClass.items.Count != 0)
                    {
                        var ret = new YoutubeVideoRetrieveResult();
                        ret.Url = $"https://www.youtube.com/watch?v={videoId}";
                        ret.Title = myDeserializedClass.items[0].snippet.title;
                        ret.ItemId = myDeserializedClass.items[0].id;
                        return ret;
                    }
                    else
                    {
                        //結果なし
                        return null;
                    }
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

        private void GetYoutubePlayListRecurrsive(ref YoutubePlayListAPIResponseModel? result, string playListId, string nextPageToken)
        {
            var requestUrl = GetYoutubePlayListInfoUrl(playListId, nextPageToken);

            string msg = null;
            HttpResponseMessage response = _httpClient.GetAsync(requestUrl).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                //プレイリストが存在しない
                result = null;
                return;
            }else if(response.StatusCode != System.Net.HttpStatusCode.OK) {
                //その他のエラー
                return;
            }
            else
            {
                //取得できた場合、結果を読み取る
                msg = response.Content.ReadAsStringAsync().Result;
            }
            
            if (String.IsNullOrEmpty(msg))
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

        public async Task<YoutubeVideoRetrieveOperationResult?> GetYoutubeVideoInfoAsync(string url)
        {

            if (String.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URLを指定してください。");
            }
            
            var ret = new YoutubeVideoRetrieveOperationResult();
            try
            {
                var videoIdFromUrl = YoutubeHelpers.GetVideoIdFromUrl(url);
                if (videoIdFromUrl == null)
                {
                    ret.OperationResult = YoutubeAPIRetrieveOperationResultType.InvalidUrl;
                    throw new ApplicationException("URLが正しくありません。Youtubeの動画URLを指定してください。");
                }
                else
                {
                    var videoInfo = await GetYoutubeVideoInfo(videoIdFromUrl);

                    if (videoInfo == null)
                    {
                        ret.OperationResult = YoutubeAPIRetrieveOperationResultType.NotFound;
                        throw new ApplicationException("指定されたURLの動画は存在しません。");
                    }
                    else
                    {

                        ret.OperationResult = YoutubeAPIRetrieveOperationResultType.Success;

                        var retrieveResult = new YoutubeVideoRetrieveResult()
                        {
                            ItemId = videoInfo.ItemId,
                            ItemSeq = videoInfo.ItemSeq,
                            Title = videoInfo.Title,
                            Url = videoInfo.Url,
                        };
                        ret.RetrieveResult = new List<YoutubeVideoRetrieveResult>();
                        ret.RetrieveResult.Add(retrieveResult);
                    }
                }
            }catch(ApplicationException appEx)
            {
                ret.Exception = appEx;
            }
            catch (Exception ex)
            {
                ret.OperationResult = YoutubeAPIRetrieveOperationResultType.UnExpectedError;
                ret.Exception = ex;
            }

            return ret;
        }

        async Task<YoutubeVideoRetrieveOperationResult?> IYoutubeDataRepository.GetYoutubePlayListInfoAsync(string url)
        {

            if (String.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URLを指定してください。");
            }

            var ret = new YoutubeVideoRetrieveOperationResult();
            try
            {
                var listIdFromUrl = YoutubeHelpers.GetListIdFromUrl(url);
                if(listIdFromUrl == null)
                {
                    ret.OperationResult = YoutubeAPIRetrieveOperationResultType.InvalidUrl;
                    throw new ApplicationException("URLが正しくありません。Youtubeの再生リストURLを指定してください。");
                }


                var playListInfo = GetYoutubePlayListInfo(listIdFromUrl);
                if(playListInfo == null)
                {
                    ret.OperationResult = YoutubeAPIRetrieveOperationResultType.NotFound;
                    throw new ApplicationException("指定されたURLの再生リストは存在しません。");
                }
                else
                {

                    //削除済み動画を除く
                    playListInfo.items = playListInfo.items.Where(item => item.snippet.title != "Deleted video" && item.snippet.description != "This video is unavailable.").ToList();
                    //非公開動画を除く
                    playListInfo.items = playListInfo.items.Where(item => item.snippet.title != "Private video" && item.snippet.description != "This video is private.").ToList();

                    if(playListInfo.items.Count > 0)
                    {
                        //プレイリストが存在
                        ret.RetrieveResult = new List<YoutubeVideoRetrieveResult>();
                        foreach (var item in playListInfo.items.Select((elem,index) => new {Value = elem,Index = index}))
                        {



                            ret.RetrieveResult.Add(
                                new YoutubeVideoRetrieveResult()
                                {
                                    ItemId = item.Value.snippet.resourceId.videoId,
                                    ItemSeq = item.Index,
                                    Title = item.Value.snippet.title,
                                    Url = $"https://www.youtube.com/watch?v={item.Value.snippet.resourceId.videoId}",
                                }
                            );
                        }
                    }
                    else
                    {
                        //すべて削除済みか非公開
                        ret.OperationResult = YoutubeAPIRetrieveOperationResultType.NotFound;
                        throw new ApplicationException("再生リストに動画が含まれていません。");
                    }
                    

                }
                ret.OperationResult = YoutubeAPIRetrieveOperationResultType.Success;

            }
            catch(ApplicationException appEx)
            {
                ret.Exception = appEx;

            }
            catch(Exception ex)
            {
                ret.OperationResult = YoutubeAPIRetrieveOperationResultType.UnExpectedError;
                ret.Exception = ex;
            }

            return ret;

        }
    }

}
