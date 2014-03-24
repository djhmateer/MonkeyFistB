using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyFist.DB;

namespace Specs {
  public class TestBase :IDisposable {

    public TestBase() {
      new Session().Database.ExecuteSqlCommand("delete from useractivitylogs;delete from usermailermessages;delete from usersessions;delete from users");
      //clear out emails
      Directory.CreateDirectory(@"c:\temp\maildrop");
      //clean out the mailers
      foreach (FileInfo file in new DirectoryInfo(@"c:\temp\maildrop").GetFiles()) {
        file.Delete();
      }
    }

    public void Dispose() {
      new Session().Database.ExecuteSqlCommand("delete from useractivitylogs;delete from usermailermessages;delete from usersessions;delete from users");
    }

  }
}
