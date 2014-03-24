using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyFist.Models;
using MonkeyFist.Services;

namespace MonkeyFist {
  public interface IMembership {
    AuthenticationResult Authenticate(MonkeyFist.Services.Credentials creds);
    AuthenticationResult Authenticate(string authenticationToken, string ipAddress = "127.0.0.1");
    AuthenticationResult Authenticate(string email, string password, string ipAddress = "127.0.0.1");
    User CurrentUser(string sessionToken);
    RegistrationResult Register(string email, string password, string confirm, string ipAddress = "127.0.0.1");
    ResetResult ResetUsersPassword(string resetToken, string newPassword);
    ReminderResult SendResetTokenToUser(string email, string resetUrl = "http://localhost/reminders/");
  }

  public class Membership : MonkeyFist.IMembership {

    Authenticator _auth;
    Registrator _reg;
    Reminders _reminders;

    public Membership() : this(new Configuration()) {}

    public Membership(Configuration config) {
      _auth = config.AuthenticationService ?? new Authenticator(config);
      _reg = config.RegistrationService ?? new Registrator(config);
      _reminders = config.ReminderService ?? new Reminders(config);
    }

    User _currentUser;
    public User CurrentUser(string sessionToken) {
      if (_currentUser == null) {
        _currentUser = _auth.GetCurrentUser(Guid.Parse(sessionToken));
      }
      return _currentUser;
    }

    public AuthenticationResult Authenticate(string email, string password, string ipAddress = "127.0.0.1") {
      return Authenticate(new Credentials(email, password, ipAddress));
    }
    public AuthenticationResult Authenticate(Credentials creds) {
      return _auth.AuthenticateUser(creds);
    }
    public AuthenticationResult Authenticate(string authenticationToken, string ipAddress = "127.0.0.1") {
      return _auth.AuthenticateUserByToken(authenticationToken, ipAddress);
    }

    public RegistrationResult Register(string email, string password, string confirm, string ipAddress = "127.0.0.1") {
      var app = new Application(email, password, confirm);
      return _reg.ApplyForMembership(app);
    }

    public ReminderResult SendResetTokenToUser(string email, string resetUrl = "http://localhost/reminders/") {
      return _reminders.SendReminderTokenToUser(email);
    }

    public ResetResult ResetUsersPassword(string resetToken, string newPassword) {
      return _reminders.ResetUserPassword(Guid.Parse(resetToken), newPassword);
    }

  }
}
