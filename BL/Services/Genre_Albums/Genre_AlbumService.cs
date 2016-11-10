using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.Genre_Albums;
using BL.Queries;
using BL.Repositories;
using AutoMapper;
using DAL.Entities;
using BL.DTOs.Filters;
using BL.DTOs.Albums;
using BL.DTOs.Genres;
using BL.DTOs.Clients;
using BL.Repositories.UserAccount;

namespace BL.Services.Genre_Albums
{
    public class Genre_AlbumService : MusicLibraryService, IGenre_AlbumService
    {
        public int PageSize => 9;

        private Genre_AlbumRepository genre_albumRepository;
        private Genre_AlbumListQuery genre_albumListQuery;
        private GenreListQuery genreListQuery;
        private AlbumListQuery albumListQuery;
        private GenreRepository genreRepository;
        private AlbumRepository albumRepository;
        private ClientRepository clientRepository;

        public Genre_AlbumService(Genre_AlbumRepository genre_albumRepository, Genre_AlbumListQuery genre_albumListQuery, 
            GenreRepository genreRepository, GenreListQuery genreListQuery, AlbumListQuery albumListQuery, 
            AlbumRepository albumRepository, ClientRepository clientRepository)
        {
            this.genre_albumListQuery = genre_albumListQuery;
            this.genre_albumRepository = genre_albumRepository;
            this.genreRepository = genreRepository;
            this.genreListQuery = genreListQuery;
            this.albumRepository = albumRepository;
            this.albumListQuery = albumListQuery;
            this.clientRepository = clientRepository;
        }

        public void CreateGenre_Album(Genre_AlbumDTO genre_albumDTO)
        {
            if (genre_albumDTO != null)
            {
                using (var uow = UnitOfWorkProvider.Create())
                {
                    var gen_al = Mapper.Map<Genre_Album>(genre_albumDTO);
                    gen_al.Album = GetGenreAlbum(genre_albumDTO.AlbumID);
                    gen_al.Genre = GetAlbumGenre(genre_albumDTO.GenreID);
                    gen_al.Creator = GetGenre_AlbumCreator(genre_albumDTO.CreatorID);

                    genre_albumRepository.Insert(gen_al);
                    uow.Commit();
                }
            } else
            {
                throw new ArgumentNullException("Genre_Album service - CreateGenre_Album(...) genre_albumDTO cannot be null");
            }
        }

        public void DeleteGenre_Album(int genre_albumId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if (genre_albumId > 0)
                {
                    var gen_al = genre_albumRepository.GetByID(genre_albumId);
                    if (gen_al == null)
                    {
                        throw new NullReferenceException("Genre_Album service - DeleteReview(...) album to be deleted is null");
                    }
                    genre_albumRepository.Delete(gen_al);
                    uow.Commit();
                } else
                {
                    throw new ArgumentOutOfRangeException("Genre_Album service - DeleteGenre_Album(...) genre_albumID cannot be null");
                }
            };
        }

        public void EditGenre_Album(Genre_AlbumDTO genre_albumDTO, int albumID, int genreID)
        {
            if (genre_albumDTO == null)
                throw new ArgumentNullException("Genre_Album service - EditGenre_Album(...) genre_albumDTO cannot be null");

            using (var uow = UnitOfWorkProvider.Create())
            {                
                var gen_al = genre_albumRepository.GetByID(genre_albumDTO.ID);
                Mapper.Map(genre_albumDTO, gen_al);

                if (gen_al != null)
                {
                    if (genreID > 0)
                    {
                        gen_al.Album = GetGenreAlbum(genreID);
                    }
                    else throw new ArgumentOutOfRangeException("Genre_AlbumService - Edit(...) genreID cannot be lesser than 1");
                    
                    if (albumID > 0)
                    {
                        gen_al.Genre = GetAlbumGenre(albumID);
                    }
                    else throw new ArgumentOutOfRangeException("Genre_AlbumService - Edit(...) albumID cannot be lesser than 1");
                }
                else
                {
                    throw new ArgumentNullException("Genre_AlbumService - Edit(...) genre_album cannot be Null");
                }

                genre_albumRepository.Update(gen_al);
                uow.Commit();
            };
        }

