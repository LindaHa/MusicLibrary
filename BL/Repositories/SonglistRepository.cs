﻿using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Repositories
{
    public class SonglistRepository : EntityFrameworkRepository<Songlist, int>
    {
        public SonglistRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
