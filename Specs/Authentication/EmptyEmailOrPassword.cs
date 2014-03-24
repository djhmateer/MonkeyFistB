using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyFist.Services;
using Xunit;

namespace Specs.Authentication {

  [Trait("Authentication","Empty email")]
  public class EmptyEmail:TestBase {
    AuthenticationResult _result;
    public EmptyEmail() {
      _result = new Authenticator().AuthenticateUser(new Credentials { Email = "" });
    }

    [Fact(DisplayName = "Not Authenticated")]
    public void AuthDenied() {
      Assert.False(_result.Authenticated);
    }
    [Fact(DisplayName = "A message is returned explaning")]
    public void MessageReturned() {
      Assert.Contains("required", _result.Message);
    }

  }

  [Trait("Authentication", "Empty password")]
  public class EmptyPassword : TestBase {
    AuthenticationResult _result;
    public EmptyPassword() {
      _result = new Authenticator().AuthenticateUser(new Credentials { Email = "rob@tekpub.com", Password="" });
    }

    [Fact(DisplayName = "Not Authenticated")]
    public void AuthDenied() {
      Assert.False(_result.Authenticated);
    }
    [Fact(DisplayName = "A message is returned explaning")]
    public void MessageReturned() {
      Assert.Contains("required", _result.Message);
    }

  }
}
