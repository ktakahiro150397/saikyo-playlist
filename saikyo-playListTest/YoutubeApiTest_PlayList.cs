//using Microsoft.Extensions.Configuration;
//using saikyo_playlist.Repository.Implements;

//namespace saikyo_playListTest
//{
//    [TestClass]
//    public class YoutubeApiTest_PlayList
//    {
//        private YoutubeDataRepository _repo { get; set; }

//        private IConfiguration Configuration { get; set; }


//        public YoutubeApiTest_PlayList()
//        {
//            Configuration = new ConfigurationBuilder().AddUserSecrets<YoutubeApiTest_Video>().Build();
//            _repo = new YoutubeDataRepository(Configuration["YoutubeAPIKey"]);
//        }

//        [TestMethod]
//        public void GetVideoInfoTest_1()
//        {
//            //erげ
//            var playListId = "PLEi4OmzKDGqHXuXLI03esLJzarUzbJVFm";

//            try
//            {
//                var result = _repo.GetYoutubePlayListInfo(playListId);

//                if(result == null)
//                {
//                    Assert.Fail();
//                }
//                Assert.AreEqual(21, result.items.Count);
//            }
//            catch (Exception ex)
//            {
//                Assert.Fail(ex.Message);

//            }
//        }

//        [TestMethod]
//        public void GetVideoInfoTest_2()
//        {
//            //288件
//            var playListId = "PLEL7lWbWu6s5SH0Os8RuYqc_7_5ZcgJ0y";

//            try
//            {
//                var result = _repo.GetYoutubePlayListInfo(playListId);

//                if (result == null)
//                {
//                    Assert.Fail();
//                }
//                Assert.AreEqual(288, result.items.Count);
//            }
//            catch (Exception ex)
//            {
//                Assert.Fail(ex.Message);

//            }
//        }

//        [TestMethod]
//        public void GetVideoInfoTest_3()
//        {
//            //ぜんぶぶち込み太郎
//            var playListId = "PLEL7lWbWu6s6hLK9hhpREBXvNiV3sWOR9";

//            try
//            {
//                var result = _repo.GetYoutubePlayListInfo(playListId);

//                if (result == null)
//                {
//                    Assert.Fail();
//                }
//                Assert.AreEqual(1121, result.items.Count);
//            }
//            catch (Exception ex)
//            {
//                Assert.Fail(ex.Message);

//            }
//        }


//    }
//}