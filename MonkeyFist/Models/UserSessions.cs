using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFist.Models {
  public class UserSession {

    public UserSession() {
      this.StartedAt = DateTime.Now;
      this.ID = Guid.NewGuid();
      //default ends at to 30 days
      this.EndsAt = DateTime.Now.AddDays(30);
    }
    [MaxLength(55)]
    public string IP { get; set; }
    public Guid ID { get; set; }

    [Required]
    public DateTime StartedAt { get; set; }
    public DateTime EndsAt { get; set; }
    public User User { get; set; }
  }
}
