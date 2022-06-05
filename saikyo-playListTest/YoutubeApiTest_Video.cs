namespace saikyo_playListTest
{
    [TestClass]
    public class YoutubeApiTest_Video
    {
        private YoutubeDataRepository _repo { get; set; }

        private const string apiKey = "AIzaSyCl2eOWfYiO9cwiqWbRkuWw4ywI2OMZYfA";

        [TestInitialize]
        public void TestInit()
        {
            _repo = new YoutubeDataRepository(apiKey);
        }

        [TestMethod]
        public void GetVideoInfoTest_1()
        {

            var targetVideoId = "MgeIqstCGsw";

            try
            {
                var result = _repo.GetYoutubeVideoInfo(targetVideoId).Result;


                Assert.AreEqual(targetVideoId,result.items[0].id);
                Assert.AreEqual("少女マイノリティ OP 『Minority』 Full 夢乃ゆき",result.items[0].snippet.title);

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void GetVideoInfoTest_2()
        {

            var targetVideoId = "Eleb8fkopjU";

            try
            {
                var result = _repo.GetYoutubeVideoInfo(targetVideoId).Result;


                Assert.AreEqual(targetVideoId, result.items[0].id);
                Assert.AreEqual("Raspberry Cube - Raspberry Cube (Main Theme)", result.items[0].snippet.title);

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void GetVideoInfoTest_3()
        {

            var targetVideoId = "maKok2RItxM";

            try
            {
                var result = _repo.GetYoutubeVideoInfo(targetVideoId).Result;


                Assert.AreEqual(targetVideoId, result.items[0].id);
                Assert.AreEqual("fhána / 青空のラプソディ - MUSIC VIDEO", result.items[0].snippet.title);

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }



    }
}