using System;
using System.Linq;
using System.Data.Entity;
using Riganti.Utils.Infrastructure.EntityFramework;
using BrockAllen.MembershipReboot;
using BL.Repositories;
using BL.Services.AlbumReviews;
using BL.Services.Albums;
using BL.Services.Artists;
using BL.Services.Clients;
using BL.Services.Genre_Albums;
using BL.Services.Genres;
using BL.Services.Songlists;
using BL.Services.Song_Songlists;
using BL.Services.Songs;
using BL.Services.SongReviews;
using Castle.Windsor;
using BL.Bootstrap;
using BL.DTOs.Albums;
using BL.DTOs.Artists;
using BL.DTOs.Clients;
using System.Collections.Generic;
using Castle.MicroKernel.SubSystems.Configuration;
using BL.DTOs.Genres;
using BL.DTOs.Filters;
using BL.DTOs.AlbumReviews;
using BL.DTOs.Genre_Albums;
using BL.DTOs.Songs;
using BL.DTOs.Songlists;
using BL.DTOs.Song_Songlists;
using BL.DTOs.SongReviews;

namespace Program
{
    public class Program
    {
        private static UserAccountService<Riganti.Utils.Infrastructure.EntityFramework.UserAccount> userAccountService;
        private static IAlbumReviewService albumReviewService;
        private static IAlbumService albumService;
        private static IArtistService artistService;
        private static IClientService clientService;
        private static IGenre_AlbumService genre_albumService;
        private static IGenreService genreService;
        private static ISong_SonglistService song_songlistService;
        private static ISonglistService songlistService;
        private static ISongService songService;
        private static ISongReviewService songReviewService;

        private static readonly IWindsorContainer Container = new WindsorContainer();

        private static AlbumFacade albumFacade = new AlbumFacade(albumService, artistService, albumReviewService, songService);
        private static ArtistFacade artistFacade = new ArtistFacade(artistService, albumService, songService);
        private static Genre_AlbumFacade genre_albumFacade = new Genre_AlbumFacade(genre_albumService, albumService, genreService);
        private static GenreFacade genreFacade = new GenreFacade(genreService, genre_albumService, songService);
        private static Song_SonglistFacade song_songlistFacade = new Song_SonglistFacade(song_songlistService, songlistService, songService);
        private static SongFacade songFacade = new SongFacade(songService, albumService, songReviewService, genre_albumService);
        private static SonglistFacade songlistFacade = new SonglistFacade(songlistService, songService, song_songlistService);

        private static int clientID, clientID2;
        private static int songID, songID2, songlistID, songlistID2;
        private static int albumID, genreID, artistID;

        static void Main(string[] args)
        {   
            InitializeWindsorContainerAndMapper();
           
            TestClientService();
            TestArtistService();
            //TestGenreService();
            //TestAlbum_GenreService();
            //TestAlbumService();
            //TestSongService();
            //TestSonglistService();
            //TestSong_SonglistService();
            //TestClientServiceGetSonglists();


            Console.WriteLine("I'm done.");
            Console.ReadKey();
        }


        private static void InitializeWindsorContainerAndMapper()
        {
            Container.Install(new BussinessLayerInstaller());
            MappingInit.ConfigureMapping();
        }

        private static void TestClientService()
        {
            List<ClientDTO> list = new List<ClientDTO>();
            IUserAccountRepository<Riganti.Utils.Infrastructure.EntityFramework.UserAccount> uar = Container.Resolve<IUserAccountRepository<Riganti.Utils.Infrastructure.EntityFramework.UserAccount>>();
            uar.GetByUsername("me");
            userAccountService = new UserAccountService<Riganti.Utils.Infrastructure.EntityFramework.UserAccount>(uar);
            userAccountService.CreateAccount("Padge", "12345", "Padge@mail.com");
            userAccountService.CreateAccount("Matt", "12345", "Matt@mail.com");
            userAccountService.CreateAccount("Mike", "12345", "Mike@mail.com");
            var mattAccount = userAccountService.GetByUsername("Matt");
            var mikeAccount = userAccountService.GetByUsername("Mike");
            var padgeAccount = userAccountService.GetByUsername("Padge");

            clientService = Container.Resolve<IClientService>();

            //Create
            clientService.CreateClient(mattAccount.ID, new ClientDTO{ FirstName = "Mattthew", LastName = "Tuck" });
            clientService.CreateClient(padgeAccount.ID, new ClientDTO{ FirstName = "Padge", LastName = "Padgey" });
            clientService.CreateClient(mikeAccount.ID, new ClientDTO{ FirstName = "Mike", LastName = "Sufferer" });

            //GetClientByEmail
            ClientDTO matt = clientService.GetClientByEmail("Matt@mail.com");
            ClientDTO mike = clientService.GetClientByEmail("Mike@mail.com");
            ClientDTO padge = clientService.GetClientByEmail("Padge@mail.com");
            list.Add(matt);
            list.Add(mike);
            Console.WriteLine(list.Count() == 2 ? "ClientService - TestGetClientByEmail - OK" : "ClientService - TestGetClientByEmail - FAIL");

            clientID2 = padge.ID;
            clientID = matt.ID;


            //GetClientById
            ClientDTO matthew = clientService.GetClient(matt.ID);
            Console.WriteLine(matthew.FirstName == "Mattthew" && matthew.LastName == "Tuck" ?
                "ClientService - Test02 - OK" : "ClientService - Test02 - FAIL");

            //ListAllClients
            //var clients = clientService.ListAllClients(1);
            //Console.WriteLine(clients.TotalResultCount == 2 ? "ClientService - TestListAllClients - OK" : "ClientService - TestListAllClients - FAIL");

            //EditClient
            matthew.FirstName = "Matthew";
            matthew.LastName = "Lord";
            clientService.EditClient(matthew);
            ClientDTO mattFromDB = clientService.GetClient(matthew.ID);
            Console.WriteLine(mattFromDB.FirstName == "Matthew" && mattFromDB.LastName == "Lord" ?
                "ClientService - TestEditClient - OK" : "ClientService - TestEditClient - FAIL");

            //DeleteClient
            clientService.DeleteClient(mike.ID);
            ClientDTO mikeFromDB = clientService.GetClientByEmail("Mike@mail.com");
            Console.WriteLine(mikeFromDB == null ? "ClientService - TestDeleteClient - OK" : "ClientService - TestDeleteClient - FAIL");
        }

