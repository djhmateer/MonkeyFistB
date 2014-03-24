using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevOne.Security.Cryptography.BCrypt;
using MonkeyFist.DB;
using MonkeyFist.Models;

namespace MonkeyFist.Services {
  public class RegistrationResult {
    public User NewUser { get; set; }
    public Application Application { get; set; }
    public Guid SessionToken { get; set; }
    public bool Successful {
      get {
        return this.Application == null ? false : this.Application.IsAccepted();
      }
    }
  }

  public class Registrator {

    Configuration _config;

    public Registrator(Configuration config = null) {
      _config = config ?? new Configuration();
    }

    protected Application CurrentApplication;

    bool EmailOrPasswordNotPresent() {
      return String.IsNullOrWhiteSpace(CurrentApplication.Email) || 
        String.IsNullOrWhiteSpace(CurrentApplication.Password);
    }

    public virtual bool EmailAlreadyRegistered() {
      var exists = false;
      using (var session = new Session()) {
        exists = session.Users.FirstOrDefault(x => x.Email == CurrentApplication.Email) != null;
      }
      return exists;
    }

    public virtual bool EmailIsInvalid() {
      return CurrentApplication.Email.Length <= 5;
    }

    public virtual bool PasswordIsInvalid() {
      return CurrentApplication.Password.Length <= _config.MinPasswordLength;
    }

    public virtual bool PasswordDoesNotMatchConfirmation() {
      return !CurrentApplication.Password.Equals(CurrentApplication.Confirmation);
    }

    public RegistrationResult InvalidApplication(string reason) {
      var result = new RegistrationResult();
      CurrentApplication.Status = ApplicationStatus.Invalid;
      result.Application = CurrentApplication;
      result.Application.UserMessage = reason;
      return result;
    }


    public virtual string HashPassword() {
      return BCryptHelper.HashPassword(CurrentApplication.Password, BCryptHelper.GenerateSalt(10));
    }


    public virtual User CreateUserFromCurrentApplication() {
      return new User { 
        Email = CurrentApplication.Email, 
        HashedPassword = HashPassword(),
        Status = UserStatus.Pending,
        IP = CurrentApplication.IPAddress
      };
    }


    public virtual User AcceptApplication() {
      User user = null;
      using (var session = new Session()) {
        //set the status
        CurrentApplication.Status = ApplicationStatus.Accepted;

        //crete the new user
        user = CreateUserFromCurrentApplication();

        //log the registration
        user.AddLogEntry("Registration", "User with email " + user.Email + " successfully registered");

        //send off an email
        var mailer = session.Mailers.FirstOrDefault(x => x.MailerType == MailerType.EmailConfirmation);
        if (mailer != null && _config.RequireEmailConfirmation) {
          //TODO need to hook this up so the email can be confirmed
          var message = UserMailerMessage.CreateFromTemplate(mailer, _config.ConfirmationUrl + "?t="+user.AuthenticationToken);
          message.SendTo(user);
        }

        user.AddLogEntry("Registration", "Email confirmation request sent");

        //save the user down
        session.Users.Add(user);
        session.SaveChanges();
      }
      return user;
    }

    public RegistrationResult ApplyForMembership(Application app) {
      var result = new RegistrationResult();

      CurrentApplication = app;
      result.Application = app;
      result.Application.UserMessage = "Welcome!";

      if (EmailOrPasswordNotPresent())
        return InvalidApplication(Properties.Resources.EmailOrPasswordMissing);

      if (EmailIsInvalid())
        return InvalidApplication(Properties.Resources.InvalidEmailMessage);

      if (PasswordIsInvalid())
        return InvalidApplication(Properties.Resources.InvalidPassword);

      if (PasswordDoesNotMatchConfirmation())
        return InvalidApplication(Properties.Resources.PasswordConfirmationMismatch);

      if (EmailAlreadyRegistered())
        return InvalidApplication(Properties.Resources.EmailExists);

      //Accept the application
      result.NewUser = AcceptApplication();
      
      //log them in
      var auth = new Authenticator().AuthenticateUser(new Credentials { Email = result.NewUser.Email, Password = CurrentApplication.Password });
      result.SessionToken = auth.Session.ID;

      return result;

    }
  }
}
