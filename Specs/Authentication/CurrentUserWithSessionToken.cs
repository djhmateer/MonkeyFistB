using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyFist.Models;
using MonkeyFist.Services;
using Xunit;

namespace Specs.Authentication {

  [Trait("Authentication", "Current User with Valid Token")]
  public class CurrentUserWithSessionToken : TestBase {

    User _user;

    public CurrentUserWithSessionToken() {
      var app = new Application("test@test.com", "password", "password");
      var regResult = new Registrator().ApplyForMembership(app);

      _user = new Authenticator().GetCurrentUser(regResult.SessionToken);
      
    }

    [Fact(DisplayName = "User is returned")]
    public void UserIsReturned() {
      Assert.NotNull(_user);
    }

  }

  [Trait("Authentication", "Logging Out")]
  public class LoggingOut : TestBase {

    bool _loggedOut;

    public LoggingOut() {
      var app = new Application("test@test.com", "password", "password");
      var regResult = new Registrator().ApplyForMembership(app);

      _loggedOut = new Authenticator().EndUserSession(regResult.SessionToken);

    }

    [Fact(DisplayName = "The session is ended")]
    public void UserIsReturned() {
      Assert.True(_loggedOut);
    }

  }

}
