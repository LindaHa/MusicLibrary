namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlbumReviews",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 1024),
                        UserRating = c.Double(nullable: false),
                        Album_ID = c.Int(nullable: false),
                        Creator_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Albums", t => t.Album_ID, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.Creator_ID)
                .Index(t => t.Album_ID)
                .Index(t => t.Creator_ID);
            
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        IsOfficial = c.Boolean(nullable: false),
                        Artist_ID = c.Int(nullable: false),
                        Creator_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Artists", t => t.Artist_ID, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.Creator_ID)
                .Index(t => t.Artist_ID)
                .Index(t => t.Creator_ID);
            
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Info = c.String(maxLength: 1024),
                        IsOfficial = c.Boolean(nullable: false),
                        Creator_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clients", t => t.Creator_ID)
                .Index(t => t.Creator_ID);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Account_Key = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserAccounts", t => t.Account_Key, cascadeDelete: true)
                .Index(t => t.Account_Key);
            
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false),
                        Tenant = c.String(nullable: false, maxLength: 50),
                        Username = c.String(nullable: false, maxLength: 254),
                        Created = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        IsAccountClosed = c.Boolean(nullable: false),
                        AccountClosed = c.DateTime(),
                        IsLoginAllowed = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(),
                        LastFailedLogin = c.DateTime(),
                        FailedLoginCount = c.Int(nullable: false),
                        PasswordChanged = c.DateTime(),
                        RequiresPasswordReset = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 254),
                        IsAccountVerified = c.Boolean(nullable: false),
                        LastFailedPasswordReset = c.DateTime(),
                        FailedPasswordResetCount = c.Int(nullable: false),
                        MobileCode = c.String(maxLength: 100),
                        MobileCodeSent = c.DateTime(),
                        MobilePhoneNumber = c.String(maxLength: 20),
                        MobilePhoneNumberChanged = c.DateTime(),
                        AccountTwoFactorAuthMode = c.Int(nullable: false),
                        CurrentTwoFactorAuthStatus = c.Int(nullable: false),
                        VerificationKey = c.String(maxLength: 100),
                        VerificationPurpose = c.Int(),
                        VerificationKeySent = c.DateTime(),
                        VerificationStorage = c.String(maxLength: 100),
                        HashedPassword = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        Type = c.String(nullable: false, maxLength: 150),
                        Value = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.LinkedAccountClaims",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        ProviderName = c.String(nullable: false, maxLength: 30),
                        ProviderAccountID = c.String(nullable: false, maxLength: 100),
                        Type = c.String(nullable: false, maxLength: 150),
                        Value = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.LinkedAccounts",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        ProviderName = c.String(nullable: false, maxLength: 30),
                        ProviderAccountID = c.String(nullable: false, maxLength: 100),
                        LastLogin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.PasswordResetSecrets",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        PasswordResetSecretID = c.Guid(nullable: false),
                        Question = c.String(nullable: false, maxLength: 150),
                        Answer = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.TwoFactorAuthTokens",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        Token = c.String(nullable: false, maxLength: 100),
                        Issued = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.UserCertificates",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        Thumbprint = c.String(nullable: false, maxLength: 150),
                        Subject = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.Songlists",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Owner_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clients", t => t.Owner_ID, cascadeDelete: true)
                .Index(t => t.Owner_ID);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Duration = c.Time(nullable: false, precision: 7),
                        IsOfficial = c.Boolean(nullable: false),
                        Album_ID = c.Int(nullable: false),
                        Creator_ID = c.Int(),
                        Songlist_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Albums", t => t.Album_ID, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.Creator_ID)
                .ForeignKey("dbo.Songlists", t => t.Songlist_ID)
                .Index(t => t.Album_ID)
                .Index(t => t.Creator_ID)
                .Index(t => t.Songlist_ID);
            
            CreateTable(
                "dbo.SongReviews",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 1024),
                        UserRating = c.Double(nullable: false),
                        Creator_ID = c.Int(),
                        Song_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clients", t => t.Creator_ID)
                .ForeignKey("dbo.Songs", t => t.Song_ID, cascadeDelete: true)
                .Index(t => t.Creator_ID)
                .Index(t => t.Song_ID);
            
            CreateTable(
                "dbo.Genre_Album",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IsOfficial = c.Boolean(nullable: false),
                        Album_ID = c.Int(nullable: false),
                        Creator_ID = c.Int(),
                        Genre_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Albums", t => t.Album_ID, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.Creator_ID)
                .ForeignKey("dbo.Genres", t => t.Genre_ID, cascadeDelete: true)
                .Index(t => t.Album_ID)
                .Index(t => t.Creator_ID)
                .Index(t => t.Genre_ID);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Info = c.String(maxLength: 1024),
                        IsOfficial = c.Boolean(nullable: false),
                        Creator_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clients", t => t.Creator_ID)
                .Index(t => t.Creator_ID);
            
            CreateTable(
                "dbo.Song_Songlist",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Creator_ID = c.Int(),
                        Song_ID = c.Int(nullable: false),
                        Songlist_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clients", t => t.Creator_ID)
                .ForeignKey("dbo.Songs", t => t.Song_ID, cascadeDelete: true)
                .ForeignKey("dbo.Songlists", t => t.Songlist_ID, cascadeDelete: true)
                .Index(t => t.Creator_ID)
                .Index(t => t.Song_ID)
                .Index(t => t.Songlist_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Song_Songlist", "Songlist_ID", "dbo.Songlists");
            DropForeignKey("dbo.Song_Songlist", "Song_ID", "dbo.Songs");
            DropForeignKey("dbo.Song_Songlist", "Creator_ID", "dbo.Clients");
            DropForeignKey("dbo.Genre_Album", "Genre_ID", "dbo.Genres");
            DropForeignKey("dbo.Genres", "Creator_ID", "dbo.Clients");
            DropForeignKey("dbo.Genre_Album", "Creator_ID", "dbo.Clients");
            DropForeignKey("dbo.Genre_Album", "Album_ID", "dbo.Albums");
            DropForeignKey("dbo.AlbumReviews", "Creator_ID", "dbo.Clients");
            DropForeignKey("dbo.AlbumReviews", "Album_ID", "dbo.Albums");
            DropForeignKey("dbo.Albums", "Creator_ID", "dbo.Clients");
            DropForeignKey("dbo.Albums", "Artist_ID", "dbo.Artists");
            DropForeignKey("dbo.Artists", "Creator_ID", "dbo.Clients");
            DropForeignKey("dbo.Songs", "Songlist_ID", "dbo.Songlists");
            DropForeignKey("dbo.SongReviews", "Song_ID", "dbo.Songs");
            DropForeignKey("dbo.SongReviews", "Creator_ID", "dbo.Clients");
            DropForeignKey("dbo.Songs", "Creator_ID", "dbo.Clients");
            DropForeignKey("dbo.Songs", "Album_ID", "dbo.Albums");
            DropForeignKey("dbo.Songlists", "Owner_ID", "dbo.Clients");
            DropForeignKey("dbo.Clients", "Account_Key", "dbo.UserAccounts");
            DropForeignKey("dbo.UserCertificates", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.TwoFactorAuthTokens", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.PasswordResetSecrets", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.LinkedAccounts", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.LinkedAccountClaims", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.UserClaims", "ParentKey", "dbo.UserAccounts");
            DropIndex("dbo.Song_Songlist", new[] { "Songlist_ID" });
            DropIndex("dbo.Song_Songlist", new[] { "Song_ID" });
            DropIndex("dbo.Song_Songlist", new[] { "Creator_ID" });
            DropIndex("dbo.Genres", new[] { "Creator_ID" });
            DropIndex("dbo.Genre_Album", new[] { "Genre_ID" });
            DropIndex("dbo.Genre_Album", new[] { "Creator_ID" });
            DropIndex("dbo.Genre_Album", new[] { "Album_ID" });
            DropIndex("dbo.SongReviews", new[] { "Song_ID" });
            DropIndex("dbo.SongReviews", new[] { "Creator_ID" });
            DropIndex("dbo.Songs", new[] { "Songlist_ID" });
            DropIndex("dbo.Songs", new[] { "Creator_ID" });
            DropIndex("dbo.Songs", new[] { "Album_ID" });
            DropIndex("dbo.Songlists", new[] { "Owner_ID" });
            DropIndex("dbo.UserCertificates", new[] { "ParentKey" });
            DropIndex("dbo.TwoFactorAuthTokens", new[] { "ParentKey" });
            DropIndex("dbo.PasswordResetSecrets", new[] { "ParentKey" });
            DropIndex("dbo.LinkedAccounts", new[] { "ParentKey" });
            DropIndex("dbo.LinkedAccountClaims", new[] { "ParentKey" });
            DropIndex("dbo.UserClaims", new[] { "ParentKey" });
            DropIndex("dbo.Clients", new[] { "Account_Key" });
            DropIndex("dbo.Artists", new[] { "Creator_ID" });
            DropIndex("dbo.Albums", new[] { "Creator_ID" });
            DropIndex("dbo.Albums", new[] { "Artist_ID" });
            DropIndex("dbo.AlbumReviews", new[] { "Creator_ID" });
            DropIndex("dbo.AlbumReviews", new[] { "Album_ID" });
            DropTable("dbo.Song_Songlist");
            DropTable("dbo.Genres");
            DropTable("dbo.Genre_Album");
            DropTable("dbo.SongReviews");
            DropTable("dbo.Songs");
            DropTable("dbo.Songlists");
            DropTable("dbo.UserCertificates");
            DropTable("dbo.TwoFactorAuthTokens");
            DropTable("dbo.PasswordResetSecrets");
            DropTable("dbo.LinkedAccounts");
            DropTable("dbo.LinkedAccountClaims");
            DropTable("dbo.UserClaims");
            DropTable("dbo.UserAccounts");
            DropTable("dbo.Clients");
            DropTable("dbo.Artists");
            DropTable("dbo.Albums");
            DropTable("dbo.AlbumReviews");
        }
    }
}
