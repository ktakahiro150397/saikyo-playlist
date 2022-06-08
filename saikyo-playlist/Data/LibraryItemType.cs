namespace saikyo_playlist.Data
{

    /// <summary>
    /// ライブラリアイテムのプラットフォーム。
    /// </summary>
    public enum LibraryItemPlatform
    {
        NotSet = 0,
        Youtube = 1,
        Spotify = 2,
        AppleMusic = 3,
    }


    static class LibraryItemPlatformExtension
    {
        /// <summary>
        /// プラットフォームIDに対応するプラットフォーム名を返します。
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public static string GetPlatformName(this LibraryItemPlatform platform)
        {
            string[] names = { "なし", "Youtube", "Spotify", "Apple Music" };

            return names[(int)platform];
        }
    }
}