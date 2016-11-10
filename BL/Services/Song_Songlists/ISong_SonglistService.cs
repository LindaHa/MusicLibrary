using BL.DTOs.Song_Songlists;
using BL.DTOs.Songlists;
using BL.DTOs.Songs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Song_Songlists
{
    /// <summary>
    /// Handles Song_Songlist CRUD
    /// </summary>  
    public interface ISong_SonglistService : ICreator
    {
        int PageSize { get; }

        /// <summary>
        /// Creates new song_songlist
        /// </summary>
        /// <param name="song_songlistDTO">song_songlist details</param>
        void CreateSong_Songlist(Song_SonglistDTO song_songlistDTO);

        /// <summary>
        /// Updates Song_Songlist according to ID
        /// </summary>
        /// <param name="Song_SonglistDTO">song_songlist details</param>
        void EditSong_Songlist(Song_SonglistDTO song_songlistDTO, int songlistID, int songID);

        /// <summary>
        /// Removes song_songlist according to ID
        /// </summary>
        /// <param name="song_songlistId">song_songlist ID</param>
        void DeleteSong_Songlist(int song_songlistId);

        /// <summary>
        /// Gets song_songlist according to ID
        /// </summary>
        /// <param name="song_songlistId">song_songlist ID</param>
        /// <returns>The song_songlist</returns>
        Song_SonglistDTO GetSong_Songlist(int song_songlistId);        

        /// <summary>
        /// Gets all song_songlists
        /// </summary>
        /// <returns>all available song_songlists</returns>
        IEnumerable<Song_SonglistDTO> ListAllSong_Songlists();

        /// <summary>
        /// Gets song IDs according to songlist ID
        /// </summary>
        /// <param name="songlistID">songlist ID</param>
        /// <returns>The IDs of songs of the given songlist</returns>
        IEnumerable<SongDTO> GetAllSongsForSonglist(int songlistID);

        /// <summary>
        /// Gets songlist IDs according to song ID
        /// </summary>
        /// <param name="songID">song ID</param>
        /// <returns>The IDs of songlists of the given song</returns>
        IEnumerable<SonglistDTO> GetAllSonglistsForSong(int songID);
    }
}
