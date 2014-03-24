using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyFist.Services;

namespace MonkeyFist {
  public class Configuration {

    public Configuration() {
      this.ResetUrl = "http://localhost/reminders/";
      this.ConfirmationUrl = "http://localhost/email/confirm";
      this.ConnectionStringName = "MonkeyFist";
      this.MinPasswordLength = 4;
      this.RequireEmailConfirmation = false;
      this.DefaultUserSessionDays = 30;
      this.AuthenticationService = new Authenticator(this);
      this.RegistrationService = new Registrator(this);
      this.ReminderService = new Reminders(this);
    }
    public Registrator RegistrationService { get; set; }
    public Authenticator AuthenticationService { get; set; }
    public Reminders ReminderService { get; set; }
    public string ResetUrl { get; set; }
    public string ConfirmationUrl { get; set; }
    public string ConnectionStringName { get; set; }
    public bool RequireEmailConfirmation { get; set; }
    public int MinPasswordLength { get; set; }
    public int DefaultUserSessionDays { get; set; }

  }
}
