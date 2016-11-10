using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Artists;
using BL.DTOs.Filters;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BL.Queries
{
    public class ArtistListQuery : AppQuery<ArtistDTO>
    {
        public ArtistFilter Filter { get; set; }

        public ArtistListQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<ArtistDTO> GetQueryable()
        {
            IQueryable<Artist> queryArtist = Context.Artists
                                            .Include(nameof(Artist.Albums))
                                            .Include(nameof(Artist.Creator));
            IQueryable<Genre_Album> queryGenre_album = Context.Genre_Albums
                                                    .Include(nameof(Genre_Album.Album))
                                                    .Include(g_a => g_a.Album.Artist)
                                                    .Include(nameof(Genre_Album.Genre));

            if (!string.IsNullOrEmpty(Filter.Name))
            {
                queryArtist = queryArtist.Where(artist => artist.Name.ToLower().Contains(Filter.Name.ToLower()));
            }
            if (Filter.CreatorIDs != null && Filter.CreatorIDs.Any())
            {
                queryArtist = queryArtist.Where(artist => Filter.CreatorIDs.Contains(artist.Creator.ID));
            }

            queryArtist = queryArtist.Where(artist => artist.IsOfficial);
            return queryArtist.ProjectTo<ArtistDTO>();
        }

        
    }
}
