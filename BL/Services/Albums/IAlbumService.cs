using BL.DTOs.AlbumReviews;
using BL.DTOs.Albums;
using BL.DTOs.Artists;
using BL.DTOs.Clients;
using BL.DTOs.Filters;
using BL.DTOs.Songs;
using System.Collections.Generic;

namespace BL.Services.Albums
{
    /// <summary>
    /// Handles Album CRUD
    /// </summary>  
    public interface IAlbumService : ICreator
    {
        int PageSize { get; }

        /// <summary>
        /// Creates new album
        /// </summary>
        /// <param name="albumDTO">album details</param>
        void CreateAlbum(AlbumDTO albumDTO);


        /// <summary>
        /// Updates Album according to ID
        /// </summary>
        /// <param name="albumDTO">album details</param>
        /// <param name="artistId">artist ID</param>
        /// <param name="albumReviewIds">album review IDs</param>
        void EditAlbum(AlbumDTO albumDTO, int artistId, List<int> albumReviewIds, List<int> songIds);

        /// <summary>
        /// Removes album according to ID
        /// </summary>
        /// <param name="albumId">album ID</param>
        void DeleteAlbum(int albumId);

        /// <summary>
        /// Gets album according to ID
        /// </summary>
        /// <param name="albumId">album ID</param>
        /// <returns>The album</returns>
        AlbumDTO GetAlbum(int albumId);

        /// <summary>
        /// Gets album ID that belongs to specified album name
        /// </summary>
        /// <param name="name">album name</param>
        /// <returns>id of album with specified name</returns>
        int GetAlbumIdByName(string name);

        /// <summary>
        /// Gets all albums
        /// </summary>
        /// <returns>all available albums</returns>
        IEnumerable<AlbumDTO> ListAllAlbums();

        /// <summary>
        /// Gets all albums
        /// </summary>
        /// <param name="filter">album filter</param>
        /// <param name="requiredPage">page to show</param>
        /// <returns>all available albums</returns>
        AlbumListQueryResultDTO ListAllAlbums(AlbumFilter filter, int requiredPage);

        /// <summary>
        /// Gets the reviews of the given album
        /// </summary>
        /// <param name="albumId"> album ID</param>
        /// <returns>all available reviews</returns>
        IEnumerable<AlbumReviewDTO> GetAllAlbumReviews(int albumId);

        /// <summary>
        /// Gets the songs of the given album
        /// </summary>
        /// <param name="albumId"> album ID</param>
        /// <returns>all available songs</returns>
        IEnumerable<SongDTO> GetAllAlbumSongs(int albumId);

        /// <summary>
        /// Gets the artist of the given album
        /// </summary>
        /// <param name="albumId"> album ID</param>
        /// <returns>album artist</returns>
        ArtistDTO GetArtistOfAlbum(int albumId);

        /// <summary>
        /// Adds an album
        /// </summary>
        /// <param name="albumDTO">Album details</param>
        void AddAlbum(AlbumDTO albumDTO);
    }
}
