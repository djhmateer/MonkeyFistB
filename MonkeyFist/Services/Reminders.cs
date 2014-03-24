using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevOne.Security.Cryptography.BCrypt;
using MonkeyFist.DB;
using MonkeyFist.Models;

namespace MonkeyFist.Services {

  public class ResetResult {
    public bool Successful { get; set; }
    public User User { get; set; }
    public string Message { get; set; }
  }

  public class ReminderResult {
    public string Message { get; set; }
    public Guid Token { get; set; }
    public DateTime SentAt { get; set; }
    public UserMailerMessage MailMessage { get; set; }
    public User User { get; set; }
    public bool Successful {
      get {
        return this.MailMessage == null ? false : this.MailMessage.Successful;
      }
    }
  }

  public class Reminders {
    Session _session;
    Configuration _config;

    public Reminders(Configuration config = null) {
      _config = config ?? new Configuration();
    }

    public virtual UserMailerTemplate GetReminderMailer() {
      return _session.Mailers.FirstOrDefault(x => x.MailerType == MailerType.ReminderToken);
    }
    public virtual string CreateReminderLink(User user) {
      return _config.ResetUrl + "?t=" + user.ReminderToken;
    }
    public virtual User GetUserByEmail(string email) {
      return _session.Users.FirstOrDefault(x => x.Email == email);
    }

    public ReminderResult SendReminderTokenToUser(string email) {
      _session = new Session();
      var result = new ReminderResult();
      result.User = GetUserByEmail(email);

      if (result.User != null) {
        result.User.ReminderToken = Guid.NewGuid();
        result.User.ReminderSentAt = DateTime.Now;

        var mailer = GetReminderMailer();
        var link = CreateReminderLink(result.User);
        var message = UserMailerMessage.CreateFromTemplate(mailer,link);


        if (message.Successful) {
          result.User.AddLogEntry("Login", "Reminder email sent at " + DateTime.Now.ToShortDateString());
        } else {
          result.User.AddLogEntry("Login", "Reminder email failed to send  " + DateTime.Now.ToShortDateString());
        }

        result.MailMessage = message.SendTo(result.User);

        _session.SaveChanges();
      } else {
        result.Message = Properties.Resources.EmailNotFound;
      }

  
      _session.Dispose();

      return result;
    }


    public virtual bool ResetWindowIsOpen(User user) {
      return user.ReminderSentAt > DateTime.Now.AddHours(-12);
    }

    public virtual bool PasswordResetIsValid(string newPassword) {
      return !String.IsNullOrWhiteSpace(newPassword) && newPassword.Length > 4;
    }

    public ResetResult ResetUserPassword(Guid token, string newPassword) {
      var result = new ResetResult();
      _session = _session ?? new Session();
      var user = GetUserByToken(token);
      if (user != null) {
        if (PasswordResetIsValid(newPassword)) {
          if (ResetWindowIsOpen(user)) {

            var hashed = BCryptHelper.HashPassword(newPassword, BCryptHelper.GenerateSalt(10));
            user.HashedPassword = hashed;
            user.AddLogEntry("Login", "Password was reset");
            _session.SaveChanges();
            result.Successful = true;
            result.Message = Properties.Resources.PasswordResetSuccessful;
            result.User = user;
          } else {
            result.Message = Properties.Resources.PasswordResetExpired;
          }
        } else {
          result.Message = Properties.Resources.InvalidPassword;
        }
      } else {
        result.Message = Properties.Resources.PasswordResetTokenInvalid;
      }
      _session.Dispose();
      return result;
    }

    private User GetUserByToken(Guid token) {
      return _session.Users.FirstOrDefault(x => x.ReminderToken == token);
    }
  }
}
