using MonkeyFist.DB;
using System;
using System.IO;

namespace Specs {
    public class TestBase : IDisposable {

        public TestBase() {
            // Clear out all database except UserMailerTemplates
            new Session().Database.ExecuteSqlCommand("DELETE FROM useractivitylogs;DELETE FROM usermailermessages;DELETE FROM usersessions;DELETE FROM users");
            Directory.CreateDirectory(@"c:\temp\maildrop");
            // Clean out the mailers
            foreach (FileInfo file in new DirectoryInfo(@"c:\temp\maildrop").GetFiles()) {
                file.Delete();
            }
        }

        public void Dispose() {
            new Session().Database.ExecuteSqlCommand("DELETE FROM useractivitylogs;DELETE FROM usermailermessages;DELETE FROM usersessions;DELETE FROM users");
        }

    }
}
