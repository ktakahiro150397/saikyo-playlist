using saikyo_playlist.Repository.Interfaces;

namespace saikyo_playlist.Repository.Implements
{
    public abstract class RepositoryBase : IUniqueIdIssuable
    {

        /// <summary>
        /// データのインサートに使用できるユニークなIDを返します。
        /// </summary>
        /// <returns></returns>
        public string GetUniqueId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
