using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Filters;
using BL.DTOs.Songs;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Queries
{
    public class SongListQuery : AppQuery<SongDTO>
    {
        public SongFilter Filter { get; set; }

        public SongListQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<SongDTO> GetQueryable()
        {
            IQueryable<Song> querySong = Context.Songs
                                            .Include(nameof(Song.Album))
                                            .Include(nameof(Song.Reviews))
                                            .Include(nameof(Song.Creator));
            IQueryable<Album> queryAlbum = Context.Albums
                                                    .Include(nameof(Album.Songs))
                                                    .Include(nameof(Album.Artist));

            if (Filter?.CreatorIDs != null && Filter.CreatorIDs.Any())
            {
                querySong = querySong.Where(song => Filter.CreatorIDs.Contains(song.Creator.ID));
            }
            if (Filter?.ArtistIDs != null && Filter.ArtistIDs.Any())
            {
                queryAlbum = queryAlbum.Where(album => Filter.ArtistIDs.Contains(album.Artist.ID) && album.IsOfficial);
                querySong = querySong.Where(song => queryAlbum.Select(album => album.ID).Contains(song.Album.ID));
            }
            if (Filter?.MinimalRating > 0)
            {
                querySong = querySong.Where(song => song.Reviews.Average(review => review.UserRating) >= Filter.MinimalRating);
            }
            if (Filter?.MinimalDuration != null)
            {
                querySong = querySong.Where(song => song.Duration >= Filter.MinimalDuration);
            }
            if (Filter?.MaximalDuration != null && Filter.MaximalDuration != new TimeSpan(0,0,0,0))
            {
                querySong = querySong.Where(song => song.Duration <= Filter.MaximalDuration);
            }
            if (!string.IsNullOrEmpty(Filter?.Name))
            {
                querySong = querySong.Where(song => song.Name.ToLower().Contains(Filter.Name.ToLower()));
            }

            querySong = querySong.Where(song => song.IsOfficial);
            return querySong.ProjectTo<SongDTO>();
        }

        protected override void PostProcessResults(IList<SongDTO> results)
        {
            foreach (var songDTO in results)
            {
                if (Context.SongReviews.Any())
                {
                    var rating = Context.SongReviews
                        .Where(review => review.Song.ID == songDTO.ID)
                        .Average(rev => (double?)(rev.UserRating));
                    songDTO.AverageRating = rating;
                }
            }

            base.PostProcessResults(results);
        }
    }
}
