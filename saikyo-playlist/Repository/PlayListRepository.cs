﻿using Microsoft.AspNetCore.Identity;
using saikyo_playlist.Data;
using System.Web;

namespace saikyo_playlist.Repository
{
    /// <summary>
    /// プレイリスト作成・取得に関する機能を提供します。
    /// </summary>
    public class PlayListRepository
    {

        private ApplicationDbContext dbContext;
        private IdentityUser user;
        private string _youtubeApiKey;

        public PlayListRepository(ApplicationDbContext applicationDbContext, IdentityUser identityUser, string youtubeApiKey)
        {
            dbContext = applicationDbContext;
            user = identityUser;
            _youtubeApiKey = youtubeApiKey;
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
                    await InsertPlayListAsync(playListName, user.Id, registerDataStr);

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
                    header.AspNetUserdId = user.Id;
                    header.Details = CreateDetailDataFromPlayListUrl(header, playListUrl);

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
                    await UpdatePlayListAsync(playListId, playListName, user.Id, registerDataStr);

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
                header.Details = CreateDetailData(header, registerDataStr);

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
                    header.Details = CreateDetailData(header, registerDataStr);

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
        private IList<PlayListDetailsEntity> CreateDetailData(PlayListHeadersEntity header, string registerDataStr)
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

                        var detail = new PlayListDetailsEntity();
                        detail.PlayListDetailsEntityId = GetUniqueId();
                        detail.ItemId = itemIdFromUrl;
                        detail.ItemSeq = dataStrItem.Index;
                        detail.Title = commaSeparated[1];
                        detail.TitleAlias = commaSeparated[2];
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
        private IList<PlayListDetailsEntity> CreateDetailDataFromPlayListUrl(PlayListHeadersEntity header, string playListUrl)
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
                var youtubeRepo = new YoutubeDataRepository(_youtubeApiKey);
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

                    var detail = new PlayListDetailsEntity();
                    detail.PlayListDetailsEntityId = GetUniqueId();
                    detail.ItemId = item.Item.snippet.resourceId.videoId;
                    detail.ItemSeq = item.Index;
                    detail.Title = item.Item.snippet.title;
                    detail.TitleAlias = "";
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
    }
}