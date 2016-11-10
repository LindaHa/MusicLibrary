using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Filters;
using BL.DTOs.Genres;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Queries
{
    public class GenreListQuery : AppQuery<GenreDTO>
    {
        public GenreFilter Filter { get; set; }

        public GenreListQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<GenreDTO> GetQueryable()
        {
            IQueryable<Genre> queryGenre = Context.Genres
                .Include(nameof(Genre.Creator));

            if (Filter.CreatorIDs != null && Filter.CreatorIDs.Any())
            {
                queryGenre = queryGenre.Where(genre => Filter.CreatorIDs.Contains(genre.Creator.ID));
            }
            if (!string.IsNullOrEmpty(Filter.Name))
            {
                queryGenre = queryGenre.Where(genre => genre.Name.ToLower().Contains(Filter.Name.ToLower()));
            }

            queryGenre = queryGenre.Where(genre => genre.IsOfficial);
            return queryGenre.ProjectTo<GenreDTO>();
        }
    }
}