        private static void TestClientServiceGetSonglists()
        {
            clientService = Container.Resolve<IClientService>();
            ClientDTO client = clientService.GetClient(clientID);
            //client.SonglistIDs = songlistIDs;
            ////clientService.EditClient(client);
            //client = clientService.GetClient(client.ID);
            Console.WriteLine(client.SonglistIDs.Count() == 1 ? "ClientService - TestClientServiceGetSonglists - OK" : "ClientService - TestClientServiceGetSonglists - FAIL");
        }

        private static void TestArtistService()
        {
            List<int> list = new List<int>();
            artistService = Container.Resolve<IArtistService>();
            clientService = Container.Resolve<IClientService>();

            //Create
            artistService.CreateArtist(new ArtistDTO
            {
                Name = "Bullet For My Valentine",
                Info = "Bullet for My Valentine are a Welsh heavy metal band from Bridgend, formed in 1998.The band is composed of Matthew Tuck(lead vocals, rhythm guitar), Michael Paget(lead guitar, backing vocals), Michael Thomas(drums) and Jamie Mathias(bass guitar).",
                IsOfficial = true,
                CreatorID = clientID
            });
            artistService.CreateArtist(new ArtistDTO
            {
                Name = "Viola Martinsson",
                Info = "Viola Martinsson is a Swedish singer and musician born in 1991 in Söråker, Sweden. Her version of “Made Of” was recorded specifically to become the music theme for the Volvo campaign “Made of Sweden”, and is a cover of the 2012 track of the same name from Nause.",
                IsOfficial = true,
                CreatorID = clientID
            });

            //GetArtistIdByName
            artistID = artistService.GetArtistIdByName("Bullet For My Valentine");
            int violaID = artistService.GetArtistIdByName("Viola Martinsson");
            list.Add(artistID);
            list.Add(violaID);
            Console.WriteLine(list.Count() == 2 ? "ClientService - GetArtistIdByName - OK" : "ClientService - GetArtistIdByName - FAIL");
                       


            //GetArtisById
            ArtistDTO bfmv = artistService.GetArtist(artistID);
            Console.WriteLine(bfmv.Name == "Bullet For My Valentine" ? "ArtistService - TestGetArtisById - OK" : "ArtistService - TestGetArtisById - FAIL");

            //ListAllArtists01
           // var artists = artistService.ListAllArtists(new ArtistFilter { Name = "Viola Martinsson" }, 1);
           // Console.WriteLine(artists.TotalResultCount == 1 ? "ArtistService - TestListAllArtists01 - OK" : "ArtistService - TestListAllArtists01 - FAIL");

            //ListAllArtists02 
            var artists2 = artistService.ListAllArtists();
            Console.WriteLine(list.Count() == 2 ? "ClientService - ListAllArtists02 - OK" : "ClientService - ListAllArtists02 - FAIL");


            //EditArtist
            bfmv.Name = "BFMV";
            artistService.EditArtist(bfmv);
            ArtistDTO bfmvFromDB = artistService.GetArtist(bfmv.ID);
            Console.WriteLine(bfmvFromDB.Name == "BFMV" ? "ArtistService - TestEditArtist - OK" : "ArtistService - TestEditArtist - FAIL");

            //DeleteArtist
            artistService.DeleteArtist(violaID);
            int violaIDFromDB = artistService.GetArtistIdByName("Viola Martinsson");
            Console.WriteLine(violaIDFromDB < 1 ? "ArtistService - TestDeleteArtist - OK" : "ArtistService - TestDeleteArtist - FAIL");

            //GetCreator
            ClientDTO creator = artistService.GetCreator(bfmv.ID);
            Console.WriteLine(creator.ID == clientID ? "ArtistService - GetCreator - OK" : "ArtistService - GetCreator - FAIL");
        }       

