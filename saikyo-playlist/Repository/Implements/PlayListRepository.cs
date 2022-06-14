using Microsoft.AspNetCore.Identity;
using saikyo_playlist.Data;
using saikyo_playlist.Repository.Interfaces;
using System.Web;

namespace saikyo_playlist.Repository.Implements
{
    /// <summary>
    /// プレイリスト作成・取得に関する機能を提供します。
    /// </summary>
    public class PlayListRepository : IPlayListRepository
    {

        private ApplicationDbContext dbContext;
        private UserManager<IdentityUser> user;
        private IConfiguration Config;

        public PlayListRepository(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> identityUser, IConfiguration config)
        {
            dbContext = applicationDbContext;
            user = identityUser;
            Config = config;
        }

        /// <summary>
        /// プレイリストを新規作成します。
        /// </summary>
        /// <param name="playListName">プレイリストの名前。</param>
        /// <param name="registerDataStr">プレイリストに登録するURL。「URL,タイトル,エイリアス」を改行で区切ります。</param>
        /// <returns>登録に成功した場合true。</returns>
        public async Task<bool> CreateNewPlayListAsync(string playListName, string registerDataStr)
        {
            var ret = false;


            //ヘッダー・詳細を同一トランザクションでインサートする
            using (var tran = dbContext.Database.BeginTransaction())
            {
                try
                {
                    //プレイリストの登録
                    //await InsertPlayListAsync(playListName, user.Id, registerDataStr);

                    tran.Commit();
                    ret = true;
                }
                catch
                {
                    tran.Rollback();
                }
            }

            return ret;
        }

        /// <summary>
        /// プレイリストURLから、プレイリストを新規作成します。
        /// </summary>
        /// <param name="playListName">プレイリストの名前。</param>
        /// <param name="playListUrl">YoutubeのプレイリストのURL。</param>
        /// <returns>登録に成功した場合true。</returns>
        public async Task<bool> CreateNewPlayListFromPlayListUrlAsync(string playListName, string playListUrl)
        {
            var ret = false;

            //ヘッダー・詳細を同一トランザクションでインサートする
            using (var tran = dbContext.Database.BeginTransaction())
            {
                var headerId = GetUniqueId();

                try
                {
                    //データを作成
                    var header = new PlayListHeadersEntity();

                    //ヘッダーの割当
                    header.PlayListHeadersEntityId = headerId;
                    header.Name = playListName;
                    //header.AspNetUserdId = user.Id;
                    header.Details = await CreateDetailDataFromPlayListUrl(header, playListUrl);

                    //プレイリストの登録
                    await InsertPlayListData(header);

                    tran.Commit();
                    ret = true;
                }
                catch
                {
                    tran.Rollback();
                }
            }

            return ret;
        }

        /// <summary>
        /// 既存のプレイリストを更新します。
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateExistPlayListAsync(string playListId, string playListName, string registerDataStr)
        {
            var ret = false;

            //ヘッダー・詳細を同一トランザクションでインサートする
            using (var tran = dbContext.Database.BeginTransaction())
            {
                try
                {
                    //プレイリストの登録
                    //await UpdatePlayListAsync(playListId, playListName, user.Id, registerDataStr);

                    tran.Commit();
                    ret = true;
                }
                catch
                {
                    tran.Rollback();
                }
            }

            return ret;
        }

        /// <summary>
        /// データのIDに使用できる一意の文字列を返します。
        /// </summary>
        /// <returns></returns>
        private string GetUniqueId()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// プレイリストデータを登録します。
        /// </summary>
        /// <param name="playListName">プレイリスト名。</param>
        /// <param name="aspNetUserdId">ユーザーのID。</param>
        /// <param name="registerDataStr">登録データ文字列。「url,タイトル,エイリアス」の形式で入力してください。</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        private async Task<bool> InsertPlayListAsync(string playListName, string aspNetUserdId, string registerDataStr)
        {
            var ret = false;

            var headerId = GetUniqueId();

            try
            {
                //データを作成
                var header = new PlayListHeadersEntity();

                //ヘッダーの割当
                header.PlayListHeadersEntityId = headerId;
                header.Name = playListName;
                header.AspNetUserdId = aspNetUserdId;
                header.Details = await CreateDetailData(header, registerDataStr);

                await dbContext.PlayListHeaders.AddAsync(header);
                await dbContext.SaveChangesAsync();

                ret = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"プレイリストの登録時にエラーが発生しました。", ex);
            }

