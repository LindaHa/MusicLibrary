using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.Song_Songlists;
using BL.Repositories;
using BL.Queries;
using AutoMapper;
using DAL.Entities;
using BL.DTOs.Filters;
using BL.DTOs.Songlists;
using BL.DTOs.Songs;
using BL.DTOs.Clients;
using BL.Repositories.UserAccount;

namespace BL.Services.Song_Songlists
{
    public class Song_SonglistService : MusicLibraryService, ISong_SonglistService
    {
        public int PageSize => 9;

        private Song_SonglistListQuery song_songlistListQuery;
        private Song_SonglistRepository song_songlistRepository;
        private SongListQuery songListQuery;
        private SonglistListQuery songlistListQuery;
        private SongRepository songRepository;
        private SonglistRepository songlistRepository;
        private ClientRepository clientRepository;

        public Song_SonglistService(Song_SonglistRepository song_songlistRepository, Song_SonglistListQuery song_songlistListQuery, 
            SongRepository songRepository, SongListQuery songListQuery, SonglistListQuery songlistListQuery, 
            SonglistRepository songlistRepository, ClientRepository clientRepository)
        {
            this.song_songlistListQuery = song_songlistListQuery;
            this.song_songlistRepository = song_songlistRepository;
            this.songRepository = songRepository;
            this.songListQuery = songListQuery;
            this.songlistRepository = songlistRepository;
            this.songlistListQuery = songlistListQuery;
            this.clientRepository = clientRepository;
        }

        public void CreateSong_Songlist(Song_SonglistDTO song_songlistDTO)
        {
            if (song_songlistDTO == null)
                throw new ArgumentNullException("Song_Songlist Service - CreateSong_Songlist(...) song_songlistDTO cannot be null");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var song_songlist = Mapper.Map<Song_Songlist>(song_songlistDTO);
                song_songlist.Songlist = GetSongSonglist(song_songlistDTO.SonglistID);
                song_songlist.Song = GetSonglistSong(song_songlistDTO.SongID);
                song_songlist.Creator = GetSong_SonglistCreator(song_songlistDTO.CreatorID);

                song_songlistRepository.Insert(song_songlist);
                uow.Commit();
            }
        }

