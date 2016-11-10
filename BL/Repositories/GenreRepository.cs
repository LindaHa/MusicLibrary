﻿using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Repositories
{
    public class GenreRepository : EntityFrameworkRepository<Genre, int>
    {
        public GenreRepository(IUnitOfWorkProvider provider) : base(provider) { }
    }
}
