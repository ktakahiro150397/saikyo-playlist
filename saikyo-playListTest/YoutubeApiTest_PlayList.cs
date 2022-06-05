namespace saikyo_playListTest
{
    [TestClass]
    public class YoutubeApiTest_PlayList
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
            //erげ
            var playListId = "PLEi4OmzKDGqHXuXLI03esLJzarUzbJVFm";

            try
            {
                var result = _repo.GetYoutubePlayListInfo(playListId).Result;

                if(result == null)
                {
                    Assert.Fail();
                }
                Assert.AreEqual(21, result.items.Count);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);

            }

        }

        [TestMethod]
        public void GetVideoInfoTest_2()
        {
            //287件
            var playListId = "PLEL7lWbWu6s5SH0Os8RuYqc_7_5ZcgJ0y";

            try
            {
                var result = _repo.GetYoutubePlayListInfo(playListId).Result;

                if (result == null)
                {
                    Assert.Fail();
                }
                Assert.AreEqual(287, result.items.Count);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);

            }
        }
    }
}