using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using saikyo_playlist.Models;

namespace saikyo_playlist.Models
{
    public class PlayerPlayListModel
    {
        /// <summary>
        /// プレイリストに存在するビデオIDを格納するリスト。
        /// </summary>
        public IList<VideoInfo> VideoId { get; set; }

        /// <summary>
        /// 現在再生している動画のインデックス。
        /// </summary>
        public int CurrentPlayingIndex { get; private set; }

        /// <summary>
        /// 次に再生する動画のインデックス。
        /// </summary>
        public int NextPlayIndex { get
            {
                if (CurrentPlayingIndex <= VideoId.Count - 1)
                {
                    return CurrentPlayingIndex + 1;
                }
                else
                {
                    return 0;
                }
            } 
        }

        public PlayerPlayListModel()
        {
            VideoId = new List<VideoInfo>();
            CurrentPlayingIndex = 0;

            var v1 = new VideoInfo()
            {
                Id = "hlxMbvGnnSw",
                Title = "ReAnswer",
                PlayCount = 0,
            };

            var v2 = new VideoInfo()
            {
                Id = "ogwfFWFGiRI",
                Title = "Ruri Yakushi / 神様いつかこの想い - 鯨神のティアスティラ ED1【Full】",
                PlayCount = 0,
            };

            var v3 = new VideoInfo()
            {
                Id = "MgeIqstCGsw",
                Title = "少女マイノリティ OP 『Minority』 Full 夢乃ゆき",
                PlayCount = 0,
            };

            VideoId.Add(v1);
            VideoId.Add(v2);
            VideoId.Add(v3);

        }

        /// <summary>
        /// 次に再生する動画のインデックスを返します。
        /// 再生中インデックスを加算します。
        /// </summary>
        /// <returns></returns>
        public int GetNextVideoIndexAndIncrementIndex()
        {
            var retNextIndex = NextPlayIndex;
            CurrentPlayingIndex = NextPlayIndex + 1;
            return retNextIndex;
        }
    }


    public class VideoInfo
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public int PlayCount { get; set; }


    }

}
