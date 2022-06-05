using saikyo_playlist.Data;

namespace saikyo_playlist.Models.PlayListManage
{
    public class ManagePlayListViewModel
    {

        public IList<ManagePlayListItem> managePlayListItems { get; set; } = new List<ManagePlayListItem>();

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

        public ManagePlayListViewModel(ApplicationDbContext dbContext)
        {
            //一覧に表示するヘッダー情報を取得する
            var headers = dbContext.PlayListHeaders
                .Select(item => new ManagePlayListItem()
                {
                    PlayListHeaderId = item.PlayListHeadersEntityId,
                    PlayListName = item.Name,
                    UserId = item.AspNetUserdId
                }).ToList();

            managePlayListItems = headers;
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
