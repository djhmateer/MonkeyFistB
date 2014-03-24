using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyFist.DB;
using MonkeyFist.Models;
using MonkeyFist.Services;
using Xunit;
using System.Data.EntityClient;

namespace Specs.Registration {
  [Trait("Registration", "Existing email")]
  public class ExistingEmail : TestBase {
    RegistrationResult _result;
    public ExistingEmail() : base(){
      var app1 = new Application("existing@tekpub.com", "password", "password");
      _result = new Registrator().ApplyForMembership(app1);

    }

    [Fact(DisplayName = "App doesn't throw")]
    public void AppDoesntThrow() {
      var app2 = new Application("existing@tekpub.com", "password", "password");
      Assert.DoesNotThrow(() => _result = new Registrator().ApplyForMembership(app2));
    }
    [Fact(DisplayName = "Application returns message ")]
    public void ApplicationReturnsMessage() {
      var app2 = new Application("existing@tekpub.com", "password", "password");
      _result = new Registrator().ApplyForMembership(app2);
      Assert.Contains("exists", _result.Application.UserMessage);
    }
    [Fact(DisplayName = "Application is invalid")]
    public void ApplicationIsInvalid() {
      var app2 = new Application("existing@tekpub.com", "password", "password");
      _result = new Registrator().ApplyForMembership(app2);
      Assert.True(app2.IsInvalid());
    }

  }

  [Trait("Registration", "Email <= 5 chars")]
  public class ShortEmail {
    RegistrationResult _result;
    public ShortEmail() {
      var app = new Application("rob@b", "password", "password");
      _result = new Registrator().ApplyForMembership(app);
    }

    [Fact(DisplayName = "Application is invalid")]
    public void UserDenied() {
      Assert.True(_result.Application.IsInvalid());
    }
    [Fact(DisplayName = "A message explains invalidation")]
    public void MessageIsShown() {
      Assert.Contains("invalid", _result.Application.UserMessage);
    }

  }
  [Trait("Registration", "Password <= 4 chars")]
  public class ShortPassword {
    RegistrationResult _result;
    public ShortPassword() {
      var app = new Application("rob@tekpub.com", "pass", "pass");
      _result = new Registrator().ApplyForMembership(app);
    }

    [Fact(DisplayName = "Application is invalid")]
    public void UserDenied() {
      Assert.True(_result.Application.IsInvalid());
    }
    [Fact(DisplayName = "A message explains invalidation")]
    public void MessageIsShown() {
      Assert.Contains("invalid", _result.Application.UserMessage);
    }
  }

  [Trait("Registration", "Password/Confirm Mismatch")]
  public class PasswordMistmatch {
    RegistrationResult _result;
    public PasswordMistmatch() {
      var app = new Application("rob@tekpub.com", "password", "something else");
      _result = new Registrator().ApplyForMembership(app);
    }

    [Fact(DisplayName = "Application is invalid")]
    public void UserDenied() {
      Assert.True(_result.Application.IsInvalid());
    }
    [Fact(DisplayName = "A message explains invalidation")]
    public void MessageIsShown() {
      Assert.Contains("don't match", _result.Application.UserMessage);
    }
  }


}