        public Genre_AlbumDTO GetGenre_Album(int genre_albumId)
        {
            if(genre_albumId < 1)
                throw new ArgumentOutOfRangeException("Genre_AlbumService - GetGenre_Album(...) genreID cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                var genre_album = genre_albumRepository.GetByID(genre_albumId);
                if (genre_album == null)
                    throw new ArgumentNullException("Genre_AlbumService - GetGenre_Album(...) genre_album cannot be Null");
                return Mapper.Map<Genre_AlbumDTO>(genre_album);
                ;
            }
        }

        public IEnumerable<AlbumDTO> GetAllAlbumsForGenre(int genreId)
        {
            if(genreId < 1)
                throw new ArgumentOutOfRangeException("Genre_AlbumService - GetAllAlbumsForGenre(...) genreID cannot be lesser than 1");

            List<AlbumDTO> listDTO = new List<AlbumDTO>();
            using (UnitOfWorkProvider.Create())
            {
                genre_albumListQuery.Filter = new Genre_AlbumFilter { GenreID = genreId };
                var list = genre_albumListQuery.Context.Albums
                                                            .Include(nameof(Album.Songs))
                                                            .Include(nameof(Album.Reviews))
                                                            .Include(nameof(Album.Artist));
                foreach (var item in list)
                {                    
                    listDTO.Add(Mapper.Map<AlbumDTO>(item));
                }                
                return listDTO;
            }
        }

        public IEnumerable<GenreDTO> GetAllGenresForAlbum(int albumId)
        {
            if(albumId < 1)
                throw new ArgumentOutOfRangeException("Genre_AlbumService - GetAllGenresForAlbum(...) albumId cannot be lesser than 1");

            List<GenreDTO> listDTO = new List<GenreDTO>();
            using (UnitOfWorkProvider.Create())
            {
                genre_albumListQuery.Filter = new Genre_AlbumFilter { AlbumID = albumId };
                var list = genre_albumListQuery.Context.Genres;
                foreach (var item in list)
                {
                    listDTO.Add(Mapper.Map<GenreDTO>(item));
                }
                return listDTO;
            }
        }

        public IEnumerable<Genre_AlbumDTO> ListAllGenre_Albums()
        {
            using (UnitOfWorkProvider.Create())
            {
                genre_albumListQuery.Filter = null;
                return genre_albumListQuery.Execute() ?? new List<Genre_AlbumDTO>();
            }
        }

        public ClientDTO GetCreator(int genre_albumID)
        {
            if (genre_albumID < 1)
                throw new ArgumentOutOfRangeException("Genre_AlbumService - GetCreator(...) genre_albumID cannot be lesser than 1");
            using (UnitOfWorkProvider.Create())
            {
                var genre_album = genre_albumRepository.GetByID(genre_albumID);
                if (genre_album == null || genre_album.Creator == null)
                {
                    throw new NullReferenceException("Genre_Album service - GetCreator(...) genre_album or creator is null");
                }
                return Mapper.Map<ClientDTO>(genre_album.Creator);
            }
        }

        /// <summary>
        /// Gets the album for a corresponding genre according to the ID
        /// </summary>
        /// <param name="albumId">The album id</param>
        /// <returns>album according to ID</returns>
        private Album GetGenreAlbum(int albumID)
        {
            var album = albumRepository.GetByID(albumID);
            if (album == null)
            {
                throw new NullReferenceException("Album_GenreService - GetAlbumOfGenre(...) the album is null");
            }
            return album;
        }

        /// <summary>
        /// Gets the genre for a corresponding album according to the ID
        /// </summary>
        /// <param name="genreId">The genre id</param>
        /// <returns>genre according to ID</returns>
        private Genre GetAlbumGenre(int genreID)
        {
            var genre = genreRepository.GetByID(genreID);
            if (genre == null)
            {
                throw new NullReferenceException("Album_GenreService - GetGenreOfAlbum(...) the genre is null");
            }
            return genre;
        }

        /// <summary>
        /// Gets a creator according to the ID
        /// </summary>
        /// <param name="creatorID">The creator ID</param>
        /// <returns>creator according to ID</returns>
        private Client GetGenre_AlbumCreator(int creatorID)
        {
            if (creatorID < 1)
                throw new ArgumentOutOfRangeException("Genre_Album service - GetGenre_AlbumCreator(...) creatorID cannot be lesser than 1");

            var creator = clientRepository.GetByID(creatorID);
            if (creator == null)
            {
                throw new NullReferenceException("Genre_Album service - GetGenre_AlbumCreator(...) creator cannot be null");
            }
            return creator;
        }

    }
}
