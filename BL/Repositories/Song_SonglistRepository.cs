using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Repositories
{
    public class Song_SonglistRepository : EntityFrameworkRepository<Song_Songlist, int>
    {
        public Song_SonglistRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
