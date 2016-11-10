using BL.DTOs.SongReviews;
using BL.DTOs.Songs;
using BL.Services.Albums;
using BL.Services.Genre_Albums;
using BL.Services.SongReviews;
using BL.Services.Songs;
using System.Collections.Generic;

namespace BL.Repositories
{
    public class SongFacade
    {
         public int PageSize => songService.PageSize;

        private readonly ISongService songService;

        private readonly IAlbumService albumService;

        private readonly ISongReviewService songReviewService;

        private readonly IGenre_AlbumService genre_albumService;

        public SongFacade(ISongService songService, IAlbumService albumService, ISongReviewService songReviewService, IGenre_AlbumService genre_albumService)
        {
            this.songService = songService;
            this.albumService = albumService;
            this.songReviewService = songReviewService;
            this.genre_albumService = genre_albumService;
        }

        /// <summary>
        /// Creates a song with an album that corresponds with given name
        /// </summary>
        /// <param name="song">songDTO</param>
        /// <param name="albumName">album name</param>
        public void CreateSongWithAlbumName(SongDTO song, string albumName)
        {
            var albumId = albumService.GetAlbumIdByName(albumName);
            songService.CreateSong(song);
        }

        /// <summary>
        /// Makes the song official
        /// </summary>
        /// <param name="song">songDTO</param>
         public void MakeOfficial(SongDTO songDTO)
        {
            songDTO.IsOfficial = true;
            songService.EditSong(songDTO, songDTO.AlbumID, songDTO.ReviewIDs);
        }

        public void EditSong(SongDTO songDTO, int albumId, params int[] songReviewIds)
        {
            songService.EditSong(songDTO, albumId, songReviewIds);
        }

        public void DeleteSong(int id)
        {
            songService.DeleteSong(id);
        }

        /// <summary>
        /// Gets song according to ID
        /// </summary>
        /// <param name="songId">song ID</param>
        /// <returns>The song</returns>
        public SongDTO GetSong(int songId)
        {
            return songService.GetSong(songId);
        }

        /// <summary>
        /// Gets the song id that belongs to the specified song name
        /// </summary>
        /// <param name="name">song name</param>
        /// <returns>id of song with the specified name</returns>
        public int GetSongIdByName(string name)
        {
            return songService.GetSongIdByName(name);
        }

        /// <summary>
        /// Gets all songs
        /// </summary>
        /// <returns>All available songs</returns>
        public IEnumerable<SongDTO> GetAllSongs()
        {
            return songService.ListAllSongs();
        }

        public void CreateReview(SongReviewDTO reviewDTO)
        {
            songReviewService.CreateSongReview(reviewDTO);
        }

        public void AddReview(SongReviewDTO reviewDTO)
        {
            songReviewService.AddReview(reviewDTO);
        }

        public void EditReview(SongReviewDTO reviewDTO)
        {
            songReviewService.EditSongReview(reviewDTO);
        }

        public void DeleteReview(int reviewId)
        {
            songReviewService.DeleteSongReview(reviewId);
        }

        /// <summary>
        /// Gets all reviews of the given song
        /// </summary>
        /// <param name="songId">song id</param>
        /// <returns>all reviews of the given song</returns>
        public IEnumerable<SongReviewDTO> GetAllReviews(int songId = 0)
        {
            return songService.GetSongReviews(songId);
        }

    }
}