        public void DeleteSong_Songlist(int song_songlistId)
        {
            if (song_songlistId < 1)
                throw new ArgumentOutOfRangeException("Song_Songlist Service - DeleteSong_Songlist(...) song_songlistId cannot be lesser than 1");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var s_s = song_songlistRepository.GetByID(song_songlistId);
                if (s_s == null)
                {
                    throw new NullReferenceException("Songlist review service - DeleteReview(...) songlist to be deleted is null");
                }
                song_songlistRepository.Delete(s_s);
                uow.Commit();
            };
        }

        public void EditSong_Songlist(Song_SonglistDTO song_songlistDTO, int songlistID, int songID)
        {
            if (song_songlistDTO == null)
                throw new ArgumentNullException("Song_Songlist Service - EditSong_Songlist(...) song_songlistDTO cannot be null");
            if (songlistID < 1)
                throw new ArgumentOutOfRangeException("Song_Songlist Service - EditSong_Songlist(...) songlistID cannot be lesser than 1");
            if (songID < 1)
                throw new ArgumentOutOfRangeException("Song_Songlist Service - EditSong_Songlist(...) songID cannot be lesser than 1");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var song_songlist = song_songlistRepository.GetByID(song_songlistDTO.ID);
                Mapper.Map(song_songlistDTO, song_songlist);

                if (song_songlist == null)
                    throw new NullReferenceException ("Song_Songlist Service - EditSong_Songlist(...) cannot be null");

                song_songlist.Songlist = GetSongSonglist(songID);
                    song_songlist.Song = GetSonglistSong(songlistID);
                song_songlistRepository.Update(song_songlist);
                uow.Commit();
            };
        }
        
        public Song_SonglistDTO GetSong_Songlist(int song_songlistId)
        {
            if (song_songlistId < 1)
                throw new ArgumentOutOfRangeException("Song_Songlist Service - GetSong_Songlist(...) song_songlistId cannot be lesser than 1");
            using (UnitOfWorkProvider.Create())
            {
                var song_songlist = song_songlistRepository.GetByID(song_songlistId);
                if (song_songlist == null)
                {
                    throw new NullReferenceException("Album_GenreService - GetSong_Songlist(...) the song_songlist is null");
                }
                return Mapper.Map<Song_SonglistDTO>(song_songlist);
            }
        }


        public IEnumerable<SonglistDTO> GetAllSonglistsForSong(int songId)
        {
            if (songId < 1)
                throw new ArgumentOutOfRangeException("Song_Songlist Service - GetAllSonglistsForSong(...) songId cannot be lesser than 1");

            List<SonglistDTO> listDTO = new List<SonglistDTO>();

            using (UnitOfWorkProvider.Create())
            {
                song_songlistListQuery.Filter = new Song_SonglistFilter { SongID = songId };
                var songlist = song_songlistListQuery.Context.Songlists.Include(nameof(Songlist.Songs));
                foreach(var item in songlist)
                {
                    listDTO.Add(Mapper.Map<SonglistDTO>(item));
                }
                return listDTO;
            }
        }

        public IEnumerable<SongDTO> GetAllSongsForSonglist(int songlistId)
        {
            if (songlistId < 1)
                throw new ArgumentOutOfRangeException("Song_Songlist Service - GetAllSongsForSonglist(...) songlistId cannot be lesser than 1");

            List<SongDTO> listDTO = new List<SongDTO>();

            using (UnitOfWorkProvider.Create())
            {
                song_songlistListQuery.Filter = new Song_SonglistFilter { SonglistID = songlistId };
                var songs = song_songlistListQuery.Context.Songs
                                                    .Include(nameof(Song.Album))
                                                    .Include(nameof(Song.Reviews));
                foreach (var item in songs)
                {
                    listDTO.Add(Mapper.Map<SongDTO>(item));
                }
                return listDTO;
            }
        }

        public IEnumerable<Song_SonglistDTO> ListAllSong_Songlists()
        {
            using (UnitOfWorkProvider.Create())
            {
                song_songlistListQuery.Filter = null;
                return song_songlistListQuery.Execute() ?? new List<Song_SonglistDTO>();
            }
        }

        public ClientDTO GetCreator(int song_songlistID)
        {
            if (song_songlistID < 1)
                throw new ArgumentOutOfRangeException("Song_Songlist Service - GetCreator(...) song_songlistID cannot be lesser than 1");

            using (UnitOfWorkProvider.Create())
            {
                var song_songlist = song_songlistRepository.GetByID(song_songlistID);
                if (song_songlist == null || song_songlist.Creator == null)
                {
                    throw new NullReferenceException("Song_Songlist service - GetCreator(...) song_songlist or creator is null");
                }
                return Mapper.Map<ClientDTO>(song_songlist.Creator);
            }
        }

        /// <summary>
        /// Gets the songlist according to the ID
        /// </summary>
        /// <param name="songlistId">The songlist id</param>
        /// <returns>songlist according to ID</returns>
        private Songlist GetSongSonglist(int songlistID)
        {
            if (songlistID < 1)
                throw new ArgumentOutOfRangeException("Song_Songlist Service - GetSongSonglist(...) songlistID cannot be lesser than 1");

            var songlist = songlistRepository.GetByID(songlistID);
            if (songlist == null)
            {
                throw new NullReferenceException("Song_SonglistService - GetSonglistOfSong(...) the songlist is null");
            }
            return songlist;
        }

        /// <summary>
        /// Gets the song according to the ID
        /// </summary>
        /// <param name="songId">The song id</param>
        /// <returns>song according to ID</returns>
        private Song GetSonglistSong(int songID)
        {
            if (songID < 1)
                throw new ArgumentOutOfRangeException("Song_Songlist Service - GetSonglistSong(...) songID cannot be lesser than 1");

            var song = songRepository.GetByID(songID);
            if (song == null)
            {
                throw new NullReferenceException("Song_SonglistService - GetSongOfSonglist(...) the song is null");
            }
            return song;
        }

        /// <summary>
        /// Gets a creator according to the ID
        /// </summary>
        /// <param name="creatorID">The creator ID</param>
        /// <returns>creator according to ID</returns>
        private Client GetSong_SonglistCreator(int creatorID)
        {
            if (creatorID < 1)
                throw new ArgumentOutOfRangeException("Song_Songlist service - GetSong_SonglistCreator(...) creatorID cannot be lesser than 1");

            var creator = clientRepository.GetByID(creatorID);
            if (creator == null)
            {
                throw new NullReferenceException("Song_Songlist service - GetSong_SonglistCreator(...) creator cannot be null");
            }
            return creator;
        }
    }
}
