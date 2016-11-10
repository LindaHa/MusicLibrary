using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace DAL
{
    public class MusicLibraryDbContext : DbContext/*IDentityDbContext<AppUser, AppRole, int, AppUserLogin, Client, AppUserClaim>, */
    {
        public MusicLibraryDbContext() : base("MusicLibraryDbContext")
        {
            InitializeDbContext();
        }

        public MusicLibraryDbContext(string connectionName) : base(connectionName)
        {
            InitializeDbContext();
        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre_Album> Genre_Albums { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Song_Songlist> Song_Songlists { get; set; }
        public DbSet<Songlist> Songlists { get; set; }
        public DbSet<SongReview> SongReviews { get; set; }
        public DbSet<AlbumReview> AlbumReviews { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<UserAccount> Users { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureMembershipRebootUserAccounts<UserAccount>();
        }

        private void InitializeDbContext()
        {
            Database.SetInitializer(new MusicLibraryDbInitializer());
            this.RegisterUserAccountChildTablesForDelete<UserAccount>();
        }
    }
}
