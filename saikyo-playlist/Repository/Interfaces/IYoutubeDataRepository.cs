using System;
using saikyo_playlist.Data.Video;

namespace saikyo_playlist.Repository.Interfaces
{

	/// <summary>
    /// Youtubeからデータを取得する操作を提供します。
    /// </summary>
	public interface IYoutubeDataRepository
	{

        /// <summary>
        /// URLから、Youtubeの動画情報を取得します。
        /// </summary>
        /// <param name="url">動画のURL。</param>
        /// <returns></returns>
        Task<YoutubeVideoRetrieveResult?> GetYoutubeVideoInfoAsync(string url);

        /// <summary>
        /// URLから、Youtube再生リストの一覧を取得します。
        /// </summary>
        /// <param name="url">動画のURL。</param>
        /// <returns></returns>
        Task<IEnumerable<YoutubeVideoRetrieveResult>> GetYoutubePlayListInfoAsync(string url);



	}
}

