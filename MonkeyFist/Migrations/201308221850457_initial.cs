namespace MonkeyFist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        AuthenticationToken = c.Guid(nullable: false),
                        Email = c.String(nullable: false, maxLength: 255),
                        HashedPassword = c.String(nullable: false, maxLength: 500),
                        Status = c.Int(nullable: false),
                        IP = c.String(),
                        LastSignInAt = c.DateTime(nullable: false),
                        CurrentSignInAt = c.DateTime(nullable: false),
                        SignInCount = c.Int(nullable: false),
                        ReminderToken = c.Guid(nullable: false),
                        ReminderSentAt = c.DateTime(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserActivityLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false, maxLength: 255),
                        Entry = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        User_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.User_ID,cascadeDelete:true)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.UserMailerMessages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false, maxLength: 255),
                        Body = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        Successful = c.Boolean(nullable: false),
                        ResultMessage = c.String(),
                        Mailer_ID = c.Guid(),
                        User_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserMailerTemplates", t => t.Mailer_ID)
                .ForeignKey("dbo.Users", t => t.User_ID, cascadeDelete: true)
                .Index(t => t.Mailer_ID)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.UserMailerTemplates",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Subject = c.String(nullable: false, maxLength: 255),
                        Markdown = c.String(nullable: false, maxLength: 1800),
                        CreatedAt = c.DateTime(nullable: false),
                        MailerType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserSessions",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        IP = c.String(maxLength: 55),
                        StartedAt = c.DateTime(nullable: false),
                        EndsAt = c.DateTime(nullable: false),
                        User_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.User_ID, cascadeDelete: true)
                .Index(t => t.User_ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserSessions", new[] { "User_ID" });
            DropIndex("dbo.UserMailerMessages", new[] { "User_ID" });
            DropIndex("dbo.UserMailerMessages", new[] { "Mailer_ID" });
            DropIndex("dbo.UserActivityLogs", new[] { "User_ID" });
            DropForeignKey("dbo.UserSessions", "User_ID", "dbo.Users");
            DropForeignKey("dbo.UserMailerMessages", "User_ID", "dbo.Users");
            DropForeignKey("dbo.UserMailerMessages", "Mailer_ID", "dbo.UserMailerTemplates");
            DropForeignKey("dbo.UserActivityLogs", "User_ID", "dbo.Users");
            DropTable("dbo.UserSessions");
            DropTable("dbo.UserMailerTemplates");
            DropTable("dbo.UserMailerMessages");
            DropTable("dbo.UserActivityLogs");
            DropTable("dbo.Users");
        }
    }
}
