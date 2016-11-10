using AutoMapper;
using BL.DTOs.Clients;
using BL.DTOs.Filters;
using BL.DTOs.Genres;
using BL.Queries;
using BL.Repositories;
using BL.Repositories.UserAccount;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Genres
{
    public class GenreService : MusicLibraryService, IGenreService
    {
        public int PageSize => 9;

        private GenreListQuery genreListQuery;
        private GenreRepository genreRepository;
        private ClientRepository clientRepository;

        public GenreService(GenreRepository genreRepository, GenreListQuery genreListQuery, ClientRepository clientRepository)
        {
            this.genreRepository = genreRepository;
            this.genreListQuery = genreListQuery;
            this.clientRepository = clientRepository;
        }

        public void CreateGenre(GenreDTO genreDTO)
        {
            if (genreDTO == null)
                throw new ArgumentNullException("Genre Service - CreateGenre(...) genreDTO cannot be null");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var genre = Mapper.Map<Genre>(genreDTO);
                genre.Creator = GetGenreCreator(genreDTO.CreatorID);

                genreRepository.Insert(genre);
                uow.Commit();
            }
        }

        public void DeleteGenre(int genreId)
        {
            if (genreId < 1)
                throw new ArgumentOutOfRangeException("Genre Service - DeleteGenre(...) genreId cannot be lesser than 1");

            using (var uow = UnitOfWorkProvider.Create())
            {
                genreRepository.Delete(genreId);
                uow.Commit();
            }
        }

        public void EditGenre(GenreDTO genreDTO)
        {
            if (genreDTO == null)
                throw new ArgumentNullException("Genre Service - EditGenre(...) genreDTO cannot be null");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var genre = genreRepository.GetByID(genreDTO.ID);
                Mapper.Map(genreDTO, genre);

                genreRepository.Update(genre);
                uow.Commit();
            }
        }

        public GenreDTO GetGenre(int genreId)
        {
            if (genreId < 1)
                throw new ArgumentOutOfRangeException("Genre Service - GetGenre(...) genreId cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                var genre = genreRepository.GetByID(genreId);
                return genre != null ? Mapper.Map<GenreDTO>(genre) : null;
            }
        }

        public int GetGenreIdByName(string name)
        {
            using (UnitOfWorkProvider.Create())
            {
                genreListQuery.Filter = new GenreFilter { Name = name };
                var genre = genreListQuery.Execute().SingleOrDefault();
                
                return genre?.ID ?? 0;
            }
        }

        public IEnumerable<GenreDTO> ListAllGenres()
        {
            using (UnitOfWorkProvider.Create())
            {
                genreListQuery.Filter = null;
                return genreListQuery.Execute() ?? new List<GenreDTO>();
            }
        }

        public GenreListQueryResultDTO ListAllGenres(GenreFilter filter, int requiredPage = 1)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = GetQuery(filter);
                query.Skip = Math.Max(0, requiredPage - 1) * PageSize;
                query.Take = PageSize;

                var sortOrder = filter.SortAscending ? SortDirection.Ascending : SortDirection.Descending;
                query.AddSortCriteria(genre => genre.Name, sortOrder);

                return new GenreListQueryResultDTO
                {
                    RequestedPage = requiredPage,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultsPage = query.Execute(),
                    Filter = filter
                };
            }
        }

        public ClientDTO GetCreator(int genreID)
        {
            if (genreID < 1)
                throw new ArgumentOutOfRangeException("Genre Service - GetCreator(...) genreId cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                var genre = genreRepository.GetByID(genreID);
                if (genre == null)
                {
                    throw new NullReferenceException("Genre service - GetCreator(...) genre cannot be null");
                }
                return Mapper.Map<ClientDTO>(genre.Creator);
            }
        }

        /// <summary>
        /// Configures genres list query
        /// </summary>
        /// <param name="filter">genre filter</param>
        /// <returns>configured query</returns>
        private IQuery<GenreDTO> GetQuery(GenreFilter filter)
        {
            var query = genreListQuery;
            query.ClearSortCriterias();
            query.Filter = filter;
            return query;
        }

        /// <summary>
        /// Gets a creator according to the ID
        /// </summary>
        /// <param name="creatorID">The creator ID</param>
        /// <returns>creator according to ID</returns>
        private Client GetGenreCreator(int creatorID)
        {
            if (creatorID < 1)
                throw new ArgumentOutOfRangeException("Genre service - GetGenreCreator(...) creatorID cannot be lesser than 1");

            var creator = clientRepository.GetByID(creatorID);
            if (creator == null)
            {
                throw new NullReferenceException("Genre service - GetGenreCreator(...) creator cannot be null");
            }
            return creator;
        }
    }
}
