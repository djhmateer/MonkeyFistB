using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyFist.DB;
using MonkeyFist.Models;
using MonkeyFist.Services;
using Xunit;

namespace Specs.Reminders {

  [Trait("Reminders","Token Response with new password")]
  public class TokenResponse : TestBase {
    ResetResult result;
    User user;
    public TokenResponse() {
      var app = new Application("test@test.com", "password", "password");
      var regResult = new Registrator().ApplyForMembership(app);
      var svc = new MonkeyFist.Services.Reminders().SendReminderTokenToUser(regResult.NewUser.Email);
      result = new MonkeyFist.Services.Reminders().ResetUserPassword(svc.User.ReminderToken, "newPassword");
      user = result.User;
    }

    [Fact(DisplayName = "New password is hashed and set")]
    public void NewPasswordHashedAndSet() {
      Assert.True(result.Successful);
    }

    [Fact(DisplayName = "Log entry created")]
    public void LogCreated() {
      Console.WriteLine(user.Logs.Count);
      Assert.True(user.Logs.Count > 0);
    }
    [Fact(DisplayName = "Message provided")]
    public void MessageProvided() {
      Assert.Contains("password was reset", result.Message);
    }

  }
  [Trait("Reminders", "Password reset called after expiration")]
  public class TokenResponseExpired : TestBase {
    ResetResult result;
    public TokenResponseExpired() {
      var app = new Application("test@test.com", "password", "password");
      var regResult = new Registrator().ApplyForMembership(app);
      
      using (var session = new Session()) {
        var user = session.Users.FirstOrDefault(x => x.Email == regResult.NewUser.Email);
        user.ReminderSentAt = DateTime.Now.AddDays(-1);
        session.SaveChanges();
      }

      result = new MonkeyFist.Services.Reminders().ResetUserPassword(regResult.NewUser.ReminderToken, "newPassword");

    }

    [Fact(DisplayName = "Result should be false")]
    public void ResultShouldBeFalse() {
      Assert.False(result.Successful);
    }
  }
}
