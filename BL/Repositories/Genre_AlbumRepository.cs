using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
namespace BL.Repositories
{
    public class Genre_AlbumRepository : EntityFrameworkRepository<Genre_Album, int>
    {
        public Genre_AlbumRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
