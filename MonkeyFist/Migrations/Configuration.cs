namespace MonkeyFist.Migrations {
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MonkeyFist.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MonkeyFist.DB.Session> {
        public Configuration() {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MonkeyFist.DB.Session context) {

            context.Database.ExecuteSqlCommand("DELETE FROM UserMailerTemplates");
            var reminder = new UserMailerTemplate();
            reminder.Subject = "Account Information";
            reminder.MailerType = MailerType.ReminderToken;
            reminder.Markdown = @"## Password Reset Link

Someone, hopefully you, asked to reset your password. Just [click right here]({LINK}) and we'll get you on your way";

            context.Mailers.Add(reminder);
            var confirmation = new UserMailerTemplate();
            confirmation.Subject = "Email Confirmation";
            confirmation.Markdown = @"## Please Confirm Your Email

Thank you for registering with us - just one last thing we need to do. Please [click right here]({LINK}) so we can confirm your email";

            context.Mailers.Add(reminder);
            context.Mailers.Add(confirmation);
            context.SaveChanges();
        }
    }
}
