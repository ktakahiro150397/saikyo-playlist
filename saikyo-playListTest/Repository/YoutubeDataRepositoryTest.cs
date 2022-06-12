using System;
using saikyo_playlist.Data.Video;
using saikyo_playlist.Repository.Implements;

namespace saikyo_playListTest.Repository
{
	public class YoutubeDataRepositoryTest
	{

		private IYoutubeDataRepository _repo { get; set; }
        private IConfiguration Configuration { get; set; }

		public YoutubeDataRepositoryTest()
		{
            Configuration = new ConfigurationBuilder().AddUserSecrets<YoutubeDataRepositoryTest>().Build();
            _repo = new YoutubeDataRepository(Configuration);
		}

        /// <summary>
        /// 成功
        /// </summary>
        [Fact]
		public async void GetYoutubeVideoInfoAsync_Success()
        {
            //Arrange
            var videoId = "https://www.youtube.com/watch?v=MgeIqstCGsw&list=PLEi4OmzKDGqHXuXLI03esLJzarUzbJVFm&index=1";


            //Act
            var result = await _repo.GetYoutubeVideoInfoAsync(videoId);

            //Assert
            Assert.Equal(YoutubeAPIRetrieveOperationResultType.Success,result.OperationResult);
            Assert.Equal(1,result.RetrieveResult.Count);
            Assert.Equal("少女マイノリティ OP 『Minority』 Full 夢乃ゆき", result.RetrieveResult[0].Title);
            Assert.Equal("MgeIqstCGsw", result.RetrieveResult[0].ItemId);
            Assert.Equal("https://www.youtube.com/watch?v=MgeIqstCGsw", result.RetrieveResult[0].Url);
            Assert.Null(result.RetrieveResult[0].ItemSeq);

        }

        /// <summary>
        /// GetYoutubeVideoInfoAsync 失敗・URLなし
        /// </summary>
        [Fact]
        public async void GetYoutubeVideoInfoAsync_Fail_InvalidURL()
        {
            //Arrange
            var videoId = "";

            //Act
            var result = await _repo.GetYoutubeVideoInfoAsync(videoId);

            //Assert
            Assert.Equal(YoutubeAPIRetrieveOperationResultType.InvalidUrl, result.OperationResult);
            var appEx = Assert.IsType<ApplicationException>(result.Exception);
            Assert.Equal("URLを指定してください。", appEx.Message);
            Assert.Null(result.RetrieveResult);
            
        }

        /// <summary>
        /// GetYoutubeVideoInfoAsync 失敗・youtube以外のURL
        /// </summary>
        [Fact]
        public async void GetYoutubeVideoInfoAsync_Fail_NotYoutubeURL()
        {
            //Arrange
            var videoId = "https://www.Google.com/watch?v=MgeIqstCGsw&list=PLEi4OmzKDGqHXuXLI03esLJzarUzbJVFm&index=1";

            //Act
            var result = await _repo.GetYoutubeVideoInfoAsync(videoId);

            //Assert
            Assert.Equal(YoutubeAPIRetrieveOperationResultType.InvalidUrl, result.OperationResult);
            var appEx = Assert.IsType<ApplicationException>(result.Exception);
            Assert.Equal("URLが正しくありません。Youtubeの動画URLを指定してください。", appEx.Message);
            Assert.Null(result.RetrieveResult);

        }

        /// <summary>
        /// GetYoutubeVideoInfoAsync 失敗・存在しない動画ID
        /// </summary>
        [Fact]
        public async void GetYoutubeVideoInfoAsync_Fail_URLNotFound()
        {
            //Arrange
            var videoId = "https://www.youtube.com/watch?v=hogehogehoge";

            //Act
            var result = await _repo.GetYoutubeVideoInfoAsync(videoId);

            //Assert
            Assert.Equal(YoutubeAPIRetrieveOperationResultType.NotFound, result.OperationResult);
            var appEx = Assert.IsType<ApplicationException>(result.Exception);
            Assert.Equal("指定されたURLの動画は存在しません。", appEx.Message);
            Assert.Null(result.RetrieveResult);

        }