            return ret;
        }

        private async Task<bool> UpdatePlayListAsync(string playListId, string playListName, string aspNetUserdId, string registerDataStr)
        {
            var ret = false;

            try
            {
                //既存データの取得
                var header = dbContext.PlayListHeaders
                        .Where(item => item.PlayListHeadersEntityId == playListId).FirstOrDefault();

                if (header != null)
                {
                    //ヘッダーの割当
                    header.PlayListHeadersEntityId = playListId;
                    header.Name = playListName;
                    header.AspNetUserdId = aspNetUserdId;
                    header.Details = await CreateDetailData(header, registerDataStr);

                    //既存の詳細プレイリストを削除
                    dbContext.RemoveRange(dbContext.PlayListDetails.Where(item => item.PlayListHeadersEntityId == playListId));

                    //await dbContext.PlayListHeaders.AddAsync(header);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ApplicationException($"「{playListId}」のプレイリストが見つかりませんでした。");
                }



                ret = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"プレイリストの登録時にエラーが発生しました。", ex);
            }

            return ret;
        }

        /// <summary>
        /// 入力されたデータでプレイリスト詳細データを作成します。
        /// </summary>
        /// <param name="headerId"></param>
        /// <param name="registerDataStr"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        private async Task<IList<PlayListDetailsEntity>> CreateDetailData(PlayListHeadersEntity header, string registerDataStr)
        {

            var ret = new List<PlayListDetailsEntity>();

            try
            {
                //CSV形式をバラす
                var dataStrList = registerDataStr.Split(Environment.NewLine);

                foreach (var dataStrItem in dataStrList.Select((item, index) => new { Item = item, Index = index }))
                {
                    //カンマ区切り
                    var commaSeparated = dataStrItem.Item.Split(",");

                    if (commaSeparated.Length != 3)
                    {
                        //データが3つ存在しない
                        throw new ApplicationException($"インデックス「{dataStrItem.Index}」でデータが3つ存在しませんでした。");
                    }
                    else
                    {
                        //URLからアイテムIDを取得
                        var urlQueries = HttpUtility.ParseQueryString(new Uri(commaSeparated[0]).Query);
                        var itemIdFromUrl = "";
                        if (urlQueries.GetValues("v") != null)
                        {
                            itemIdFromUrl = urlQueries.GetValues("v")[0];

                        }
                        else
                        {
                            throw new ApplicationException($"URLからのID取得に失敗しました。");
                        }

                        //アイテムライブラリに追加・または取得する
                        //var itemLibraryRepo = new ItemLibraryRepository(dbContext, user);
                        //var libraryItem = await itemLibraryRepo.InsertOrRetrieveAsync(LibraryItemPlatform.Youtube, itemIdFromUrl, commaSeparated[1]);

                        //プレイリスト詳細を設定し、追加する
                        var detail = new PlayListDetailsEntity();
                        detail.PlayListDetailsEntityId = GetUniqueId();
                        detail.ItemSeq = dataStrItem.Index;
                        //detail.ItemLibrariesEntityId = libraryItem.ItemLibrariesEntityId;
                        //detail.ItemLibrariesEntity = libraryItem;
                        detail.PlayListHeadersEntityId = header.PlayListHeadersEntityId;
                        detail.PlayListHeadersEntity = header;
                        ret.Add(detail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"詳細データ作成時にエラーが発生しました。", ex);
            }

            return ret;
        }

        private async Task<bool> InsertPlayListData(PlayListHeadersEntity header)
        {
            var ret = false;

            try
            {
                await dbContext.PlayListHeaders.AddAsync(header);
                await dbContext.SaveChangesAsync();

                ret = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"プレイリストの登録時にエラーが発生しました。", ex);
            }

            return ret;
        }

        /// <summary>
        /// プレイリストURLから、プレイリスト詳細データを作成します。
        /// </summary>
        /// <param name="headerId"></param>
        /// <param name="registerDataStr"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        private async Task<IList<PlayListDetailsEntity>> CreateDetailDataFromPlayListUrl(PlayListHeadersEntity header, string playListUrl)
        {

            var ret = new List<PlayListDetailsEntity>();

            try
            {
                //URLからプレイリストIDを取得
                var urlQueries = HttpUtility.ParseQueryString(new Uri(playListUrl).Query);
                var playListId = "";
                if (urlQueries.GetValues("list") != null)
                {
                    playListId = urlQueries.GetValues("list")[0];

                }

                //プレイリストのデータをすべて取得する
                var youtubeRepo = new YoutubeDataRepository(Config);
                var playListData = youtubeRepo.GetYoutubePlayListInfo(playListId);

                if (playListData == null)
                {
                    //データが存在しない
                    throw new ApplicationException($"プレイリスト「{playListUrl}」が存在しませんでした。");
                }

                //削除済み動画を除く
                playListData.items = playListData.items.Where(item => item.snippet.title != "Deleted video" && item.snippet.description != "This video is unavailable.").ToList();
                //非公開動画を除く
                playListData.items = playListData.items.Where(item => item.snippet.title != "Private video" && item.snippet.description != "This video is private.").ToList();

                if (playListData.items.Count == 0)
                {
                    //データが存在しない
                    throw new ApplicationException($"プレイリスト「{playListUrl}」に動画が存在しませんでした。");
                }

                foreach (var item in playListData.items.Select((item, index) => new { Item = item, Index = index }))
                {

                    //アイテムライブラリに追加・または取得する
                    //var itemLibraryRepo = new ItemLibraryRepository(dbContext, user);
                    //var libraryItem = await itemLibraryRepo.InsertOrRetrieveAsync(LibraryItemPlatform.Youtube, item.Item.snippet.resourceId.videoId, item.Item.snippet.title);

                    //プレイリスト詳細を設定し、追加する
                    var detail = new PlayListDetailsEntity();
                    detail.PlayListDetailsEntityId = GetUniqueId();
                    detail.ItemSeq = item.Index;
                    //detail.ItemLibrariesEntityId = libraryItem.ItemLibrariesEntityId;
                    //detail.ItemLibrariesEntity = libraryItem;
                    detail.PlayListHeadersEntityId = header.PlayListHeadersEntityId;
                    detail.PlayListHeadersEntity = header;
                    ret.Add(detail);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"詳細データ作成時にエラーが発生しました。", ex);
            }

            return ret;
        }

        public Task<PlayListOperationResult> CreateNewPlayListAsync(string playListName)
        {
            throw new NotImplementedException();
        }

        public Task<PlayListOperationResult> AddItemToPlayListAsync(PlayListHeadersEntity header, PlayListDetailsEntity detail)
        {
            throw new NotImplementedException();
        }

        public Task<PlayListOperationResult> AddItemToPlayListAsync(PlayListHeadersEntity header, IEnumerable<PlayListDetailsEntity> detailList)
        {
            throw new NotImplementedException();
        }

        public Task<PlayListOperationResult> RemoveItemFromPlayListAsync(PlayListHeadersEntity header, string playListDetailId)
        {
            throw new NotImplementedException();
        }

        public Task<PlayListOperationResult> UpdatePlayListItemAsync(PlayListHeadersEntity header, PlayListDetailsEntity detail)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PlayListHeadersEntity>> GetPlayListHeaderAll()
        {
            throw new NotImplementedException();
        }
    }

    public class GetPlayListOperationResult
    {
        public PlayListOperationResultType OperationResult { get; set; }

        public PlayList PlayList { get; set; }

        public GetPlayListOperationResult()
        {
            PlayList = new PlayList();
        }
    }


    /// <summary>
    /// 単一のプレイリストを表します。
    /// </summary>
    public class PlayList
    {
        public PlayListHeadersEntity Header { get; set; }

        public IList<PlayListDetailsEntity> Details { get; set; }

        public PlayList()
        {
            Header = new PlayListHeadersEntity();
            Details = new List<PlayListDetailsEntity>();
        }
    }

    public class PlayListOperationResult
    {
        /// <summary>
        /// 操作結果
        /// </summary>
        public PlayListOperationResultType OperationResult { get; set; }

        /// <summary>
        /// エラーが発生している場合、その例外オブジェクト。
        /// </summary>
        public Exception? Exception { get; set; }
    }

    /// <summary>
    /// プレイリストエンティティへの操作結果を表します。
    /// </summary>
    public enum PlayListOperationResultType
    {
        /// <summary>
        /// 操作に成功
        /// </summary>
        Success,

        /// <summary>
        /// (取得・更新・削除時)対象のデータが存在しなかった
        /// </summary>
        NotFound,

        /// <summary>
        /// 操作時に予期せぬエラー
        /// </summary>
        UnExpectedError,

    }


}
