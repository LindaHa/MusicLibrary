using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Filters;
using BL.DTOs.SongReviews;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Queries
{
    public class SongReviewListQuery : AppQuery<SongReviewDTO>
    {
        public SongReviewFilter Filter { get; set; }

        public SongReviewListQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<SongReviewDTO> GetQueryable()
        {
            IQueryable<SongReview> query = Context.SongReviews
                                                .Include(nameof(SongReview.Song))
                                                .Include(nameof(SongReview.Creator));

            if (Filter.CreatorIDs != null && Filter.CreatorIDs.Any())
            {
                query = query.Where(review => Filter.CreatorIDs.Contains(review.Creator.ID));
            }
            if (Filter.SongID > 0)
            {
                query = query.Where(review => review.Song.ID == Filter.SongID);
            }
            return query.ProjectTo<SongReviewDTO>();
        }
    }
}
