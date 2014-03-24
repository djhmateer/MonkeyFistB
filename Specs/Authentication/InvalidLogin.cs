using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyFist.Models;
using MonkeyFist.Services;
using Xunit;

namespace Specs.Authentication {

  [Trait("Authentication","Email is not found")]
  public class EmailNotFound {
    AuthenticationResult _result;
    public EmailNotFound() {
      _result = new Authenticator().AuthenticateUser(new Credentials { Email = "joe@tekpub.com", Password = "password" });
    }

    [Fact(DisplayName = "Not Authenticated")]
    public void AuthDenied() {
      Assert.False(_result.Authenticated);
    }
    [Fact(DisplayName = "A message is returned explaning")]
    public void MessageReturned() {
      Assert.Contains("Invalid email", _result.Message);
    }
  }
  [Trait("Authentication", "Password doesn't match")]
  public class PasswordDontMatch : TestBase {

    AuthenticationResult _result;
    public PasswordDontMatch() {
      var app = new Application("rob@tekpub.com", "password", "password");
      new Registrator().ApplyForMembership(app);

      _result = new Authenticator().AuthenticateUser(new Credentials { Email = "rob@tekpub.com", Password = "fixlesl" });

    }
    [Fact(DisplayName = "Not authenticated")]
    public void NotAuthenticated() {
      Assert.False(_result.Authenticated);

    }
    [Fact(DisplayName = "Message provided")]
    public void MessageReturned() {
      Assert.Contains("Invalid email", _result.Message);
    }
  }
  

}