        private static void TestGenreServis()
        {
            List<int> list = new List<int>();
            genreService = Container.Resolve<IGenreService>();
            clientService = Container.Resolve<IClientService>();

            //Create
            genreService.CreateGenre(new GenreDTO
            {
                Name = "Metalcore",
                Info = "Metalcore is a broad fusion genre of extreme metal and hardcore punk. The word is a blend of the names of the two genres. Metalcore is noted for its use of breakdowns, which are slow, intense passages that are conducive to moshing.",
                IsOfficial = true,
                CreatorID = clientID,          
            });
            genreService.CreateGenre(new GenreDTO
            {
                Name = "Indie pop",
                Info = "Indie pop is a subgenre of alternative rock or indie rock[1] and a subculture[2] that originated in the United Kingdom in the late 1970s. The style is inspired by punk's DIY ethic and related ideologies, and it generated a thriving fanzine, label, and club and gig circuit.",
                IsOfficial = true,
                CreatorID = clientID
            });

            //GetGenreIdByName
            genreID = genreService.GetGenreIdByName("Metalcore");
            int indieID = genreService.GetGenreIdByName("Indie pop");
            list.Add(genreID);
            list.Add(indieID);
            Console.WriteLine(list.Count() == 2 ? "ClientService - GetGenreIdByName - OK" : "ClientService - GetGenreIdByName - FAIL");



            //GetGenreById
            GenreDTO metalcore = genreService.GetGenre(genreID);
            Console.WriteLine(metalcore.Name == "Metalcore" ? "GenreService - GetGenreById - OK" : "GenreService - GetGenreById - FAIL");

            //ListAllGenres01
            //var genres = genreService.ListAllGenres(new GenreFilter { ArtistID = artistID }, 1);
            //Console.WriteLine(genres.TotalResultCount == 1 ? "GenreService - TestListAllGenres01 - OK" : "GenreService - TestListAllGenres01 - FAIL");

            //ListAllGenres02
            var genres2 = genreService.ListAllGenres();
            Console.WriteLine(genres2.Count() == 2 ? "GenreService - ListAllGenres02 - OK" : "GenreService - ListAllGenres02 - FAIL");

            //EditGenre
            metalcore.Name = "Metal Core";
            genreService.EditGenre(metalcore);
            GenreDTO bfmvFromDB = genreService.GetGenre(metalcore.ID);
            Console.WriteLine(bfmvFromDB.Name == "Metal Core" ? "GenreService - TestEditGenre - OK" : "GenreService - TestEditGenre - FAIL");

            //DeleteGenre
            genreService.DeleteGenre(indieID);
            int violaIDFromDB = genreService.GetGenreIdByName("Viola Martinsson");
            Console.WriteLine(violaIDFromDB < 1 ? "GenreService - TestDeleteGenre - OK" : "GenreService - TestDeleteGenre - FAIL");

            //GetCreator
            ClientDTO creator = genreService.GetCreator(metalcore.ID);
            Console.WriteLine(creator.ID == clientID ? "GenreService - GetCreator - OK" : "GenreService - GetCreator - FAIL");
        }

