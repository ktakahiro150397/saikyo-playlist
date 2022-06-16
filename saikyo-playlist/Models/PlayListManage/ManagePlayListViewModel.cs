using Microsoft.AspNetCore.Identity;
using saikyo_playlist.Data;
using saikyo_playlist.Repository.Interfaces;

namespace saikyo_playlist.Models.PlayListManage
{
    public class ManagePlayListViewModel
    {

        public IList<ManagePlayListItem> managePlayListItems { get; set; } = new List<ManagePlayListItem>();

        private IPlayListRepository PlayListRepository { get; set; }

        private IdentityUser User { get; set; }

        public ManagePlayListViewModel()
        {
            {
                var ManagePlayListItem = new ManagePlayListItem();
                ManagePlayListItem.PlayListName = "playlistName1";
                ManagePlayListItem.UserId = "user1";
                managePlayListItems.Add(ManagePlayListItem);
            }
            {
                var ManagePlayListItem = new ManagePlayListItem();
                ManagePlayListItem.PlayListName = "playlistName2";
                ManagePlayListItem.UserId = "user2";
                managePlayListItems.Add(ManagePlayListItem);
            }
            {
                var ManagePlayListItem = new ManagePlayListItem();
                ManagePlayListItem.PlayListName = "playlistName3";
                ManagePlayListItem.UserId = "user3";
                managePlayListItems.Add(ManagePlayListItem);
            }
            {
                var ManagePlayListItem = new ManagePlayListItem();
                ManagePlayListItem.PlayListName = "playlistName4";
                ManagePlayListItem.UserId = "user4";
                managePlayListItems.Add(ManagePlayListItem);
            }
        }

        public ManagePlayListViewModel(IPlayListRepository playListRepos,IdentityUser user)
        {
            PlayListRepository = playListRepos;
            User = user;
        }

        /// <summary>
        /// このViewModelを初期化します。
        /// </summary>
        /// <returns></returns>
        public async Task Initialize() {

            var playList = PlayListRepository.GetPlayListHeaderAll(User);

            if(playList == null)
            {
                //データなし
            }
            else
            {
                managePlayListItems = playList.Select(
                    elem => new ManagePlayListItem()
                    {
                        PlayListHeaderId = elem.PlayListHeadersEntityId,
                        PlayListName = elem.Name,
                        UserId = elem.AspNetUserdId
                    }).ToList();
            }
        }

    }

    public class ManagePlayListItem
    {

        public string PlayListName { get; set; }

        public string UserId { get; set; }

        public string PlayListHeaderId { get; set; }

        public ManagePlayListItem()
        {
            PlayListName = "";
            UserId = "";
            PlayListHeaderId = "";
        }
    }
}
