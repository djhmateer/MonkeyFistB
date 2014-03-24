using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyFist.Models;
using MonkeyFist.Services;
using Xunit;

namespace Specs.Authentication {
  [Trait("Authentication", "Valid Login with Token")]
  public class ValidTokenLogin : TestBase {
    AuthenticationResult _result;
    public ValidTokenLogin() {
      //register the new user...
      var app = new Application("rob@tekpub.com", "password", "password");
      var result = new Registrator().ApplyForMembership(app);

      var auth = new Authenticator();
      _result = auth.AuthenticateUserByToken(result.NewUser.AuthenticationToken.ToString());
    }

    [Fact(DisplayName = "User authenticated")]
    public void AuthenticateUser() {
      Assert.True(_result.Authenticated);
    }
    [Fact(DisplayName = "User is returned")]
    public void UserReturned() {
      Assert.NotNull(_result.User);
    }

    [Fact(DisplayName = "Log entry created")]
    public void CreateLogEntry() {
      Assert.True(_result.User.Logs.Count > 0);
    }
    [Fact(DisplayName = "A session is created")]
    public void SessionCreated() {
      Assert.True(_result.User.Sessions.Count > 0);

    }
    [Fact(DisplayName = "User has current session")]
    public void RememberMeTokenCreated() {
      Assert.NotNull(_result.User.CurrentSession);
    }
    [Fact(DisplayName = "Session expires in an hour")]
    public void RememberMeExpiresInAnHour() {
      Assert.Equal(_result.User.CurrentSession.EndsAt.ToShortDateString(), DateTime.Now.AddHours(1).ToShortDateString());
    }

    [Fact(DisplayName = "A welcome message is provided")]
    public void WelcomeMessageProvided() {
      Assert.Contains("Welcome", _result.Message);

    }

    [Fact(DisplayName = "The IP is tracked")]
    public void IPTracked() {
      Assert.NotNull(_result.User.IP);
      Assert.NotNull(_result.User.CurrentSession.IP);

    }
    [Fact(DisplayName = "The current sign in is set")]
    public void CurrentLoginSet() {
      Assert.Equal(_result.User.CurrentSignInAt.ToShortDateString(), DateTime.Now.ToShortDateString());
    }
    [Fact(DisplayName = "The last sign in set")]
    public void LastLoginSet() {
      Assert.Equal(_result.User.LastSignInAt.ToShortDateString(), DateTime.Now.ToShortDateString());

    }

    [Fact(DisplayName = "Sign in count is incremented")]
    public void SignInCountIncremented() {
      Assert.True(_result.User.SignInCount > 0);

    }

  }

}
