using BL.DTOs.Albums;
using BL.DTOs.Artists;
using BL.DTOs.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Artists
{
    /// <summary>
    /// Handles Artist CRUD
    /// </summary>  
    public interface IArtistService : ICreator
    {
        int PageSize { get; }

        /// <summary>
        /// Creates new artist
        /// </summary>
        /// <param name="artistDTO">artist details</param>
        void CreateArtist(ArtistDTO artistDTO);

        /// <summary>
        /// Updates Artist according to ID
        /// </summary>
        /// <param name="ArtistDTO">artist details</param>
        void EditArtist(ArtistDTO artistDTO, List<int> albumIds);

        /// <summary>
        /// Removes artist according to ID
        /// </summary>
        /// <param name="artistId">artist ID</param>
        void DeleteArtist(int artistId);

        /// <summary>
        /// Gets artist according to ID
        /// </summary>
        /// <param name="artistId">artist ID</param>
        /// <returns>The artist</returns>
        ArtistDTO GetArtist(int artistId);

        /// <summary>
        /// Gets artist id that belongs to specified artist name
        /// </summary>
        /// <param name="name">artist name</param>
        /// <returns>id of artist with specified name</returns>
        int GetArtistIdByName(string name);

        /// <summary>
        /// Gets all artists
        /// </summary>
        /// <returns>all available artists</returns>
        IEnumerable<ArtistDTO> ListAllArtists();

        /// <summary>
        /// Gets all artists
        /// </summary>
        /// <param name="filter">artist filter</param>
        /// <param name="requiredPage">page to show</param>
        /// <returns>all available albums</returns>
        ArtistListQueryResultDTO ListAllArtists(ArtistFilter filter, int requiredPage);

        /// <summary>
        /// Gets the albums of the given artist
        /// </summary>
        /// <param name="artistId"> artist ID</param>
        /// <returns>all available albums</returns>
        IEnumerable<AlbumDTO> GetAllAlbumsForArtist(int artistId);
    }
}
