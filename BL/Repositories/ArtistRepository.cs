using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Repositories
{
    public class ArtistRepository : EntityFrameworkRepository<Artist, int>
    {
        public ArtistRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
