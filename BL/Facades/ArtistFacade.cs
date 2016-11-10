using BL.DTOs.Albums;
using BL.DTOs.Artists;
using BL.DTOs.Songs;
using BL.Services.Albums;
using BL.Services.Artists;
using BL.Services.Songs;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using System.Collections.Generic;

namespace BL.Repositories
{
    public class ArtistFacade
    {
        public int PageSize => artistService.PageSize;

        private readonly IArtistService artistService;

        private readonly IAlbumService albumService;

        private readonly ISongService songService;

        public ArtistFacade(IArtistService artistService, IAlbumService albumService, ISongService songService)
        {
            this.artistService = artistService;
            this.albumService = albumService;
            this.songService = songService;
        }

        /// <summary>
        /// Makes the artist official
        /// </summary>
        /// <param name="artist">artistDTO</param>
        public void MakeOfficial(ArtistDTO artistDTO)
        {
            artistDTO.IsOfficial = true;
            artistService.EditArtist(artistDTO, artistDTO.AlbumIDs);
        }

        /// <summary>
        /// Creates an artist 
        /// </summary>
        /// <param name="artist">artist</param>
        public void CreateArtist(ArtistDTO artist)
        {            
            artistService.CreateArtist(artist);
        }

        public void EditArtist(ArtistDTO artistDTO, List<int> albumIds)
        {
            artistService.EditArtist(artistDTO, albumIds);
        }

        public void DeleteArtist(int id)
        {
            artistService.DeleteArtist(id);
        }

        /// <summary>
        /// Gets artist according to ID
        /// </summary>
        /// <param name="artistId">artist ID</param>
        /// <returns>The artist</returns>
        public ArtistDTO GetArtist(int artistId)
        {
            return artistService.GetArtist(artistId);
        }

        /// <summary>
        /// Gets the artist id that belongs to a specified artist name
        /// </summary>
        /// <param name="name">artist name</param>
        /// <returns>id of artist with specified name</returns>
        public int GetArtistIdByName(string name)
        {
            return artistService.GetArtistIdByName(name);
        }

        /// <summary>
        /// Gets all artists
        /// </summary>
        /// <returns>All available artists</returns>
        public IEnumerable<ArtistDTO> GetAllArtists()
        {
            return artistService.ListAllArtists();
        }


        public void AddAlbum(AlbumDTO albumDTO)
        {
            albumService.AddAlbum(albumDTO);
        }

        /// <summary>
        /// Gets all songs of the specified artist
        /// </summary>
        /// <param name="artistId">artist ID</param>
        /// <returns>All available songs</returns>
        public IEnumerable<SongDTO> GetAllSongsForArtist(int artistId)
        {
            var albums = artistService.GetAllAlbumsForArtist(artistId);
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
