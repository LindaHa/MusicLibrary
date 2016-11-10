using BrockAllen.MembershipReboot;
using DAL.Entities;
using System;
using System.Data.Entity;
using Castle.Windsor;


namespace DAL
{
    public class MusicLibraryDbInitializer : DropCreateDatabaseAlways<MusicLibraryDbContext>
    {
        private static UserAccountService<Riganti.Utils.Infrastructure.EntityFramework.UserAccount> userAccountService;
        private static readonly IWindsorContainer Container = new WindsorContainer();

        public override void InitializeDatabase(MusicLibraryDbContext context)
        {
            base.InitializeDatabase(context);
            return;

            userAccountService = Container.Resolve<UserAccountService<Riganti.Utils.Infrastructure.EntityFramework.UserAccount>>();
            userAccountService.CreateAccount("Admin", "Admin", "123", "Admin@mail.com");

            var adminAccount = userAccountService.GetByUsername("Admin");
            Client admin = new Client { Account = adminAccount };
            

            Artist bfmv = new Artist
            {
                Name = "Bullet For My Valentine",
                Info = "Bullet for My Valentine are a Welsh heavy metal band from Bridgend, formed in 1998. The band is composed of Matthew Tuck (lead vocals, rhythm guitar), Michael Paget (lead guitar, backing vocals), Michael Thomas (drums) and Jamie Mathias (bass guitar). Former members include Nick Crandle and Jason James; both were on bass.",
                IsOfficial = true,
                Creator = admin
            };
            context.Artists.Add(bfmv);
            
            var thePoisonAlbum = new Album
            {
                Name = "The Poison",
                Artist = bfmv,
                IsOfficial = true,
                Creator = admin
            };
            var feverAlbum = new Album
            {
                Name = "Fever",
                Artist = bfmv,
                IsOfficial = true,
                Creator = admin
            };
            context.Albums.Add(thePoisonAlbum);
            context.Albums.Add(feverAlbum);

            Song thePoison = new Song
            {
                Name = "The Poison",
                IsOfficial = true,
                Duration = new TimeSpan(0, 0, 3, 39),
                Creator = admin,
                Album = thePoisonAlbum                
            };            

            Song theLastFight = new Song
            {
                Name = "The Last Fight",
                IsOfficial = true,
                Duration = new TimeSpan(0, 0, 4, 19),
                Creator = admin,
                Album = feverAlbum
            };
            context.Songs.Add(thePoison);
            context.Songs.Add(theLastFight);

            Genre metalcore = new Genre
            {
                Name = "Metalcore",
                Info = "Metalcore is a broad fusion genre of extreme metal and hardcore punk. The word is a blend of the names of the two genres. Metalcore is noted for its use of breakdowns, which are slow, intense passages that are conducive to moshing.",
                IsOfficial = true,
                Creator = admin
            };
            context.Genres.Add(metalcore);

            Genre_Album metalcore_thePoison = new Genre_Album
            {
                Genre = metalcore,
                Album = thePoisonAlbum,
                IsOfficial = true,
                Creator = admin
            };

            Genre_Album metalcore_fever = new Genre_Album
            {
                Genre = metalcore,
                Album = feverAlbum,
                IsOfficial = true,
                Creator = admin
            };
            context.Genre_Albums.Add(metalcore_fever);
            context.Genre_Albums.Add(metalcore_thePoison);

            
             context.SaveChanges();
          
            
        }
    }
}
