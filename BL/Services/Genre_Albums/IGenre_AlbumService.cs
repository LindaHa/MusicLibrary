using BL.DTOs.Albums;
using BL.DTOs.Genre_Albums;
using BL.DTOs.Genres;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Genre_Albums
{
    /// <summary>
    /// Handles Genre_Album CRUD
    /// </summary>  
    public interface IGenre_AlbumService : ICreator
    {
        int PageSize { get; }

        /// <summary>
        /// Creates new genre_album
        /// </summary>
        /// <param name="genre_albumDTO">genre_album details</param>
        void CreateGenre_Album(Genre_AlbumDTO genre_albumDTO);

        /// <summary>
        /// Updates Genre_Album according to ID
        /// </summary>
        /// <param name="Genre_AlbumDTO">genre_album details</param>
        void EditGenre_Album(Genre_AlbumDTO genre_albumDTO, int albumID, int genreID);

        /// <summary>
        /// Removes genre_album according to ID
        /// </summary>
        /// <param name="genre_albumId">genre_album ID</param>
        void DeleteGenre_Album(int genre_albumId);

        /// <summary>
        /// Gets genre IDs according to album ID
        /// </summary>
        /// <param name="albumId">album ID</param>
        /// <returns>The IDs of genres of the given album</returns>
        IEnumerable<GenreDTO> GetAllGenresForAlbum(int albumId);

        /// <summary>
        /// Gets album IDs according to genre ID
        /// </summary>
        /// <param name="genreId">genre ID</param>
        /// <returns>The IDs of albums of the given genre</returns>
        IEnumerable<AlbumDTO> GetAllAlbumsForGenre(int genreId);

        /// <summary>
        /// Gets all genre_albums
        /// </summary>
        /// <returns>all available genre_albums </returns>
        IEnumerable<Genre_AlbumDTO> ListAllGenre_Albums();

        /// <summary>
        /// Gets genre_album according to ID
        /// </summary>
        /// <param name="genre_albumId">genre_album ID</param>
        /// <returns>genre_album according to ID</returns>
        Genre_AlbumDTO GetGenre_Album(int genre_albumId);
    }
}
