using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Albums;
using BL.DTOs.Filters;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Queries
{
    public class AlbumListQuery : AppQuery<AlbumDTO>
    {
        public AlbumFilter Filter { get; set; }

        public AlbumListQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<AlbumDTO> GetQueryable()
        {
            IQueryable<Album> queryAlbum = Context.Albums
                                            .Include(nameof(Album.Reviews))
                                            .Include(nameof(Album.Artist))
                                            .Include(nameof(Album.Creator));
            IQueryable<Genre_Album> queryGenre_album = Context.Genre_Albums
                                                    .Include(nameof(Genre_Album.Album))
                                                    .Include(nameof(Genre_Album.Genre));

            if (Filter.CreatorIDs != null && Filter.CreatorIDs.Any())
            {
                queryAlbum = queryAlbum.Where(album => Filter.CreatorIDs.Contains(album.Creator.ID));
            }
            if (Filter.GenreIDs != null && Filter.GenreIDs.Any())
            {
                queryGenre_album = queryGenre_album.Where(g_a => Filter.GenreIDs.Contains(g_a.Genre.ID) && g_a.IsOfficial);
                queryAlbum = queryAlbum.Where(album => queryGenre_album.Select(g_a => g_a.Album.ID).Contains(album.ID));
            }
            if (Filter.ArtistIDs != null && Filter.ArtistIDs.Any())
            {
                queryAlbum = queryAlbum.Where(album => Filter.ArtistIDs.Contains(album.Artist.ID) && album.Artist.IsOfficial);
            }
            if (Filter.MinimalRating > 0)
            {
                queryAlbum = queryAlbum.Where(album => album.Reviews.Average(review => review.UserRating) >= Filter.MinimalRating);
            }
            if (!string.IsNullOrEmpty(Filter.Name))
            {
                queryAlbum = queryAlbum.Where(album => album.Name.ToLower().Contains(Filter.Name.ToLower()));
            }

            queryAlbum = queryAlbum.Where(album => album.IsOfficial);
            return queryAlbum.ProjectTo<AlbumDTO>();
        }

        protected override void PostProcessResults(IList<AlbumDTO> results)
        {
            foreach (var albumDTO in results)
            {
                if (Context.AlbumReviews.Any())
                {
                    var rating = Context.AlbumReviews
                        .Where(review => review.Album.ID == albumDTO.ID)
                        .Average(rev => (double?)(rev.UserRating));
                    albumDTO.AverageRating = rating;
                }
            }

            base.PostProcessResults(results);
        }
    }
}