        private static void TestAlbumService()
        {
            List<int> list = new List<int>();
            albumService = Container.Resolve<IAlbumService>();
            clientService = Container.Resolve<IClientService>();

            //Create
            albumService.CreateAlbum(new AlbumDTO
            {
                Name = "The Poison",
                ArtistID = artistID,
                IsOfficial = true,
                CreatorID = clientID,
            });
            albumService.CreateAlbum(new AlbumDTO
            {
                Name = "Venom",
                ArtistID = artistID,
                IsOfficial = true,
                CreatorID = clientID
            });

            
            //GetAlbumIdByName
            albumID = albumService.GetAlbumIdByName("The Poison");
            int venomID = albumService.GetAlbumIdByName("Indie pop");
            list.Add(albumID);
            list.Add(venomID);
            Console.WriteLine(list.Count() == 2 ? "ClientService - GetAlbumIdByName - OK" : "ClientService - GetAlbumIdByName - FAIL");
            
            //GetAlbumById
            AlbumDTO poison = albumService.GetAlbum(albumID);
            AlbumDTO venom = albumService.GetAlbum(venomID);
            Console.WriteLine(poison.Name == "The Poison" ? "AlbumService - GetAlbumById - OK" : "AlbumService - GetAlbumById - FAIL");

            artistService = Container.Resolve<IArtistService>();

            //AddAlbum
            albumService.AddAlbum(poison);
            albumService.AddAlbum(venom);
            ArtistDTO artist = artistService.GetArtist(artistID);
            Console.WriteLine(artist.AlbumIDs.Contains(albumID) ?
                "AlbumService - AddAlbum - OK" : "AlbumService - AddAlbum - FAIL");

            //GetArtistOfAlbum
            ArtistDTO artist2 = albumService.GetArtistOfAlbum(albumID);
            Console.WriteLine(artist2.ID == artistID ?
                "AlbumService - GetArtistOfAlbum - OK" : "AlbumService - GetArtistOfAlbum - FAIL");

            //TestArtistServisGetAllAlbums            
            Console.WriteLine(artist.AlbumIDs.Count() == 2 ?
            "ArtistService - TestArtistServisGetAllAlbums - OK" : "ArtistService - TestArtistServisGetAllAlbums - FAIL");




            //ListAllAlbums
            //var albums = albumService.ListAllAlbums(new AlbumFilter { Name = "Venom" }, 1);
            //Console.WriteLine(albums.TotalResultCount == 1 ? "AlbumService - TestListAllAlbums - OK" : "AlbumService - TestListAllAlbums - FAIL");

            //ListAllAlbumss02
            var albums2 = albumService.ListAllAlbums();
            Console.WriteLine(albums2.Count() == 2 ? "AlbumService - ListAllAlbumss02 - OK" : "AlbumService - ListAllAlbumss02 - FAIL");

            //EditAlbum
            poison.Name = "The Poisonous Poison";
            albumService.EditAlbum(poison,artistID,null,null);
            AlbumDTO poisonFromDB = albumService.GetAlbum(poison.ID);
            Console.WriteLine(poisonFromDB.Name == "The Poisonous Poison" ? "AlbumService - TestEditAlbum - OK" : "AlbumService - TestEditAlbum - FAIL");

            //DeleteAlbum
            albumService.DeleteAlbum(venomID);
            AlbumDTO venomFromDB = albumService.GetAlbum(venomID);
            Console.WriteLine(venomFromDB == null ? "AlbumService - TestDeleteAlbum - OK" : "AlbumService - TestDeleteAlbum - FAIL");

            //GetCreator
            ClientDTO creator = genreService.GetCreator(poison.ID);
            Console.WriteLine(creator.ID == clientID ? "AlbumService - GetCreator - OK" : "AlbumService - GetCreator - FAIL");            
        }

        private static void TestAlbumReview()
        {
            List<int> list = new List<int>();
            albumReviewService = Container.Resolve<IAlbumReviewService>();
            clientService = Container.Resolve<IClientService>();

            //Create
            albumReviewService.CreateAlbumReview(new AlbumReviewDTO
            {
                Text = "This disc is a great improvement on an already brilliant EP, keeping the same style with great screams and guitars and improving melodically, lyrically and each song delivering a catchy chorus.",
                AlbumID = albumID,
                CreatorID = clientID,
            });
            albumReviewService.CreateAlbumReview(new AlbumReviewDTO
            {
                Text = "I remember listening to The Poison, 10 years ago when this style of metalcore was at an incredibly high popularity. I definitely didn’t hate it but still preferred to listen to other bands of even similar genres.",
                AlbumID = albumID,
                CreatorID = clientID2,
            });

            //ListAllAlbumReviews
            var albumReviews = albumReviewService.ListAllAlbumReviews(new AlbumReviewFilter { AlbumID = albumID }, 1);
            Console.WriteLine(albumReviews.TotalResultCount == 2 ? "AlbumReviewService - TestListAllAlbumReviews - OK" : "AlbumReviewService - TestListAllAlbumReviews - FAIL");
            
            //GetAlbumReviewById
            var reviews = albumReviewService.ListAllAlbumReviews(new AlbumReviewFilter { CreatorIDs = new int[] { clientID } }, 1);
            AlbumReviewDTO albumReview = reviews.ResultsPage.FirstOrDefault();
            var testedReview = albumReviewService.GetAlbumReview(albumReview.ID);
            Console.WriteLine(testedReview.ID == albumReview.ID ? "AlbumReviewService - TestGetAlbumReviewById - OK" : "AlbumReviewService - TestGetAlbumReviewById - FAIL");
        
            //AddAlbumReview
            reviews = albumReviewService.ListAllAlbumReviews(new AlbumReviewFilter { CreatorIDs = new int[] { clientID2 } }, 1);
            AlbumReviewDTO review2 = reviews.ResultsPage.FirstOrDefault();
            albumReviewService.AddReview(albumReview);
            albumReviewService.AddReview(review2);
            AlbumDTO album = albumService.GetAlbum(albumID);
            Console.WriteLine(album.ReviewIDs.Contains(review2.ID) ?
                "AlbumReviewService - TestAddAlbumReview - OK" : "AlbumReviewService - TestAddAlbumReview - FAIL");

            //TestAlbumServisGetAllAlbumReviews            
            Console.WriteLine(album.ReviewIDs.Count() == 2 ?
            "AlbumService - TestAlbumServisGetAllAlbumReviews - OK" : "AlbumService - TestAlbumServisGetAllAlbumReviews - FAIL");
                        
            //EditAlbumReview
            review2.Text = "1010";
            albumReviewService.EditAlbumReview(review2);
            AlbumReviewDTO review2FromDB = albumReviewService.GetAlbumReview(review2.ID);
            Console.WriteLine(review2FromDB.Text == "1010" ? "AlbumReviewService - TestEditAlbumReview - OK" : "AlbumReviewService - TestEditAlbumReview - FAIL");

            //DeleteAlbumReview
            albumReviewService.DeleteAlbumReview(review2.ID);
            AlbumReviewDTO venomFromDB = albumReviewService.GetAlbumReview(review2.ID);
            Console.WriteLine(venomFromDB == null ? "AlbumReviewService - TestDeleteAlbumReview - OK" : "AlbumReviewService - TestDeleteAlbumReview - FAIL");

            //GetCreator
            ClientDTO creator = albumReviewService.GetCreator(albumReview.ID);
            Console.WriteLine(creator.ID == clientID ? "AlbumReviewService - TestGetCreator - OK" : "AlbumReviewService - TestGetCreator - FAIL");

        }

