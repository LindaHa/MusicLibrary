using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Filters;
using BL.DTOs.Genre_Albums;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Queries
{
    public class Genre_AlbumListQuery : AppQuery<Genre_AlbumDTO>
    {
        public Genre_AlbumFilter Filter { get; set; }

        public Genre_AlbumListQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<Genre_AlbumDTO> GetQueryable()
        { 
            IQueryable<Genre_Album> queryGenre_Album = Context.Genre_Albums
                                                    .Include(nameof(Genre_Album.Album))
                                                    .Include(nameof(Genre_Album.Genre))
                                                    .Include(nameof(Genre_Album.Creator));

            if (Filter.CreatorIDs != null && Filter.CreatorIDs.Any())
            {
                queryGenre_Album = queryGenre_Album.Where(g_a => Filter.CreatorIDs.Contains(g_a.Creator.ID));
            }
            if (Filter.GenreID > 0)
            {
                queryGenre_Album = queryGenre_Album.Where(g_a => Filter.GenreID == g_a.Genre.ID && g_a.IsOfficial);
            }
            if (Filter.AlbumID > 0)
            {
                queryGenre_Album = queryGenre_Album.Where(g_a => Filter.AlbumID == g_a.Album.ID && g_a.IsOfficial);
            }

            queryGenre_Album = queryGenre_Album.Where(genre_album => genre_album.IsOfficial);
            return queryGenre_Album.ProjectTo<Genre_AlbumDTO>();
        }
    }
}
