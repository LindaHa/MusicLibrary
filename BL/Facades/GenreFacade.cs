

using BL.DTOs.Genres;
using BL.DTOs.Songs;
using BL.Services.Genre_Albums;
using BL.Services.Genres;
using BL.Services.Songs;
using System.Collections.Generic;

namespace BL.Repositories
{
    public class GenreFacade
    {
        public int PageSize => genreService.PageSize;

        private readonly IGenreService genreService;
        private readonly IGenre_AlbumService genre_albumService;
        private readonly ISongService songService;

        public GenreFacade(IGenreService genreService, IGenre_AlbumService genre_albumService, ISongService songService)
        {
            this.genreService = genreService;
            this.genre_albumService = genre_albumService;
            this.songService = songService;
        }

        /// <summary>
        /// Makes the genre official
        /// </summary>
        /// <param name="genre">genreDTO</param>
        public void MakeOfficial(GenreDTO genreDTO)
        {
            genreDTO.IsOfficial = true;
            genreService.EditGenre(genreDTO);
        }

        /// <summary>
        /// Creates genre
        /// </summary>
        /// <param name="genre">genre</param>
        public void CreateGenreWithArtistName(GenreDTO genre)
        {
            genreService.CreateGenre(genre);
        }

        public void EditGenre(GenreDTO genreDTO)
        {
            genreService.EditGenre(genreDTO);
        }

        public void DeleteGenre(int id)
        {
            genreService.DeleteGenre(id);
        }

        /// <summary>
        /// Gets genre according to ID
        /// </summary>
        /// <param name="genreId">genre ID</param>
        /// <returns>The genre</returns>
        public GenreDTO GetGenre(int genreId)
        {
            return genreService.GetGenre(genreId);
        }

        /// <summary>
        /// Gets the genre id that belongs to a specified genre name
        /// </summary>
        /// <param name="name">genre name</param>
        /// <returns>id of genre with specified name</returns>
        public int GetGenreIdByName(string name)
        {
            return genreService.GetGenreIdByName(name);
        }

        /// <summary>
        /// Gets all genres
        /// </summary>
        /// <returns>All available genres</returns>
        public IEnumerable<GenreDTO> GetAllGenres()
        {
            return genreService.ListAllGenres();
        }



        /// <summary>
        /// Gets all songss of the specified genre
        /// </summary>
        /// <param name="genreId">genre ID</param>
        /// <returns>All available songs</returns>
        public IEnumerable<SongDTO> GetAllSongsForGenre(int genreId)
        {
            var albums = genre_albumService.GetAllAlbumsForGenre(genreId);
            List<SongDTO> songs = new List<SongDTO>();
            foreach (var album in albums)
            {
                foreach (var songID in album.SongIDs)
                {
                    songs.Add(songService.GetSong(songID));
                }
            }
            return songs;
        }
    }
}
