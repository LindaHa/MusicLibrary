using System;
using System.Collections.Generic;
using System.Linq;
using BL.DTOs.Filters;
using BL.DTOs.Songlists;
using BL.DTOs.Songs;
using AutoMapper;
using DAL.Entities;
using BL.Queries;
using BL.Repositories;
using Riganti.Utils.Infrastructure.Core;
using BL.DTOs.Clients;
using BL.Repositories.UserAccount;

namespace BL.Services.Songlists
{
    public class SonglistService : MusicLibraryService, ISonglistService
    {
        public int PageSize => 9;

        private SonglistListQuery songlistListQuery;
        private SonglistRepository songlistRepository;
        private SongListQuery songListQuery;
        private SongRepository songRepository;
        private ArtistRepository artistRepository;
        private ClientRepository clientRepository;

        public SonglistService(SonglistRepository songlistRepository, SonglistListQuery songlistListQuery, SongListQuery songListQuery, 
            SongRepository songRepository, ArtistRepository artistRepository, ClientRepository clientRepository)
        {
            this.songlistRepository = songlistRepository;
            this.songlistListQuery = songlistListQuery;
            this.songRepository = songRepository;
            this.songListQuery = songListQuery;
            this.artistRepository = artistRepository;
            this.clientRepository = clientRepository;
        }


        public void CreateSonglist(SonglistDTO songlistDTO)
        {
            if (songlistDTO == null)
                throw new ArgumentNullException("Songlist Service - CreateSonglist(...) songlistDTO cannot be null");

            Songlist songlist = Mapper.Map<Songlist>(songlistDTO);
            using (var uow = UnitOfWorkProvider.Create())
            {
                if (songlistDTO.SongIDs != null)
                {
                    foreach (int ID in songlistDTO.SongIDs)
                    {
                        if (songlist.Songs == null) songlist.Songs = new List<Song>();
                        Song song = GetSonglistSong(ID);
                        songlist.Songs.Add(song);
                    }
                }

                songlist.Owner = GetSonglistCreator(songlistDTO.OwnerID);

                songlistRepository.Insert(songlist);
                uow.Commit();
            }
        }

        public void DeleteSonglist(int songlistId)
        {
            if (songlistId < 1)
                throw new ArgumentOutOfRangeException("Songlist Service - DeleteSonglist(...) songlistId cannot be lesser than 1");

            using (var uow = UnitOfWorkProvider.Create())
            {
                songlistRepository.Delete(songlistId);
                uow.Commit();
            }
        }

        public void EditSonglist(SonglistDTO songlistDTO, List<int> songIds)
        {
            if (songlistDTO == null)
                throw new ArgumentNullException("Songlist Service - EditSonglist(...) songlistDTO cannot be null");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var songlist = songlistRepository.GetByID(songlistDTO.ID);

                if (songIds != null && songIds.Any())
                {
                    var songs = songRepository.GetByIDs(songIds);
                    songlist.Songs.RemoveAll(song => !songs.Contains(song));
                    songlist.Songs.AddRange(
                        songs.Where(song => !songlist.Songs.Contains(song)));
                }
                else
                {
                    songlist.Songs.Clear();
                }

                songlistRepository.Update(songlist);
                uow.Commit();
            }
        }

        public SonglistDTO GetSonglist(int songlistId)
        {
            if (songlistId < 1)
                throw new ArgumentOutOfRangeException("Songlist Service - GetSonglist(...) songlistId cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                var songlist = songlistRepository.GetByID(songlistId);
                return songlist != null ? Mapper.Map<SonglistDTO>(songlist) : null;
            }
        }

        public int GetSonglistIdByName(string name)
        {
            if(name == null)
                throw new ArgumentNullException("Songlist Service - GetSonglistIdByName(...) name cannot be null");

            using (UnitOfWorkProvider.Create())
            {
                songlistListQuery.Filter = new SonglistFilter { Name = name };
                var songlist = songlistListQuery.Execute().SingleOrDefault();
                if (songlist == null)
                    throw new ArgumentNullException("Songlist Service - GetSonglistIdByName(...) songlistDTO cannot be null");

                return songlist?.ID ?? 0;
            }
        }

