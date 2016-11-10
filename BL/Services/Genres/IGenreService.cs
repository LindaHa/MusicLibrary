using BL.DTOs.Filters;
using BL.DTOs.Genres;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Genres
{
    /// <summary>
    /// Handles Genre CRUD
    /// </summary>  
    public interface IGenreService : ICreator
    {
        int PageSize { get; }

        /// <summary>
        /// Creates new genre
        /// </summary>
        /// <param name="genreDTO">genre details</param>
        void CreateGenre(GenreDTO genreDTO);

        /// <summary>
        /// Updates Genre according to ID
        /// </summary>
        /// <param name="GenreDTO">genre details</param>
        void EditGenre(GenreDTO genreDTO);

        /// <summary>
        /// Removes genre according to ID
        /// </summary>
        /// <param name="genreId">genre ID</param>
        void DeleteGenre(int genreId);

        /// <summary>
        /// Gets genre according to ID
        /// </summary>
        /// <param name="genreId">genre ID</param>
        /// <returns>The genre</returns>
        GenreDTO GetGenre(int genreId);

        /// <summary>
        /// Gets genre id that belongs to specified genre name
        /// </summary>
        /// <param name="name">genre name</param>
        /// <returns>id of genre with specified name</returns>
        int GetGenreIdByName(string name);

        /// <summary>
        /// Gets all genres
        /// </summary>
        /// <returns>all available genres</returns>
        IEnumerable<GenreDTO> ListAllGenres();

        /// <summary>
        /// Gets all genres
        /// </summary>
        /// <param name="filter">genre filter</param>
        /// <param name="requiredPage">page to show</param>
        /// <returns>all available genres</returns>
        GenreListQueryResultDTO ListAllGenres(GenreFilter filter, int requiredPage);
    }
}