        [Fact]
        /// <summary>
        /// GetYoutubePlayListInfoAsync 成功
        /// </summary>
        public async void GetYoutubePlayListInfoAsync_Success()
        {
            //Arrange
            var playListId = "https://www.youtube.com/watch?v=MgeIqstCGsw&list=PLEi4OmzKDGqHXuXLI03esLJzarUzbJVFm&index=1";

            //Act
            var result = await _repo.GetYoutubePlayListInfoAsync(playListId);

            //Assert
            Assert.Equal(YoutubeAPIRetrieveOperationResultType.Success, result.OperationResult);
            Assert.Null(result.Exception);
            Assert.Equal(21,result.RetrieveResult.Count);

            //中身を確認

            //Minority
            var data0 = result.RetrieveResult[0];

            Assert.Equal("MgeIqstCGsw", data0.ItemId);
            Assert.Equal( "少女マイノリティ OP 『Minority』 Full 夢乃ゆき",data0.Title);
            Assert.Equal("https://www.youtube.com/watch?v=MgeIqstCGsw",data0.Url);
            Assert.Equal(0,data0.ItemSeq);

            //Entrance to you
            var data11 = result.RetrieveResult[11];

            Assert.Equal("kLLKnUP9gN4", data11.ItemId);
            Assert.Equal("カスタムメイド3D - Entrance to You", data11.Title);
            Assert.Equal("https://www.youtube.com/watch?v=kLLKnUP9gN4", data11.Url);
            Assert.Equal(10, data0.ItemSeq);

        }

        [Fact]
        /// <summary>
        /// GetYoutubePlayListInfoAsync 失敗・URLなし
        /// </summary>
        public async void GetYoutubePlayListInfoAsync_Fail_InvalidURL()
        {
            //Arrange
            var playListId = "";

            //Act
            var result = await _repo.GetYoutubePlayListInfoAsync(playListId);

            //Assert
            Assert.Equal(YoutubeAPIRetrieveOperationResultType.InvalidUrl, result.OperationResult);
            var appEx = Assert.IsType<ApplicationException>(result.Exception);
            Assert.Equal("URLを指定してください。", appEx.Message);
            Assert.Null(result.RetrieveResult);

        }

        [Fact]
        /// <summary>
        /// GetYoutubePlayListInfoAsync 失敗・youtube以外のURL
        /// </summary>
        public async void GetYoutubePlayListInfoAsync_Fail_NotYoutubeURL()
        {
            //Arrange
            var playListId = "https://www.youtube.com/watch?v=kLLKnUP9gN4&list=hogehogehoge&index=12";

            //Act
            var result = await _repo.GetYoutubePlayListInfoAsync(playListId);

            //Assert
            Assert.Equal(YoutubeAPIRetrieveOperationResultType.InvalidUrl, result.OperationResult);
            var appEx = Assert.IsType<ApplicationException>(result.Exception);
            Assert.Equal("URLが正しくありません。Youtubeの再生リストURLを指定してください。", appEx.Message);
            Assert.Null(result.RetrieveResult);

        }

        [Fact]
        /// <summary>
        /// GetYoutubePlayListInfoAsync 失敗・存在しないURL
        /// </summary>
        public async void GetYoutubePlayListInfoAsync_Fail__Fail_URLNotFound()
        {
            //Arrange
            var playListId = "https://www.youtube.com/watch?v=kLLKnUP9gN4&list=hogehogehoge&index=12";

            //Act
            var result = await _repo.GetYoutubePlayListInfoAsync(playListId);

            //Assert
            Assert.Equal(YoutubeAPIRetrieveOperationResultType.NotFound, result.OperationResult);
            var appEx = Assert.IsType<ApplicationException>(result.Exception);
            Assert.Equal("指定されたURLの動画は存在しません。", appEx.Message);
            Assert.Null(result.RetrieveResult);

        }

        


    }
}