        private static void TestGenre_AlbumService()
        {
            List<int> list = new List<int>();
            genre_albumService = Container.Resolve<IGenre_AlbumService>();
            clientService = Container.Resolve<IClientService>();

            //Create
            genre_albumService.CreateGenre_Album(new Genre_AlbumDTO
            {
                GenreID = genreID,
                AlbumID = albumID,
                IsOfficial = true,
                CreatorID = clientID,
            });
            genre_albumService.CreateGenre_Album(new Genre_AlbumDTO
            {
                GenreID = genreID,
                AlbumID = albumID,
                IsOfficial = true,
                CreatorID = clientID2,
            });            
            
            //ListAllGenre_Albums
            var genre_albums = genre_albumService.ListAllGenre_Albums();
            Console.WriteLine(genre_albums.Count() == 2 ? "Genre_AlbumService - ListAllGenre_Albums - OK" : "Genre_AlbumService - ListAllGenre_Albums - FAIL");

            //GetGenre_AlbumById
            Genre_AlbumDTO song_songlist = genre_albums.FirstOrDefault();
            Genre_AlbumDTO song_songlist2 = genre_albums.LastOrDefault();
            Genre_AlbumDTO testedGenre = genre_albumService.GetGenre_Album(song_songlist.ID);
            Console.WriteLine(testedGenre.ID == song_songlist.ID ? "Genre_AlbumService - GetGenre_AlbumById - OK" : "Genre_AlbumService - GetGenre_AlbumById - FAIL");

            //EditGenre_Album
            song_songlist2.CreatorID = clientID;
            genre_albumService.EditGenre_Album(song_songlist2, albumID, genreID);
            Genre_AlbumDTO bfmvFromDB = genre_albumService.GetGenre_Album(song_songlist2.ID);
            Console.WriteLine(bfmvFromDB.CreatorID == clientID ? "Genre_AlbumService - TestEditGenre_Album - OK" : "Genre_AlbumService - TestEditGenre_Album - FAIL");
                        
            //DeleteGenre_Album
            int g_aID = song_songlist2.ID;
            genre_albumService.DeleteGenre_Album(song_songlist2.ID);
            song_songlist2 = genre_albumService.GetGenre_Album(g_aID);
            Console.WriteLine(song_songlist2 == null ? "Genre_AlbumService - TestDeleteGenre_Album - OK" : "Genre_AlbumService - TestDeleteGenre_Album - FAIL");

            //GetCreator
            ClientDTO creator = genre_albumService.GetCreator(song_songlist.ID);
            Console.WriteLine(creator.ID == clientID ? "Genre_AlbumService - GetCreator - OK" : "Genre_AlbumService - GetCreator - FAIL");

            //GetAllGenresForAlbum

            var genres = genre_albumService.GetAllGenresForAlbum(albumID);
            Console.WriteLine(genres.Count() == 1 ? "Genre_AlbumService - GetAllGenresForAlbum - OK" : "Genre_AlbumService - GetAllGenresForAlbum - FAIL");

            //GetAllAlbumsForGenre

            var albums = genre_albumService.GetAllAlbumsForGenre(genreID);
            Console.WriteLine(albums.Count() == 1 ? "Genre_AlbumService - GetAllAlbumsForGenre - OK" : "Genre_AlbumService - GetAllAlbumsForGenre - FAIL");

        }

