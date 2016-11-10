using BL.DTOs.Albums;
using BL.DTOs.Filters;
using BL.DTOs.SongReviews;
using BL.DTOs.Songs;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Songs
{
    /// <summary>
    /// Handles Song CRUD
    /// </summary>  
    public interface ISongService : ICreator
    {
        int PageSize { get; }

        /// <summary>
        /// Creates new song
        /// </summary>
        /// <param name="songDTO">song details</param>
        void CreateSong(SongDTO songDTO);

        /// <summary>
        /// Updates Song according to ID
        /// </summary>
        /// <param name="SongDTO">song details</param>
        void EditSong(SongDTO songDTO, int albumId, params int[] songReviewIds);

        /// <summary>
        /// Removes song according to ID
        /// </summary>
        /// <param name="songId">song ID</param>
        void DeleteSong(int songId);

        /// <summary>
        /// Gets song according to ID
        /// </summary>
        /// <param name="songId">song ID</param>
        /// <returns>The song</returns>
        SongDTO GetSong(int songId);

        /// <summary>
        /// Gets song id that belongs to specified song name
        /// </summary>
        /// <param name="name">song name</param>
        /// <returns>id of song with specified name</returns>
        int GetSongIdByName(string name);

        /// <summary>
        /// Gets all songs
        /// </summary>
        /// <returns>all available songs</returns>
        IEnumerable<SongDTO> ListAllSongs();

        /// <summary>
        /// Gets all songs
        /// </summary>
        /// <param name="filter">songs filter</param>
        /// <param name="requiredPage">page to show</param>
        /// <returns>all available songs</returns>
        SongListQueryResultDTO ListAllSongs(SongFilter filter, int requiredPage);

        /// <summary>
        /// Gets the album of the given song
        /// </summary>
        /// <param name="songID">song ID</param>
        /// <returns>album of the given song</returns>
        AlbumDTO GetAlbumOfSong(int songID);

        /// <summary>
        /// Gets the reviews of the given song
        /// </summary>
        /// <param name="songID">song ID</param>
        /// <returns>reviews of the given song</returns>
        IEnumerable<SongReviewDTO> GetSongReviews(int songID);

        /// <summary>
        /// Adds a song
        /// </summary>
        /// <param name="songDTO">Review details</param>
        void AddSong(SongDTO songDTO);
    }
}