        public IEnumerable<SongDTO> GetSonglistSongs(int songlistID)
        {
            if (songlistID < 1)
                throw new ArgumentOutOfRangeException("Songlist Service - GetSonglistSongs(...) songlistID cannot be lesser than 1");

            List<SongDTO> songsDTO = new List<SongDTO>();
            List<Song> songs;
            Songlist songlist;
            using (UnitOfWorkProvider.Create())
            {
                songlist = songlistRepository.GetByID(songlistID);
                if (songlist == null)
                    throw new ArgumentNullException("Songlist Service - GetSonglistSongs(...) songlist cannot be null");

                songs = songlist.Songs;
                foreach(var item in songs)
                {
                    songsDTO.Add(Mapper.Map<SongDTO>(item));
                }

                return songListQuery.Execute();
            }
        }

        public SonglistListQueryResultDTO ListAllSonglists(SonglistFilter filter, int requiredPage = 1)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = GetQuery(filter);
                query.Skip = Math.Max(0, requiredPage - 1) * PageSize;
                query.Take = PageSize;

                var sortOrder = filter.SortAscending ? SortDirection.Ascending : SortDirection.Descending;
                query.AddSortCriteria(songlist => songlist.Name, sortOrder);

                return new SonglistListQueryResultDTO
                {
                    RequestedPage = requiredPage,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultsPage = query.Execute(),
                    Filter = filter
                };
            }
        }

        public IEnumerable<SonglistDTO> ListAllSonglists()
        {
            using (UnitOfWorkProvider.Create())
            {
                songlistListQuery.Filter = null;
                return songlistListQuery.Execute() ?? new List<SonglistDTO>();
            }
        }

        public ClientDTO GetCreator(int songlistID)
        {
            if (songlistID < 1)
                throw new ArgumentNullException("Snoglist service - GetCreator(...) songlistID cannot bew lesser than 1");
            using (UnitOfWorkProvider.Create())
            {
                var songlist = songlistRepository.GetByID(songlistID);
                if (songlist == null)
                {
                    throw new NullReferenceException("Snoglist service - GetCreator(...) songlist cannot be null");
                }
                return Mapper.Map<ClientDTO>(songlist.Owner);
            }
        }

        /// <summary>
        /// Configures songlist list query
        /// </summary>
        /// <param name="filter">songlist filter</param>
        /// <returns>configured query</returns>
        private IQuery<SonglistDTO> GetQuery(SonglistFilter filter)
        {
            var query = songlistListQuery;
            query.ClearSortCriterias();
            query.Filter = filter;
            return query;
        }

        /// <summary>
        /// Gets a song according to the ID
        /// </summary>
        /// <param name="songID">The song IDI</param>
        /// <returns>song according to ID</returns>
        private Song GetSonglistSong(int songID)
        {
            var song = songRepository.GetByID(songID);
            if (song == null)
            {
                throw new NullReferenceException("Songlist service - GetSonglistSong(...) song cannot be null");
            }
            return song;
        }

        /// <summary>
        /// Gets a creator according to the ID
        /// </summary>
        /// <param name="creatorID">The creator ID</param>
        /// <returns>creator according to ID</returns>
        private Client GetSonglistCreator(int creatorID)
        {
            if (creatorID < 1)
                throw new ArgumentOutOfRangeException("Songlist service - GetSonglistCreator(...) creatorID cannot be lesser than 1");

            var creator = clientRepository.GetByID(creatorID);
            if (creator == null)
            {
                throw new NullReferenceException("Songlist service - GetSonglistCreator(...) creator cannot be null");
            }
            return creator;
        }
    }
}
