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
            //TestSongService();
            //TestArtistService();
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
            userAccountService.CreateAccount("Matt", "12345", "Matt@mail.com");
            userAccountService.CreateAccount("Mike", "12345", "Mike@mail.com");
            var mattAccount = userAccountService.GetByUsername("Matt");
            var mikeAccount = userAccountService.GetByUsername("Mike");

            clientService = Container.Resolve<IClientService>();

            //Create
            clientService.CreateClient(mattAccount.ID, new ClientDTO{ FirstName = "Mattthew", LastName = "Payne" });
            clientService.CreateClient(mikeAccount.ID, new ClientDTO{ FirstName = "Mike", LastName = "Sufferer" });

            //GetClientByEmail
            ClientDTO matt = clientService.GetClientByEmail("Matt@mail.com");
            ClientDTO mike = clientService.GetClientByEmail("Mike@mail.com");
            list.Add(matt);
            list.Add(mike);
            Console.WriteLine(list.Count() == 2 ? "ClientService - Test01 - OK" : "ClientService - Test01 - FAIL");

            //EditClient
            matt.FirstName = "Matthew";
            matt.LastName = "Lord";
            clientService.EditClient(matt);

            Console.WriteLine(matt.FirstName == "Matthew" && matt.LastName == "Lord" ? 
                "ClientService - Test02 - OK" : "ClientService - Test02 - FAIL");

            //DeleteClient
            clientService.DeleteClient(mike.ID);

            //GetClient
            ClientDTO matthew = clientService.GetClient(matt.ID);
            Console.WriteLine(matt.FirstName == "Matthew" && matt.LastName == "Lead@mail.com" ?
                "ClientService - Test02 - OK" : "ClientService - Test02 - FAIL");



        }

        private static void TestArtistService()
        {
            int client1 = 2;

            artistService = Container.Resolve<IArtistService>();

            //Create
            artistService.CreateArtist(new ArtistDTO
            {
                Name = "Bullet For My Valentine",
                Info = "Bullet for My Valentine are a Welsh heavy metal band from Bridgend, formed in 1998.The band is composed of Matthew Tuck(lead vocals, rhythm guitar), Michael Paget(lead guitar, backing vocals), Michael Thomas(drums) and Jamie Mathias(bass guitar).",
                IsOfficial = true,
                CreatorID = client1
            });

        }

        private static void TestSongService()
        {
            int client1 = 2;

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