        private static void TestSongService()
        {
            List<int> list = new List<int>();
            songService = Container.Resolve<ISongService>();
            clientService = Container.Resolve<IClientService>();

            //Create
            songService.CreateSong(new SongDTO
            {
                Name = "Hit The Floor",
                AlbumID = albumID,
                CreatorID = clientID,
                Duration = new TimeSpan(0, 0, 3, 30),
                IsOfficial = true,
            });
            songService.CreateSong(new SongDTO
            {
                Name = "4 Words",
                AlbumID = albumID,
                CreatorID = clientID,
                Duration = new TimeSpan(0, 0, 3, 43),
                IsOfficial = true,
            });
            songService.CreateSong(new SongDTO
            {
                Name = "All These Things I Hate",
                AlbumID = albumID,
                CreatorID = clientID2,
                Duration = new TimeSpan(0, 0, 3, 45),
                IsOfficial = true,
            });


            //GetSongIdByName
            songID = songService.GetSongIdByName("Hit The Floor");
            int wordsID = songService.GetSongIdByName("4 Words");
            songID2 = songService.GetSongIdByName("All These Things I Hate");
            list.Add(songID);
            list.Add(wordsID);
            list.Add(songID2);
            Console.WriteLine(list.Count() == 3 ? "ClientService - GetSongIdByName - OK" : "ClientService - GetSongIdByName - FAIL");

            //GetSongById
            SongDTO floor = songService.GetSong(songID);
            SongDTO words4 = songService.GetSong(wordsID);
            SongDTO hate = songService.GetSong(songID2);
            Console.WriteLine(floor.Name == "Hit The Floor" ? "SongService - GetSongById - OK" : "SongService - GetSongById - FAIL");

            albumService = Container.Resolve<IAlbumService>();

            //AddSong
            songService.AddSong(floor);
            songService.AddSong(words4);
            songService.AddSong(hate);
            AlbumDTO album = albumService.GetAlbum(albumID);
            Console.WriteLine(album.SongIDs.Contains(songID) ?
                "SongService - AddSong - OK" : "SongService - AddSong - FAIL");

            //GetAlbumOfSong
            AlbumDTO album2 = songService.GetAlbumOfSong(songID);
            Console.WriteLine(album2.ID == albumID ?
                "SongService - GetAlbumOfSong - OK" : "SongService - GetAlbumOfSong - FAIL");

            //TestAlbumServisGetAllSongs            
            Console.WriteLine(album.SongIDs.Count() == 3 ?
            "AlbumService - TestAlbumServisGetAllSongs - OK" : "AlbumService - TestAlbumServisGetAllSongs - FAIL");

            ////ListAllSongs
            //var songs = songService.ListAllSongs(new SongFilter { AlbumID = albumID }, 1);
            //Console.WriteLine(songs.TotalResultCount == 3 ? "SongService - TestListAllSongs - OK" : "SongService - TestListAllSongs - FAIL");

            //ListAllSongss02
            var songs2 = songService.ListAllSongs();
            Console.WriteLine(songs2.Count() == 3 ? "SongService - ListAllSongss02 - OK" : "SongService - ListAllSongss02 - FAIL");

            //EditSong
            words4.Name = "Four Words";
            songService.EditSong(words4, albumID);
            SongDTO words4FromDB = songService.GetSong(words4.ID);
            Console.WriteLine(words4FromDB.Name == "Four Words" ? "SongService - TestEditSong - OK" : "SongService - TestEditSong - FAIL");

            //DeleteSong
            songService.DeleteSong(wordsID);
            SongDTO wordsFromDB = songService.GetSong(wordsID);
            Console.WriteLine(wordsFromDB == null ? "SongService - TestDeleteSong - OK" : "SongService - TestDeleteSong - FAIL");

            //GetCreator
            ClientDTO creator = genreService.GetCreator(floor.ID);
            Console.WriteLine(creator.ID == clientID ? "SongService - GetCreator - OK" : "SongService - GetCreator - FAIL");
        }

