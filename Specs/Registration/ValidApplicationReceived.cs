using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyFist.Services;
using MonkeyFist.Models;
using Xunit;
using MonkeyFist.DB;

namespace Specs.Registration {

  [Trait("Registration", "Valid Application")]
  public class ValidApplicationReceived :TestBase {

    Registrator _reg;
    RegistrationResult _result;
    User _user;
    
    public ValidApplicationReceived() : base() {
      _reg = new Registrator();
      var app = new Application(email:"rob@tekpub.com",password:"password",confirm:"password");
      _result = _reg.ApplyForMembership(app);
      _user = _result.NewUser;
    }

    [Fact(DisplayName = "Application is validated")]
    public void ApplicationValidated() {
      Assert.True(_result.Application.IsValid());
    }
    [Fact(DisplayName = "Application is Accecpted")]
    public void ApplicationAccepted() {
      Assert.True(_result.Application.IsAccepted());
    }
    [Fact(DisplayName="A user is added to the system")]
    public void User_Be_Added_To_System() {
      Assert.NotNull(_user);
    }
    
    [Fact(DisplayName="User status set to Pending")]
    public void User_Status_Set_to_Pending() {
      Assert.Equal(UserStatus.Pending, _user.Status);
    } 
    [Fact(DisplayName="Log entry created for event")]
    public void Log_Entry_Is_Created_For_Event() {
      Assert.True(_result.NewUser.Logs.Count > 0);
    }

    [Fact(DisplayName="A confirmation message is provided to show to the user")]
    public void A_Message_is_Provided_for_User() {
      Assert.Equal("Welcome!", _result.Application.UserMessage);
    }

  }
}
