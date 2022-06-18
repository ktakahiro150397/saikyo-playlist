﻿using saikyo_playlist.Data;
using System.ComponentModel.DataAnnotations;
using saikyo_playlist.Repository.Interfaces;
using saikyo_playlist.Repository.Implements;
using Microsoft.AspNetCore.Identity;

namespace saikyo_playlist.Models.PlayListManage
{
    public class CreateEditDeletePlayListViewModel
    {

        public string PlayListHeaderId { get; set; }

        [Required]
        [Display(Name = "プレイリスト名")]
        public string Title { get; set; }

        /// <summary>
        /// アイテムライブラリの一覧
        /// </summary>
        public List<ItemLibrariesEntity> Libraries { get; set; }

        /// <summary>
        /// アイテムライブラリのうち、プレイリストに追加するよう選択されたアイテム情報リスト。
        /// </summary>
        public List<SelectedItem> SelectedLibraryInfo { get; set; }

        /// <summary>
        /// 取得時点でこのプレイリストに登録されているアイテムの情報。
        /// </summary>
        public List<PlayListDetailsEntity> PlayListDetails { get; set; }

        public string ErrorMessage { get; set; }

        public CreateEditDeletePlayListViewModel()
        {
            PlayListHeaderId = "";
            Title = "";
            Libraries = new List<ItemLibrariesEntity>();
            PlayListDetails = new List<PlayListDetailsEntity>();
            SelectedLibraryInfo = new List<SelectedItem>();
            ErrorMessage = "";
        }

        public async Task SetPlayList(string playListHeaderId,IItemLibraryRepository itemLibraryRepository, IPlayListRepository playListRepository, IdentityUser user)
        {
            var getResult = playListRepository.GetPlayList(playListHeaderId, user);
            if(getResult.OperationResult != PlayListOperationResultType.Success)
            {
                //取得に失敗
                throw new ApplicationException("プレイリストの取得に失敗しました。", getResult.Exception);
            }
            else
            {
                PlayListHeaderId = playListHeaderId;
                PlayListDetails = getResult.HeaderEntity!.Details.ToList();
            }

            var libResult = await itemLibraryRepository.GetAllAsync(user);
            Libraries = libResult.ToList();
        }

    }

    public class SelectedItem
    {
        /// <summary>
        /// 選択されたアイテムID。
        /// </summary>
        public string ItemLibraryEntityId { get; set; }

        /// <summary>
        /// 選択されたアイテムIDの連番。
        /// </summary>
        public int itemSeq { get; set; }

        public SelectedItem()
        {
            ItemLibraryEntityId = "";
        }

    }

}
