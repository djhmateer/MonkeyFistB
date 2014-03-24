using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyFist.Models;

namespace MonkeyFist.DB {
  public class Session :DbContext {
    
    public Session() : base(nameOrConnectionString:"MonkeyFist") {
      //nice for development
      Database.SetInitializer<Session>(new DropCreateDatabaseIfModelChanges<Session>());
    }
    public DbSet<User> Users { get; set; }
    public DbSet<UserActivityLog> ActivityLogs { get; set; }
    public DbSet<UserMailerMessage> MailMessages { get; set; }
    public DbSet<UserSession> Sessions{ get; set; }
    public DbSet<UserMailerTemplate> Mailers { get; set; }
  }
}