        private static void TestSonglistService()
        {
            List<int> list = new List<int>();
            songlistService = Container.Resolve<ISonglistService>();
            clientService = Container.Resolve<IClientService>();

            //Create
            songlistService.CreateSonglist(new SonglistDTO
            {
                Name = "Matthew:LongList",
                OwnerID = clientID,
                SongIDs = new int[] { songID, songID2 }
            });
            songlistService.CreateSonglist(new SonglistDTO
            {
                Name = "Matthew:ShortList",
                OwnerID = clientID,
                SongIDs = new int[] { songID}
            });
            songlistService.CreateSonglist(new SonglistDTO
            {
                Name = "Padge:ShortList",
                OwnerID = clientID2,
                SongIDs = new int[] { songID}
            });

            //GetSonglistIdByName
            songlistID = songlistService.GetSonglistIdByName("Matthew:LongList");
            int mattShortListID = songlistService.GetSonglistIdByName("Matthew:ShortList");
            songlistID2 = songlistService.GetSonglistIdByName("Padge:ShortList");
            list.Add(songlistID);
            list.Add(mattShortListID);
            list.Add(songlistID2);
            Console.WriteLine(list.Count() == 3 ? "ClientService - TestGetSonglistIdByName - OK" : "ClientService - TestGetSonglistIdByName - FAIL");



            //GetSonglistById
            SonglistDTO mattLongList = songlistService.GetSonglist(songlistID);
            Console.WriteLine(mattLongList.Name == "Matthew:LongList" ? "SonglistService - TestGetSonglistById - OK" : "SonglistService - TestGetSonglistById - FAIL");

            ////ListAllSonglists01
            // var songlists = songlistService.ListAllSonglists(new SonglistFilter { OwnerID = clientID }, 1);
            //Console.WriteLine(songlists.TotalResultCount == 2 ? "SonglistService - TestListAllSonglists01 - OK" : "SonglistService - TestListAllSonglists01 - FAIL");

            //ListAllSonglists02 
            var songlists2 = songlistService.ListAllSonglists();
            Console.WriteLine(list.Count() == 3 ? "ClientService - TestListAllSonglists02 - OK" : "ClientService - TestListAllSonglists02 - FAIL");


            //EditSonglist
            mattLongList.Name = "longLongList";
            songlistService.EditSonglist(mattLongList);
            SonglistDTO listFromDB = songlistService.GetSonglist(mattLongList.ID);
            Console.WriteLine(listFromDB.Name == "longLongList" ? "SonglistService - TestEditSonglist - OK" : "SonglistService - TestEditSonglist - FAIL");

            //DeleteSonglist
            songlistService.DeleteSonglist(mattShortListID);
            SonglistDTO listIDFromDB = songlistService.GetSonglist(mattShortListID);
            Console.WriteLine(listIDFromDB == null ? "SonglistService - TestDeleteSonglist - OK" : "SonglistService - TestDeleteSonglist - FAIL");

            //GetCreator
            ClientDTO creator = songlistService.GetCreator(mattLongList.ID);
            Console.WriteLine(creator.ID == clientID ? "SonglistService - TestGetCreator - OK" : "SonglistService - TestGetCreator - FAIL");

            //GetSonglistSongs
            var songs = songlistService.GetSonglistSongs(songlistID);
            Console.WriteLine(songs.Count() == 2 ? "SonglistService - TestGetSonglistSongs - OK" : "SonglistService -Test GetSonglistSongs - FAIL");
        }

        private static void TestSong_Songlist()
        {
            List<int> list = new List<int>();
            song_songlistService = Container.Resolve<ISong_SonglistService>();
            clientService = Container.Resolve<IClientService>();

            //Create
            song_songlistService.CreateSong_Songlist(new Song_SonglistDTO
            {
                SongID = songID,
                SonglistID = songlistID,
                CreatorID = clientID
            });
            song_songlistService.CreateSong_Songlist(new Song_SonglistDTO
            {
                SongID = songID2,
                SonglistID = songlistID,
                CreatorID = clientID                
            });
            song_songlistService.CreateSong_Songlist(new Song_SonglistDTO
            {
                SongID = songID,
                SonglistID = songlistID2,
                CreatorID = clientID2
            });

            //ListAllSong_Songlists
            var song_songlists = song_songlistService.ListAllSong_Songlists();
            Console.WriteLine(song_songlists.Count() == 3 ? "Song_SonglistService - TestListAllSong_Songlists - OK" : "Song_SonglistService - TestListAllSong_Songlists - FAIL");

            //GetSong_SonglistById
            Song_SonglistDTO song_songlist = song_songlists.FirstOrDefault();
            Song_SonglistDTO song_songlist2 = song_songlists.ElementAt(1);
            Song_SonglistDTO testedSong = song_songlistService.GetSong_Songlist(song_songlist.ID);
            Console.WriteLine(testedSong.ID == song_songlist.ID ? "Song_SonglistService - TestGetSong_SonglistById - OK" : "Song_SonglistService - TestGetSong_SonglistById - FAIL");

            //EditSong_Songlist
            song_songlist2.CreatorID = clientID;
            song_songlistService.EditSong_Songlist(song_songlist2, song_songlist2.SonglistID, song_songlist2.SongID);
            Song_SonglistDTO s_sFromDB = song_songlistService.GetSong_Songlist(song_songlist2.ID);
            Console.WriteLine(s_sFromDB.CreatorID == clientID ? "Song_SonglistService - TestEditSong_Songlist - OK" : "Song_SonglistService - TestEditSong_Songlist - FAIL");

            //DeleteSong_Songlist
            int s_sID = song_songlist2.ID;
            song_songlistService.DeleteSong_Songlist(song_songlist2.ID);
            song_songlist2 = song_songlistService.GetSong_Songlist(s_sID);
            Console.WriteLine(song_songlist2 == null ? "Song_SonglistService - TestDeleteSong_Songlist - OK" : "Song_SonglistService - TestDeleteSong_Songlist - FAIL");

            //GetCreator
            ClientDTO creator = song_songlistService.GetCreator(song_songlist.ID);
            Console.WriteLine(creator.ID == clientID ? "Song_SonglistService - TestGetCreator - OK" : "Song_SonglistService - TestGetCreator - FAIL");

            //GetAllSongsForSonglist
            var songs = song_songlistService.GetAllSongsForSonglist(songlistID);
            Console.WriteLine(songs.Count() == 2 ? "Song_SonglistService - TestGetAllSongsForSonglist - OK" : "Song_SonglistService - TestGetAllSongsForSonglist - FAIL");

            //GetAllSonglistsForSong
            var songlists = song_songlistService.GetAllSonglistsForSong(songID);
            Console.WriteLine(songlists.Count() == 2 ? "Song_SonglistService - TestGetAllSonglistsForSong - OK" : "Song_SonglistService - TestGetAllSonglistsForSong - FAIL");

        }

