using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyFist.Models;
using Xunit;

namespace Specs.Mailers {

  [Trait("Mailers","Valid Mailer")]
  public class ValidMailer {
    UserMailerTemplate _mailer;

    public ValidMailer() {
      _mailer = new UserMailerTemplate();
      _mailer.Subject = "I'm descriptive";
      _mailer.Markdown = "## Hello";
      _mailer.MailerType = MailerType.ReminderToken;
    }

    [Fact(DisplayName = "Transforms Markdown to HTML")]
    public void TransformsMarkdownToHTML() {
      var html = _mailer.FormatBody("");
      Assert.Contains("<h2>Hello</h2>", html);

    }

    [Fact(DisplayName = "Has a descriptive subject")]
    public void HasDescriptiveSubject() {
      Assert.Equal("I'm descriptive", _mailer.Subject);

    }

    [Fact(DisplayName = "Has a descriptive type")]
    public void HasDescriptiveType() {
      Assert.Equal(MailerType.ReminderToken, _mailer.MailerType);
    }

  }
}
