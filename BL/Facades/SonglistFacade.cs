using BL.DTOs.Songlists;
using BL.DTOs.Songs;
using BL.Services.Song_Songlists;
using BL.Services.Songlists;
using BL.Services.Songs;
using System.Collections.Generic;

namespace BL.Repositories
{
    public class SonglistFacade
    {
        public int PageSize => songlistService.PageSize;

        private readonly ISonglistService songlistService;

        private readonly ISongService songService;

        private readonly ISong_SonglistService song_songlist;

        public SonglistFacade(ISonglistService songlistService, ISongService songService, ISong_SonglistService song_songlist)
        {
            this.songlistService = songlistService;
            this.songService = songService;
            this.song_songlist = song_songlist;
        }

        /// <summary>
        /// Creates an songlist 
        /// </summary>
        /// <param name="songlist">songlist</param>
        public void CreateSonglist(SonglistDTO songlist)
        {
            songlistService.CreateSonglist(songlist);
        }

        public void EditSonglist(SonglistDTO songlistDTO, params int[] songIds)
        {
            songlistService.EditSonglist(songlistDTO, songIds);
        }

        public void DeleteSonglist(int id)
        {
            songlistService.DeleteSonglist(id);
        }

        /// <summary>
        /// Gets songlist according to ID
        /// </summary>
        /// <param name="songlistId">songlist ID</param>
        /// <returns>The songlist</returns>
        public SonglistDTO GetSonglist(int songlistId)
        {
            return songlistService.GetSonglist(songlistId);
        }

        /// <summary>
        /// Gets the songlist id that belongs to a specified songlist name
        /// </summary>
        /// <param name="name">songlist name</param>
        /// <returns>id of songlist with specified name</returns>
        public int GetSonglistIdByName(string name)
        {
            return songlistService.GetSonglistIdByName(name);
        }

        /// <summary>
        /// Gets all songlists
        /// </summary>
        /// <returns>All available songlists</returns>
        public IEnumerable<SonglistDTO> GetAllSonglists()
        {
            return songlistService.ListAllSonglists();
        }


        public void AddSong(SongDTO songDTO)
        {
            songService.AddSong(songDTO);
        }

        public void EditSong(SongDTO songDTO)
        {
            songService.EditSong(songDTO, songDTO.AlbumID, songDTO.ReviewIDs);
        }

        public void DeleteSong(int songId)
        {
            songService.DeleteSong(songId);
        }

        /// <summary>
        /// Gets all reviews of the given songlist
        /// </summary>
        /// <param name="songlistId">songlist id</param>
        /// <returns>all reviews of the given songlist</returns>
        public IEnumerable<SongDTO> GetAllSnogs(int songlistId = 0)
        {
            return songlistService.GetSonglistSongs(songlistId);
        }


    }
}
