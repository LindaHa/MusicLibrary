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
        private static int songID;
        private static int albumID;
        private static int genreID;
        private static int artistID;
        private static int[] songlistIDs;

        static void Main(string[] args)
        {   
            InitializeWindsorContainerAndMapper();



            //var userAccount = userAccountService.GetByUsername("Armin");

            //// do the testing
            //using (var context = new DAL.MusicLibraryDbContext())
            //{
            //    context.Users.Add(new UserAccount
            //    {
            //        UserName = "Guest"
            //    });

            //    context.Songs.Add(new Song
            //    {
            //        Name = "Hit the Floor",
            //        IsOfficial = true,
            //        Duration = new TimeSpan(0, 0, 3, 30),
            //        Album = context.Albums
            //        .Where(a => a.Name == "The Poison")
            //        .First(),
            //        Creator = context.Users
            //        .Where(a => a.UserName == "Armin Admin")
            //        .First(),
            //    });


            //    context.SaveChanges();
            //}   

            //using(var context = new MusicLibraryDbContext())
            //{
            //    DbSet<Song> songs = context.Songs;
            //    foreach(Song song in songs)
            //    {
            //        string text = (song.IsOfficial ? "Can " : "Can't ") + "be found in our official Database.";
            //        Console.WriteLine("the song " + song.Name + " from " + song.Album.Name + " by " + song.Album.Artist.Name + text);
            //    }
            //}
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
            var clients = clientService.ListAllClients(1);
            Console.WriteLine(clients.TotalResultCount == 2 ? "ClientService - TestListAllClients - OK" : "ClientService - TestListAllClients - FAIL");

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

            //GetArtistIdByEmail
            artistID = artistService.GetArtistIdByName("Bullet For My Valentine");
            int violaID = artistService.GetArtistIdByName("Viola Martinsson");
            list.Add(artistID);
            list.Add(violaID);
            Console.WriteLine(list.Count() == 2 ? "ClientService - GetArtistIdByEmail - OK" : "ClientService - GetArtistIdByEmail - FAIL");
                       


            //GetArtisById
            ArtistDTO bfmv = artistService.GetArtist(artistID);
            Console.WriteLine(bfmv.Name == "Bullet For My Valentine" ? "ArtistService - TestGetArtisById - OK" : "ArtistService - TestGetArtisById - FAIL");

            //ListAllArtists01
            var artists = artistService.ListAllArtists(new ArtistFilter { Name = "Viola Martinsson" }, 1);
            Console.WriteLine(artists.TotalResultCount == 1 ? "ArtistService - TestListAllArtists01 - OK" : "ArtistService - TestListAllArtists01 - FAIL");

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

            //GetGenreIdByEmail
            genreID = genreService.GetGenreIdByName("Metalcore");
            int indieID = genreService.GetGenreIdByName("Indie pop");
            list.Add(genreID);
            list.Add(indieID);
            Console.WriteLine(list.Count() == 2 ? "ClientService - GetGenreIdByEmail - OK" : "ClientService - GetGenreIdByEmail - FAIL");



            //GetGenreById
            GenreDTO metalcore = genreService.GetGenre(genreID);
            Console.WriteLine(metalcore.Name == "Metalcore" ? "GenreService - TestGetArtisById - OK" : "GenreService - TestGetArtisById - FAIL");

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

            
            //GetAlbumIdByEmail
            albumID = albumService.GetAlbumIdByName("The Poison");
            int venomID = albumService.GetAlbumIdByName("Indie pop");
            list.Add(albumID);
            list.Add(venomID);
            Console.WriteLine(list.Count() == 2 ? "ClientService - GetAlbumIdByEmail - OK" : "ClientService - GetAlbumIdByEmail - FAIL");
            
            //GetAlbumById
            AlbumDTO poison = albumService.GetAlbum(albumID);
            AlbumDTO venom = albumService.GetAlbum(venomID);
            Console.WriteLine(poison.Name == "The Poison" ? "AlbumService - TestGetArtisById - OK" : "AlbumService - TestGetArtisById - FAIL");

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
            var albums2 = genreService.ListAllGenres();
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
          }

        private static void TestSongService()
        {
            

            songService = Container.Resolve<ISongService>();

            songService.GetSong(1);

            songService.AddSong(new BL.DTOs.Songs.SongDTO
            {
                Name = "My sexy back",
                Duration = 1.2f,
                IsOfficial = false,
                AlbumID = 1
            });

        }

        //private static void TestAlbumService()
        //{
        //    albumService = Container.Resolve<IAlbumService>();

        //    //Create
        //    albumService.CreateAlbum(new AlbumDTO { Name = "The Poison",
        //                                                });
        //    album1Id = albumService.GetProductAlbumIdByName("Electronics");
        //    albumService.CreateAlbum(new AlbumDTO { Name = "Telephones", ParentID = null });
        //    album2Id = albumService.GetProductAlbumIdByName("Telephones");
        //    albumService.CreateAlbum(new AlbumDTO { Name = "Smart phones", ParentID = album2Id });
        //    album3Id = albumService.GetProductAlbumIdByName("Smart phones");

        //    //Rename and Change parent
        //    albumService.EditAlbum(new AlbumDTO { ID = album2Id, Name = "Phones", ParentID = album1Id });

        //    //GetAlbumPath
        //    var list = albumService.GetAlbumPath(album3Id);
        //    Console.WriteLine(list.Count() == 3 ? "AlbumService - Test01 - OK" : "AlbumService - Test01 - FAIL");

        //    var categories = albumService.ListAllCategories();

        //    //Delete
        //    albumService.DeleteAlbum(album3Id);
        //}
    }
}
