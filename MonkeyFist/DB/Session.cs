using MonkeyFist.Models;
using System.Data.Entity;

namespace MonkeyFist.DB {
  public class Session :DbContext {
    
    public Session() : base(nameOrConnectionString:"MonkeyFist") {
      // Nice for development
      Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Session>());
    }
    public DbSet<User> Users { get; set; }
    public DbSet<UserActivityLog> ActivityLogs { get; set; }
    public DbSet<UserMailerMessage> MailMessages { get; set; }
    public DbSet<UserSession> Sessions{ get; set; }
    public DbSet<UserMailerTemplate> Mailers { get; set; }
  }
}
