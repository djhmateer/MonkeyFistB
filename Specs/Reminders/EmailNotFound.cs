using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyFist.Services;
using Xunit;

namespace Specs.Reminders {

  [Trait("Reminders","Email not found")]
  public class EmailNotFound {

    ReminderResult _result;
    public EmailNotFound() {
      _result = new MonkeyFist.Services.Reminders().SendReminderTokenToUser("duh@test.com");
    }

    [Fact(DisplayName = "Is not successful")]
    public void NotSuccessful() {
      Assert.False(_result.Successful);
    }

    [Fact(DisplayName = "A message is provided")]
    public void MessageProvided() {
      Assert.Contains("not found", _result.Message);
    }
  }

}
