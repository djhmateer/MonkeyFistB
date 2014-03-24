using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFist.Models {

  public enum UserStatus {
    Pending = 1,
    InvalidEmail = 66
  }
  
  public class User {
    public User() {
      this.ID = Guid.NewGuid();
      this.Status = UserStatus.Pending;
      this.Logs = new List<UserActivityLog>();
      this.MailerLogs = new List<UserMailerMessage>();
      this.Sessions = new List<UserSession>();
      this.CreatedAt = DateTime.Now;
      this.LastSignInAt = DateTime.Now;
      this.CurrentSignInAt = DateTime.Now;
      this.SignInCount = 0;
      this.AuthenticationToken = Guid.NewGuid();
      this.ReminderToken = Guid.NewGuid();
    }

    [Required]
    public Guid AuthenticationToken { get; set; }

    [MaxLength(255)]
    [Required]
    public string Email { get; set; }
    [MaxLength(500)]
    [Required]
    public string HashedPassword { get; set; }
    public Guid ID { get; set; }
    [Required]
    public UserStatus Status{ get; set; }
    public string IP { get; set; }
    public DateTime LastSignInAt { get; set; }
    public DateTime CurrentSignInAt { get; set; }
    [Required]
    public int SignInCount { get; set; }


    public Guid ReminderToken { get; set; }
    public DateTime? ReminderSentAt { get; set; }

    public ICollection<UserActivityLog> Logs { get; set; }
    public ICollection<UserMailerMessage> MailerLogs { get; set; }
    public ICollection<UserSession> Sessions { get; set; }

    public DateTime CreatedAt { get; set; }

    public void AddLogEntry(string subject, string entry) {
      this.Logs.Add(new UserActivityLog { Subject = subject, Entry = entry });

    }

    public UserSession CurrentSession {
      get {
        return this.Sessions.OrderByDescending(x => x.EndsAt).FirstOrDefault();
      }
    }

  }
}
