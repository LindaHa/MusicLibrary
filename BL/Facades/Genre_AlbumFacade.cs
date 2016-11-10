
using BL.DTOs.Albums;
using BL.DTOs.Genre_Albums;
using BL.DTOs.Genres;
using BL.DTOs.Songs;
using BL.Services.Albums;
using BL.Services.Genre_Albums;
using BL.Services.Genres;
using System.Collections.Generic;

namespace BL.Repositories
{
    public class Genre_AlbumFacade
    {
        public int PageSize => genre_albumService.PageSize;

        private readonly IGenre_AlbumService genre_albumService;

        private readonly IAlbumService albumService;

        private readonly IGenreService genreService;


        public Genre_AlbumFacade(IGenre_AlbumService genre_albumService, IAlbumService albumService, IGenreService genreService)
        {
            this.genre_albumService = genre_albumService;
            this.albumService = albumService;
            this.genreService = genreService;
        }

        /// <summary>
        /// Makes the genre_album official
        /// </summary>
        /// <param name="genre_album">genre_albumDTO</param>
        public void MakeOfficial(Genre_AlbumDTO genre_albumDTO)
        {
            genre_albumDTO.IsOfficial = true;
            genre_albumService.EditGenre_Album(genre_albumDTO, genre_albumDTO.AlbumID, genre_albumDTO.GenreID);
        }

        /// <summary>
        /// Creates a genre_album with album and genre names
        /// </summary>
        /// <param name="genre_album">genre_album</param>
        public void CreateGenre_Album(Genre_AlbumDTO genre_album)
        {   
            genre_albumService.CreateGenre_Album(genre_album);
        }

        public void EditGenre_Album(Genre_AlbumDTO genre_albumDTO, int albumId, int genreID)
        {
            genre_albumService.EditGenre_Album(genre_albumDTO, albumId, genreID);
        }

        public void DeleteGenre_Album(int id)
        {
            genre_albumService.DeleteGenre_Album(id);
        }

        /// <summary>
        /// Gets genre_album according to ID
        /// </summary>
        /// <param name="genre_albumId">genre_album ID</param>
        /// <returns>The genre_album</returns>
        public Genre_AlbumDTO GetGenre_Album(int genre_albumId)
        {
            return genre_albumService.GetGenre_Album(genre_albumId);
        }

        /// <summary>
        /// Gets all genre_albums
        /// </summary>
        /// <returns>All available genre_albums</returns>
        public IEnumerable<Genre_AlbumDTO> GetAllGenre_Albums()
        {
            return genre_albumService.ListAllGenre_Albums();
        }

        /// <summary>
        /// Gets all genres of the specified album
        /// </summary>
        /// <param name="albumId">album ID</param>
        /// <returns>All available genres</returns>
        public IEnumerable<GenreDTO> GetAllGenresOfAlbum(int albumID)
        {
            return genre_albumService.GetAllGenresForAlbum(albumID);
        }

        /// <summary>
        /// Gets all albums of the specified genre
        /// </summary>
        /// <param name="genreId">genre ID</param>
        /// <returns>All available albums</returns>
        public IEnumerable<AlbumDTO> GetAllAlbumsForGenre(int genreId)
        {
            return genre_albumService.GetAllAlbumsForGenre(genreId);
        }
    }
}
