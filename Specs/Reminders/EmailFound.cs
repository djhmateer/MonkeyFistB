using MonkeyFist.Models;
using MonkeyFist.Services;
using System;
using Xunit;

namespace Specs.Reminders {

    [Trait("Reminders", "Email Found")]
    public class ValidEmail : TestBase {

        ReminderResult _result;
        public ValidEmail() {
            var app = new Application("test@test.com", "password", "password");
            var result = new Registrator().ApplyForMembership(app);

            _result = new MonkeyFist.Services.Reminders().SendReminderTokenToUser("test@test.com");
        }

        [Fact(DisplayName = "Sets the reminder token")]
        public void SetsReminderToken() {
            Assert.NotNull(_result.User.ReminderToken);
        }

        [Fact(DisplayName = "Sets reminder sent at date")]
        public void SetsReminderSentAt() {
            Assert.Equal(_result.User.ReminderSentAt.Value.ToShortDateString(), DateTime.Now.ToShortDateString());
        }

        [Fact(DisplayName = "Sends email to user")]
        public void SendsEmail() {
            Assert.True(_result.MailMessage.Successful);

        }
        [Fact(DisplayName = "Activity log created")]
        public void ActivityLogCreated() {
            Assert.Equal(1, _result.User.Logs.Count);
        }
        [Fact(DisplayName = "Mail log created")]
        public void MailLogCreated() {
            Assert.Equal(1, _result.User.MailerLogs.Count);

        }

    }
}
