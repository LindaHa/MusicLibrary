using BL.DTOs.AlbumReviews;
using BL.DTOs.Albums;
using BL.DTOs.Songs;
using BL.Services.AlbumReviews;
using BL.Services.Albums;
using BL.Services.Artists;
using BL.Services.Songs;
using System.Collections.Generic;

namespace BL.Repositories
{
    public class AlbumFacade 
    {
        public int PageSize => albumService.PageSize;

        private readonly IAlbumService albumService;

        private readonly IArtistService artistService;

        private readonly IAlbumReviewService albumReviewService;

        private readonly ISongService songService;

        public AlbumFacade(IAlbumService albumService, IArtistService artistService, IAlbumReviewService albumReviewService, ISongService songService)
        {
            this.albumService = albumService;
            this.artistService = artistService;
            this.albumReviewService = albumReviewService;
            this.songService = songService;
        }

        /// <summary>
        /// Makes the album official
        /// </summary>
        /// <param name="album">albumDTO</param>
        public void MakeOfficial(AlbumDTO albumDTO)
        {
            albumDTO.IsOfficial = true;
            albumService.EditAlbum(albumDTO, albumDTO.ArtistID, albumDTO.ReviewIDs, albumDTO.SongIDs);
        }

        /// <summary>
        /// Creates album with artist that corresponds with given name
        /// </summary>
        /// <param name="album">album</param>
        /// <param name="artistName">artist name</param>
        public void CreateAlbumWithArtistName(AlbumDTO album, string artistName)
        {
            var artistId = artistService.GetArtistIdByName(artistName);
            albumService.CreateAlbum(album);
        }

        public void EditAlbum(AlbumDTO albumDTO, int artistId, int[] albumReviewIds, int[] songIds)
        {
            albumService.EditAlbum(albumDTO, artistId, albumReviewIds, songIds);
        }

        public void DeleteAlbum(int id)
        {
            albumService.DeleteAlbum(id);
        }

        /// <summary>
        /// Gets album according to ID
        /// </summary>
        /// <param name="albumId">album ID</param>
        /// <returns>The album</returns>
        public AlbumDTO GetAlbum(int albumId)
        {
            return albumService.GetAlbum(albumId);
        }

        /// <summary>
        /// Gets the album id that belongs to a specified album name
        /// </summary>
        /// <param name="name">album name</param>
        /// <returns>id of album with specified name</returns>
        public int GetAlbumIdByName(string name)
        {
            return albumService.GetAlbumIdByName(name);
        }

        /// <summary>
        /// Gets all albums
        /// </summary>
        /// <returns>All available albums</returns>
        public IEnumerable<AlbumDTO> GetAllAlbums()
        {
            return albumService.ListAllAlbums();
        }

        /// <summary>
        /// Gets all songss of the specified album
        /// </summary>
        /// <param name="albumId">album ID</param>
        /// <returns>All available songs</returns>
        public IEnumerable<SongDTO> GetAllSongsOfAlbum(int albumId)
        {
            return albumService.GetAllAlbumSongs(albumId);
        }

        public void AddSong(SongDTO songDTO)
        {
            songService.AddSong(songDTO);
        }




        public  void CreateReview(AlbumReviewDTO reviewDTO)
        {
            albumReviewService.CreateAlbumReview(reviewDTO);
        }

        public void AddReview(AlbumReviewDTO reviewDTO)
        {
            albumReviewService.AddReview(reviewDTO);
        }

        public void EditReview(AlbumReviewDTO reviewDTO)
        {
            albumReviewService.EditAlbumReview(reviewDTO);
        }

        public void DeleteReview(int reviewId)
        {
            albumReviewService.DeleteAlbumReview(reviewId);
        }

        /// <summary>
        /// Gets all reviews of the given album
        /// </summary>
        /// <param name="albumId">album id</param>
        /// <returns>all reviews of the given album</returns>
        public IEnumerable<AlbumReviewDTO> GetAllReviews(int albumId = 0)
        {
            return albumService.GetAllAlbumReviews(albumId);
        }
    }
}
