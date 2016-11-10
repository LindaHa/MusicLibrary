using AutoMapper;
using BL.DTOs.AlbumReviews;
using BL.DTOs.Albums;
using BL.DTOs.Artists;
using BL.DTOs.Clients;
using BL.DTOs.Genre_Albums;
using BL.DTOs.Genres;
using BL.DTOs.Song_Songlists;
using BL.DTOs.Songlists;
using BL.DTOs.SongReviews;
using BL.DTOs.Songs;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Bootstrap
{
    public class MappingInit
    {
        public static void ConfigureMapping()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<AlbumReview, AlbumReviewDTO>()
                            .ForMember(albumReviewDTO => albumReviewDTO.AlbumID, option => option.MapFrom(albumReview => albumReview.Album.ID))
                            .ReverseMap();

                config.CreateMap<Album, AlbumDTO>()
                            .ForMember(albumDTO => albumDTO.ArtistID, option => option.MapFrom(album => album.Artist.ID))
                            .ForMember(albumDTO => albumDTO.SongIDs, option => option.MapFrom(album => album.Songs.Select(song => song.ID)))
                            .ForMember(albumDTO => albumDTO.ReviewIDs, option => option.MapFrom(album => album.Reviews.Select(review => review.ID)))
                            .ReverseMap();

                config.CreateMap<Artist, ArtistDTO>()
                            .ForMember(artistDTO => artistDTO.AlbumIDs, option => option.MapFrom(artist => artist.Albums.Select(album => album.ID)))
                            .ReverseMap();

                config.CreateMap<Genre_Album, Genre_AlbumDTO>()
                            .ForMember(genre_albumDTO => genre_albumDTO.AlbumID, option => option.MapFrom(genre_album => genre_album.Album.ID))
                            .ForMember(genre_albumDTO => genre_albumDTO.GenreID, option => option.MapFrom(genre_album => genre_album.Genre.ID))
                            .ReverseMap();

                config.CreateMap<Genre, GenreDTO>().ReverseMap();

                config.CreateMap<Song_Songlist, Song_SonglistDTO>()
                            .ForMember(song_songlistDTO => song_songlistDTO.SongID, option => option.MapFrom(song_songlist => song_songlist.Song.ID))
                            .ForMember(song_songlistDTO => song_songlistDTO.SonglistID, option => option.MapFrom(song_songlist => song_songlist.Songlist.ID))
                            .ReverseMap();

                config.CreateMap<Songlist, SonglistDTO>()
                            .ForMember(songlistDTO => songlistDTO.SongIDs, option => option.MapFrom(songlist => songlist.Songs.Select(song => song.ID)))
                            .ReverseMap();

                config.CreateMap<SongReview, SongReviewDTO>()
                            .ForMember(songReviewDTO => songReviewDTO.SongID, option => option.MapFrom(songReview => songReview.Song.ID))
                            .ReverseMap();

                config.CreateMap<Song, SongDTO>()
                            .ForMember(songDTO => songDTO.AlbumID, option => option.MapFrom(song => song.Album.ID))
                            .ForMember(songDTO => songDTO.ReviewIDs, option => option.MapFrom(song => song.Reviews.Select(review => review.ID)))
                            .ReverseMap();

                config.CreateMap<Client, ClientDTO>()
                    .ForMember(clientDTO => clientDTO.UserAccountID, opts => opts.MapFrom(client => client.Account.Email))
                    //.ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Account.Email))
                    //.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.Account.FirstName))
                    //.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.Account.LastName))
                    .ForMember(clientDTO => clientDTO.SonglistIDs, opts => opts.MapFrom(client => client.Songlists.Select(songlist => songlist.ID)))
                    .ReverseMap();
            });
        }

    }
}
