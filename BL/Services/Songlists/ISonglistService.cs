using BL.DTOs.Filters;
using BL.DTOs.Songlists;
using BL.DTOs.Songs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Songlists
{
    /// <summary>
    /// Handles Songlist CRUD
    /// </summary>  
    public interface ISonglistService : ICreator
    {
        int PageSize { get; }

        /// <summary>
        /// Creates new songlist
        /// </summary>
        /// <param name="songlistDTO">songlist details</param>
        void CreateSonglist(SonglistDTO songlistDTO);

        /// <summary>
        /// Updates Songlist according to ID
        /// </summary>
        /// <param name="SonglistDTO">songlist details</param>
        void EditSonglist(SonglistDTO songlistDTO, List<int> songIds);

        /// <summary>
        /// Removes songlist according to ID
        /// </summary>
        /// <param name="songlistId">songlist ID</param>
        void DeleteSonglist(int songlistId);

        /// <summary>
        /// Gets songlist according to ID
        /// </summary>
        /// <param name="songlistId">songlist ID</param>
        /// <returns>The songlist</returns>
        SonglistDTO GetSonglist(int songlistId);

        /// <summary>
        /// Gets songlist id that belongs to specified songlist name
        /// </summary>
        /// <param name="name">songlist name</param>
        /// <returns>id of songlist with specified name</returns>
        int GetSonglistIdByName(string name);

        /// <summary>
        /// Gets all songlists
        /// </summary>
        /// <returns>all available songlists</returns>
        IEnumerable<SonglistDTO> ListAllSonglists();

        /// <summary>
        /// Gets all songlists
        /// </summary>
        /// <param name="filter">songlist filter</param>
        /// <param name="requiredPage">page to show</param>
        /// <returns>all available songlists</returns>
        SonglistListQueryResultDTO ListAllSonglists(SonglistFilter filter, int requiredPage);

        /// <summary>
        /// Gets songs of the given songlist
        /// </summary>
        /// <param name="songlistID">songlist ID</param>
        /// <returns>all available songlists</returns>
        IEnumerable<SongDTO> GetSonglistSongs(int songlistID);
    }
}
