
using BL.DTOs.Song_Songlists;
using BL.DTOs.Songlists;
using BL.DTOs.Songs;
using BL.Services.Song_Songlists;
using BL.Services.Songlists;
using BL.Services.Songs;
using System.Collections.Generic;

namespace BL.Repositories
{
    public class Song_SonglistFacade
    {
        public int PageSize => song_songlistService.PageSize;

        private readonly ISong_SonglistService song_songlistService;

        private readonly ISonglistService songlistService;

        private readonly ISongService songService;


        public Song_SonglistFacade(ISong_SonglistService song_songlistService, ISonglistService songlistService, ISongService songService)
        {
            this.song_songlistService = song_songlistService;
            this.songlistService = songlistService;
            this.songService = songService;
        }        

        /// <summary>
        /// Creates a song_songlist with songlist and song names
        /// </summary>
        /// <param name="song_songlist">song_songlist</param>
        public void CreateSong_Songlist(Song_SonglistDTO song_songlist)
        {
            song_songlistService.CreateSong_Songlist(song_songlist);
        }

        public void EditSong_Songlist(Song_SonglistDTO song_songlistDTO, int songlistId, int songID)
        {
            song_songlistService.EditSong_Songlist(song_songlistDTO, songlistId, songID);
        }

        public void DeleteSong_Songlist(int id)
        {
            song_songlistService.DeleteSong_Songlist(id);
        }

        /// <summary>
        /// Gets song_songlist according to ID
        /// </summary>
        /// <param name="song_songlistId">song_songlist ID</param>
        /// <returns>The song_songlist</returns>
        public Song_SonglistDTO GetSong_Songlist(int song_songlistId)
        {
            return song_songlistService.GetSong_Songlist(song_songlistId);
        }

        /// <summary>
        /// Gets all song_songlists
        /// </summary>
        /// <returns>All available song_songlists</returns>
        public IEnumerable<Song_SonglistDTO> GetAllSong_Songlists()
        {
            return song_songlistService.ListAllSong_Songlists();
        }

        /// <summary>
        /// Gets all songs of the specified songlist
        /// </summary>
        /// <param name="songlistId">songlist ID</param>
        /// <returns>All available songs</returns>
        public IEnumerable<SongDTO> GetAllSongsOfSonglist(int songlistID)
        {
            return song_songlistService.GetAllSongsForSonglist(songlistID);
        }

        /// <summary>
        /// Gets all songlists of the specified song
        /// </summary>
        /// <param name="songId">song ID</param>
        /// <returns>All available songlists</returns>
        public IEnumerable<SonglistDTO> GetAllSonglistsForSong(int songId)
        {
            return song_songlistService.GetAllSonglistsForSong(songId);
        }
    }
}
