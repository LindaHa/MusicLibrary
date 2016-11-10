using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Filters;
using BL.DTOs.Songlists;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Queries
{
    public class SonglistListQuery : AppQuery<SonglistDTO>
    {
        public SonglistFilter Filter { get; set; }

        public SonglistListQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<SonglistDTO> GetQueryable()
        {
            IQueryable<Songlist> querySonglist = Context.Songlists
                                            .Include(nameof(Songlist.Songs))
                                            .Include(nameof(Songlist.Owner));

            if (Filter.OwnerIDs != null && Filter.OwnerIDs.Any())
            {
                querySonglist = querySonglist.Where(songlist => Filter.OwnerIDs.Contains(songlist.Owner.ID));
            }
            if (Filter.SongID > 0)
            {
                querySonglist = querySonglist.Where(songlist => songlist.Songs.Select(song => song.ID).Contains(Filter.SongID));
            }
            if (!string.IsNullOrEmpty(Filter.Name))
            {
                querySonglist = querySonglist.Where(songlist => songlist.Name.ToLower().Contains(Filter.Name.ToLower()));
            }
            
            return querySonglist.ProjectTo<SonglistDTO>();
        }

       
    }
}
