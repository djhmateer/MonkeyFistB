using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevOne.Security.Cryptography.BCrypt;
using MonkeyFist.DB;
using MonkeyFist.Models;

namespace MonkeyFist.Services {

  public class AuthenticationResult {
    public bool Authenticated { get; set; }
    public string Message { get; set; }
    public User User { get; set; }
    public UserSession Session { get; set; }
  }

  public class Credentials {
    public Credentials(string email, string password, string IP="127.0.0.1") {
      this.Email = email;
      this.Password = password;
      this.IP = IP;
    }
    public Credentials() {
      this.IP = "127.0.0.1";
      this.RememberMe = false;
    }
    public string Email { get; set; }
    public string Password { get; set; }
    public string IP { get; set; }
    public bool RememberMe { get; set; }
    public Guid Token { get; set; }
  }

  public class Authenticator {
    Credentials CurrentCredentials;
    Session _session;
    Configuration _config;

    public Authenticator(Configuration config = null) {
      _config = config ?? new Configuration();
    }



    public virtual UserSession CreateSession(User user) {
      var session = new UserSession { IP = CurrentCredentials.IP };
      session.EndsAt = CurrentCredentials.RememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddHours(6);
      user.Sessions.Add(session);
      return session;
    }

    public virtual User LocateUser() {
      return _session.Users.FirstOrDefault(x => x.Email == CurrentCredentials.Email);
    }

    public virtual void SetUserLoginStats(User user) {

      user.SignInCount += 1;
      user.CurrentSignInAt = user.LastSignInAt;
      user.LastSignInAt = DateTime.Now;
      user.IP = CurrentCredentials.IP;

    }

    public virtual void UserAuthenticated(User user) {}

    public virtual User FindUserByAuthenticationToken() {
      return _session.Users.FirstOrDefault(x => x.AuthenticationToken == CurrentCredentials.Token);
    }

    public virtual User GetCurrentUser(Guid sessionToken) {
      User user = null;
      using (var session = new Session()) {
        var validSession = session.Sessions.Include("User").FirstOrDefault(x => x.ID == sessionToken && x.EndsAt > DateTime.Now);
        if (validSession != null) {
          user = validSession.User;
        }
      }
      return user;
    }

    public virtual bool EndUserSession(Guid sessionToken) {
      var result = false;
      using (var session = new Session()) {
        var userSession = session.Sessions.FirstOrDefault(x => x.ID == sessionToken);
        if (userSession != null) {
          userSession.EndsAt = DateTime.Now;
          session.SaveChanges();
          result = true;
        }
      }
      return result;
    }

    public AuthenticationResult AuthenticateUserByToken(string token, string ip = "127.0.0.1") {
      var result = new AuthenticationResult();
      _session = new Session();

      if (String.IsNullOrWhiteSpace(token)) {
        result = InvalidLogin("No token provided");
      } else {
        this.CurrentCredentials = new Credentials { Token = Guid.Parse(token), IP = ip };

        var user = FindUserByAuthenticationToken();
        if (user == null) {
          result = InvalidLogin("Invalid token");
        } else {
          //success
          user.AddLogEntry("Login", "User logged in by token");
          result.Session = CreateSession(user);
          SetUserLoginStats(user);
          UserAuthenticated(user);

          result.Authenticated = true;
          result.User = user;
          result.Message = Properties.Resources.UserAuthenticated;
          _session.SaveChanges();
        }
      }
      _session.Dispose();
      return result;
    }

    public AuthenticationResult AuthenticateUser(Credentials creds) {
      _session = new Session();
      var result = new AuthenticationResult();
      User user = null;
      this.CurrentCredentials = creds;

      if (EmailOrPasswordNotPresent()) {
        result = InvalidLogin(Properties.Resources.EmailOrPasswordMissing);
      } else {
        //find the user
        user = LocateUser();

        //if they're not here, we're done
        if (user == null) {
          result = InvalidLogin(Properties.Resources.InvalidLogin);
        
          //does the password match?
        } else if (HashedPasswordDoesNotMatch(user)) {
          result = InvalidLogin(Properties.Resources.InvalidLogin);
        
          //success
        } else {
          //success!
          user.AddLogEntry("Login", "User logged in");
          result.Session = CreateSession(user);

          SetUserLoginStats(user);
          //save changes
          UserAuthenticated(user);

          result.Authenticated = true;
          result.User = user;
          result.Message = Properties.Resources.UserAuthenticated;

          _session.SaveChanges();
        }
      }

      //dispose of this
      _session.Dispose();

      return result;
    }

    public virtual bool HashedPasswordDoesNotMatch(User user) {
      return !BCryptHelper.CheckPassword(CurrentCredentials.Password, user.HashedPassword);
    }

    private AuthenticationResult InvalidLogin(string message) {
      if(_session!=null)
        _session.Dispose();
      return new AuthenticationResult { Message = message, Authenticated = false };
    }

    private bool EmailOrPasswordNotPresent() {
      return String.IsNullOrWhiteSpace(CurrentCredentials.Email) ||
        String.IsNullOrWhiteSpace(CurrentCredentials.Password);
    }

  }
}
