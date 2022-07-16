using Microsoft.AspNetCore.Identity;
using saikyo_playlist.Data;
using saikyo_playlist.Repository.Interfaces;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace saikyo_playlist.Repository.Implements
{
    /// <summary>
    /// プレイリスト作成・取得に関する機能を提供します。
    /// </summary>
    public class PlayListRepository : IPlayListRepository
    {

        private ApplicationDbContext dbContext;
        private IConfiguration Config;

        public PlayListRepository(ApplicationDbContext applicationDbContext, IConfiguration config)
        {
            dbContext = applicationDbContext;
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

        public IEnumerable<PlayListHeadersEntity> GetPlayListHeaderAll(IdentityUser user)
        {
            var result = dbContext.PlayListHeaders.Where(header => header.AspNetUserdId == user.Id)
                            .OrderByDescending(header => header.LastPlayedDate)
                            .ToList();

            foreach(var header in result)
            {
                //詳細の最初の1データを設定
                header.Details = dbContext.PlayListDetails
                                    .Include(item => item.ItemLibrariesEntity)
                                    .Where(item => item.PlayListHeadersEntityId == header.PlayListHeadersEntityId)
                                    .Where(item => item.ItemSeq == 0)
                                    .ToList();
            }

            return result;
        }

        public async Task<PlayListOperationResult> CreateNewPlayListAsync(string playListName, IdentityUser user)
        {
            var ret = new PlayListOperationResult();

            if (String.IsNullOrEmpty(playListName))
            {
                ret.OperationResult = PlayListOperationResultType.NoName;
                ret.Exception = new ApplicationException("プレイリストの名前が入力されていません。");
                return ret;
            }

            try
            {
                var header = new PlayListHeadersEntity()
                {
                    Name = playListName,
                    AspNetUserdId = user.Id,
                    PlayListHeadersEntityId = GetUniqueId(),
                    LastPlayedDate = DateTime.Now
                };
                dbContext.PlayListHeaders.Add(header);

                await dbContext.SaveChangesAsync();

                ret.HeaderEntity = header;
                ret.OperationResult = PlayListOperationResultType.Success;
            }
            catch (Exception ex)
            {
                ret.OperationResult = PlayListOperationResultType.UnExpectedError;
                ret.Exception = ex;
            }

            return ret;
        }

        public async Task<PlayListOperationResult> AddItemToPlayListAsync(string headerEntityId, PlayListDetailsEntity detail, IdentityUser user)
        {
            var ret = new PlayListOperationResult();

            var header = dbContext.PlayListHeaders.Where(h => h.PlayListHeadersEntityId == headerEntityId).FirstOrDefault();

            if (header == null)
            {
                ret.OperationResult = PlayListOperationResultType.NotFound;
                ret.Exception = new ApplicationException("プレイリストが存在しませんでした。");
                return ret;
            }

            try
            {
                ////詳細の最大番号を取得
                //var detailMaxSeq = dbContext.PlayListDetails.Where(
                //    detail => detail.PlayListHeadersEntityId == header.PlayListHeadersEntityId)
                //    .Max(detail => detail.ItemSeq);
                //detail.ItemSeq = detailMaxSeq + 1;

                //データを設定
                detail.PlayListDetailsEntityId = GetUniqueId();

                header.Details.Add(detail);
                await dbContext.SaveChangesAsync();

                ret.OperationResult = PlayListOperationResultType.Success;
                ret.HeaderEntity = header;
            }
            catch (Exception ex)
            {
                ret.OperationResult = PlayListOperationResultType.UnExpectedError;
                ret.Exception = ex;
            }

            return ret;
        }

        public async Task<PlayListOperationResult> AddItemToPlayListAsync(string headerEntityId, IEnumerable<PlayListDetailsEntity> detailList, IdentityUser user)
        {
            var ret = new PlayListOperationResult();

            foreach (var detail in detailList)
            {
                ret = await AddItemToPlayListAsync(headerEntityId, detail, user);

                if (ret.OperationResult != PlayListOperationResultType.Success)
                {
                    //成功以外の場合、終了
                    return ret;
                }
            }

            return ret;
        }

        public async Task<PlayListOperationResult> RemoveItemFromPlayListAsync(string headerEntityId, string playListDetailId, IdentityUser user)
        {
            var ret = new PlayListOperationResult();

            var header = dbContext.PlayListHeaders.Where(item => item.PlayListHeadersEntityId == headerEntityId).FirstOrDefault();
            ret.HeaderEntity = header;

            if (header == null)
            {
                ret.OperationResult = PlayListOperationResultType.NotFound;
                ret.Exception = new ApplicationException("削除対象のプレイリストが存在しませんでした。");
                return ret;
            }
            else
            {
                if (header.AspNetUserdId != user.Id)
                {
                    ret.OperationResult = PlayListOperationResultType.NotFound;
                    ret.Exception = new ApplicationException("他ユーザーのプレイリストデータを削除しようとしました。");
                    return ret;
                }


                //削除対象のプレイリスト詳細を取得
                var detail = header.Details.SingleOrDefault(detail => detail.PlayListDetailsEntityId == playListDetailId);
                if (detail == null)
                {
                    ret.OperationResult = PlayListOperationResultType.NotFound;
                    ret.Exception = new ApplicationException("削除対象のプレイリストアイテムが存在しませんでした。");
                    return ret;
                }
                else
                {
                    dbContext.PlayListDetails.Remove(detail);
                    await dbContext.SaveChangesAsync();

                    ret.OperationResult = PlayListOperationResultType.Success;
                    return ret;
                }

            }
        }


        public PlayListOperationResult GetPlayList(string headerEntityId, IdentityUser user)
        {
            var ret = new PlayListOperationResult();

            var playListInfo = dbContext.PlayListHeaders
                .SingleOrDefault(header => header.PlayListHeadersEntityId == headerEntityId);

            if (playListInfo == null)
            {
                ret.OperationResult = PlayListOperationResultType.NotFound;
                ret.Exception = new ApplicationException("プレイリストが存在しませんでした。");
            }
            else
            {
                if (playListInfo.AspNetUserdId != user.Id)
                {
                    ret.OperationResult = PlayListOperationResultType.NotFound;
                    ret.Exception = new ApplicationException("他ユーザーのプレイリストデータを取得しようとしました。");
                    return ret;
                }

                playListInfo.Details = dbContext.PlayListDetails
                    .Where(elem => elem.PlayListHeadersEntityId == headerEntityId)
                    .Join(
                        dbContext.ItemLibraries,
                        detail => detail.ItemLibrariesEntityId,
                        lib => lib.ItemLibrariesEntityId,
                        (detail, lib) => new PlayListDetailsEntity()
                        {
                            ItemLibrariesEntity = lib,
                            PlayListDetailsEntityId = detail.PlayListDetailsEntityId,
                            ItemSeq = detail.ItemSeq,
                            TimeStamp = detail.TimeStamp,
                            ItemLibrariesEntityId = lib.ItemLibrariesEntityId,
                        }
                    )
                    .OrderBy(elem => elem.ItemSeq)
                    .ToList();

                ret.OperationResult = PlayListOperationResultType.Success;
                ret.HeaderEntity = playListInfo;
            }

            return ret;
        }

        public PlayListOperationResult UpdatePlayListItemSeqAsync(string headerEntityId, string playListDetailId, int itemSeq, IdentityUser user)
        {

            if (itemSeq < 0)
            {
                throw new ArgumentException("プレイリスト連番に負の値が指定されました。", nameof(itemSeq));
            }

            var ret = new PlayListOperationResult();

            var header = dbContext.PlayListHeaders
                .SingleOrDefault(header => header.PlayListHeadersEntityId == headerEntityId);

            if (header == null)
            {
                ret.OperationResult = PlayListOperationResultType.NotFound;
                ret.Exception = new ApplicationException("プレイリストが存在しませんでした。");
                return ret;
            }
            else
            {
                if (header.AspNetUserdId != user.Id)
                {
                    ret.OperationResult = PlayListOperationResultType.NotFound;
                    ret.Exception = new ApplicationException("他ユーザーのプレイリストデータを取得しようとしました。");
                    return ret;
                }

                if (!header.Details.Any(detail => detail.PlayListDetailsEntityId == playListDetailId))
                {
                    //指定されたアイテムが存在しない
                    ret.OperationResult = PlayListOperationResultType.NotFound;
                    ret.Exception = new ApplicationException("更新対象のプレイリストアイテムが存在しませんでした。");
                    return ret;
                }

                //入れ替え処理

                //入れ替え対象オブジェクトを保持
                var detailId = header.Details.Single(detail => detail.PlayListDetailsEntityId == playListDetailId);
                var detailIdSeq = detailId.ItemSeq;

                //入れ替えられるオブジェクトを保持
                int targetDetailSeq;
                if (!header.Details.Any(detail => detail.ItemSeq == itemSeq))
                {
                    //引数のitemSeqが最大値を超えている
                    //最大値のitemSeqに置換して処理
                    targetDetailSeq = header.Details.Max(detail => detail.ItemSeq);
                    itemSeq = targetDetailSeq;
                }
                else
                {
                    targetDetailSeq = header.Details.Single(detail => detail.ItemSeq == itemSeq).ItemSeq;
                }

                //対象オブジェクトが元あったItemSeqまで加算する
                //加算対象のオブジェクトを取得
                IList<PlayListDetailsEntity> addTarget;

                if (targetDetailSeq <= detailIdSeq)
                {
                    addTarget = header.Details
                    .Where(detail => targetDetailSeq <= detail.ItemSeq && detail.ItemSeq < detailIdSeq)
                    .ToList();

                    //対象オブジェクトのItemSeqを加算
                    foreach (var data in addTarget)
                    {
                        data.ItemSeq += 1;
                    }
                }
                else
                {
                    addTarget = header.Details
                   .Where(detail => detailIdSeq < detail.ItemSeq && detail.ItemSeq <= targetDetailSeq)
                   .ToList();

                    //対象オブジェクトのItemSeqを減算
                    foreach (var data in addTarget)
                    {
                        data.ItemSeq -= 1;
                    }
                }

                //対象オブジェクトの順序を変更
                dbContext.PlayListDetails
                    .Single(detail => detail.PlayListDetailsEntityId == detailId.PlayListDetailsEntityId)
                    .ItemSeq = itemSeq;

                //データを保存
                dbContext.SaveChanges();

                ret.OperationResult = PlayListOperationResultType.Success;
                ret.HeaderEntity = header;
                return ret;
            }


            throw new NotImplementedException();
        }

        public async Task<PlayListOperationResult> DeletePlayListAsync(string headerEntityId, IdentityUser user)
        {
            var ret = new PlayListOperationResult();

            var deleteTargetElement = dbContext.PlayListHeaders
                .FirstOrDefault(elem => elem.PlayListHeadersEntityId == headerEntityId && elem.AspNetUserdId == user.Id);

            if (deleteTargetElement == null)
            {
                //削除対象の要素が存在しない
                ret.OperationResult = PlayListOperationResultType.NotFound;
            }
            else
            {
                //削除対象の要素が存在する
                dbContext.Remove(deleteTargetElement);
                await dbContext.SaveChangesAsync();

                ret.OperationResult = PlayListOperationResultType.Success;
            }

            return ret;
            throw new NotImplementedException();
        }

        public async Task<PlayListOperationResult> RemoveItemAllFromPlayListAsync(string headerEntityId, IdentityUser user)
        {
            var ret = new PlayListOperationResult();

            var details = dbContext.PlayListDetails.Where(elem => elem.PlayListHeadersEntityId == headerEntityId).ToList();

            if (details.Count == 0)
            {
                ret.OperationResult = PlayListOperationResultType.NotFound;
                ret.Exception = new ApplicationException("プレイリストが存在しませんでした。");
            }
            else
            {
                //DELETE文発行
                dbContext.Database.ExecuteSqlInterpolated($"DELETE FROM [PlayListDetails] WHERE PlayListHeadersEntityId = {headerEntityId}");

                //EFCoreのトラッカーをクリアする
                dbContext.ChangeTracker.Clear();

                ret.OperationResult = PlayListOperationResultType.Success;
            }

            return ret;
        }

        public async Task<PlayListOperationResult> UpdatePlayListAsync(string headerEntityId, string playListName)
        {
            var ret = new PlayListOperationResult();
            var header = dbContext.PlayListHeaders.Where(elem => elem.PlayListHeadersEntityId == headerEntityId).FirstOrDefault();

            if(header == null)
            {
                ret.OperationResult = PlayListOperationResultType.NotFound;
            }
            else
            {
                header.Name = playListName;
                await dbContext.SaveChangesAsync();

                ret.OperationResult = PlayListOperationResultType.Success;
            }

            return ret;
        }

        public async Task<PlayListOperationResult> UpdatePlayListPlayTime(string headerEntityId, DateTime dateTime)
        {
            var ret = new PlayListOperationResult();
            var header = dbContext.PlayListHeaders.Where(elem => elem.PlayListHeadersEntityId == headerEntityId).FirstOrDefault();

            if (header == null)
            {
                ret.OperationResult = PlayListOperationResultType.NotFound;
            }
            else
            {
                header.LastPlayedDate = dateTime;
                await dbContext.SaveChangesAsync();

                ret.OperationResult = PlayListOperationResultType.Success;
            }

            return ret;
        }
    }

    public class GetPlayListOperationResult
    {
        public PlayListOperationResultType OperationResult { get; set; }

        public PlayList PlayList { get; set; }

        public Exception? Exception { get; set; }

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

        public PlayList()
        {
            Header = new PlayListHeadersEntity();
        }
    }

    public class PlayListOperationResult
    {
        /// <summary>
        /// 操作結果
        /// </summary>
        public PlayListOperationResultType OperationResult { get; set; }

        /// <summary>
        /// 操作対象のエンティティ。新規作成時にもセットされます。
        /// </summary>
        public PlayListHeadersEntity? HeaderEntity { get; set; }

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
        /// (作成時)名前が入力されていない
        /// </summary>
        NoName,

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