        private static void TestSongReview()
        {
            List<int> list = new List<int>();
            songReviewService = Container.Resolve<ISongReviewService>();
            clientService = Container.Resolve<IClientService>();

            //Create
            songReviewService.CreateSongReview(new SongReviewDTO
            {
                Text = "Reminds me of my dreaded youth.",
                SongID = songID,
                CreatorID = clientID,
                
            });
            songReviewService.CreateSongReview(new SongReviewDTO
            {
                Text = "One of the best songs, on their best album!",
                SongID = songID,
                CreatorID = clientID2,
            });
            songReviewService.CreateSongReview(new SongReviewDTO
            {
                Text = "This video actually makes sense unlike other songs, and the video matches the lyrics 2 boot! I LOVE IT!!! sry, sugar rush (still worst joke ever) sry, but anyway, this video is very good, SO GOOD!!! ",
                SongID = songID2,
                CreatorID = clientID,
            });

            //ListAllSongReviews
            var songReviews = songReviewService.ListAllSongReviews(new SongReviewFilter { SongID = songID }, 1);
            Console.WriteLine(songReviews.TotalResultCount == 2 ? "SongReviewService - TestListAllSongReviews - OK" : "SongReviewService - TestListAllSongReviews - FAIL");

            //GetSongReviewById
            var reviews = songReviewService.ListAllSongReviews(null, 1);
            SongReviewDTO songReview = reviews.ResultsPage.FirstOrDefault();
            var testedReview = songReviewService.GetSongReview(songReview.ID);
            Console.WriteLine(testedReview.ID == songReview.ID ? "SongReviewService - TestGetSongReviewById - OK" : "SongReviewService - TestGetSongReviewById - FAIL");

            //AddSongReview
            SongReviewDTO review2 = reviews.ResultsPage.ElementAt(1);
            SongReviewDTO review3 = reviews.ResultsPage.LastOrDefault();
            reviews = songReviewService.ListAllSongReviews(null, 1);
            songReview = reviews.ResultsPage.ElementAt(1);
            songReviewService.AddReview(songReview);
            songReviewService.AddReview(review2);
            songReviewService.AddReview(review3);
            SongDTO song = songService.GetSong(songID);
            Console.WriteLine(song.ReviewIDs.Contains(songReview.ID) ?
                "SongReviewService - TestAddSongReview - OK" : "SongReviewService - TestAddSongReview - FAIL");

            //TestSongServisGetAllSongReviews            
            Console.WriteLine(song.ReviewIDs.Count() == 2 ?
            "SongService - TestSongServisGetAllSongReviews - OK" : "SongService - TestSongServisGetAllSongReviews - FAIL");

            //EditSongReview
            review2.Text = "1010";
            songReviewService.EditSongReview(review2);
            SongReviewDTO review2FromDB = songReviewService.GetSongReview(review2.ID);
            Console.WriteLine(review2FromDB.Text == "1010" ? "SongReviewService - TestEditSongReview - OK" : "SongReviewService - TestEditSongReview - FAIL");

            //DeleteSongReview
            songReviewService.DeleteSongReview(review2.ID);
            SongReviewDTO reviewFromDB = songReviewService.GetSongReview(review2.ID);
            Console.WriteLine(reviewFromDB == null ? "SongReviewService - TestDeleteSongReview - OK" : "SongReviewService - TestDeleteSongReview - FAIL");

            //GetCreator
            ClientDTO creator = songReviewService.GetCreator(songReview.ID);
            Console.WriteLine(creator.ID == clientID ? "SongReviewService - TestGetCreator - OK" : "SongReviewService - TestGetCreator - FAIL");
        }
    }
}
