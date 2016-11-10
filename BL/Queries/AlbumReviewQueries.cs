using BL.AppInfrastructure;
using BL.DTOs.Filters;
using AutoMapper.QueryableExtensions;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.AlbumReviews;
using DAL.Entities;

namespace BL.Queries
{
    public class AlbumReviewListQuery : AppQuery<AlbumReviewDTO>
    {
        public AlbumReviewFilter Filter { get; set; }

        public AlbumReviewListQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<AlbumReviewDTO> GetQueryable()
        {
            IQueryable<AlbumReview> query = Context.AlbumReviews
                                                .Include(nameof(AlbumReview.Album))
                                                .Include(nameof(AlbumReview.Creator));

            if (Filter?.CreatorIDs != null && Filter.CreatorIDs.Any())
            {
                query = query.Where(album => Filter.CreatorIDs.Contains(album.Creator.ID));
            }
            if (Filter?.AlbumID > 0)
            {
                query = query.Where(review => review.Album.ID == Filter.AlbumID);
            }
            return query.ProjectTo<AlbumReviewDTO>();
        }
    }
}
