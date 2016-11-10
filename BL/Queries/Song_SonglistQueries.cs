using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Filters;
using BL.DTOs.Song_Songlists;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Queries
{
    public class Song_SonglistListQuery : AppQuery<Song_SonglistDTO>
    {
        public Song_SonglistFilter Filter { get; set; }

        public Song_SonglistListQuery(IUnitOfWorkProvider provider) : base(provider) { }

        protected override IQueryable<Song_SonglistDTO> GetQueryable()
        {
            IQueryable<Song_Songlist> querySong_Songlist = Context.Song_Songlists
                                                    .Include(nameof(Song_Songlist.Songlist))
                                                    .Include(nameof(Song_Songlist.Song))
                                                    .Include(nameof(Song_Songlist.Creator));


            if (Filter.CreatorIDs != null && Filter.CreatorIDs.Any())
            {
                querySong_Songlist = querySong_Songlist.Where(s_s => Filter.CreatorIDs.Contains(s_s.Creator.ID));
            }
            if (Filter.SongID > 0)
            {
                querySong_Songlist = querySong_Songlist.Where(s_s => Filter.SongID == s_s.Song.ID);
            }
            if (Filter.SonglistID > 0)
            {
                querySong_Songlist = querySong_Songlist.Where(s_s => Filter.SonglistID == s_s.Songlist.ID);
            }
            
            return querySong_Songlist.ProjectTo<Song_SonglistDTO>();
        }
    }
}
