using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFist.Models {

  public enum MailerType {
    EmailConfirmation,
    ReminderToken
  }

  public class UserMailerTemplate {

    public UserMailerTemplate() {
      this.ID = Guid.NewGuid();
      this.CreatedAt = DateTime.Now;
    }
    public Guid ID { get; set; }
    [Required]
    [MaxLength(255)]
    public string Subject { get; set; }
    [Required]
    [MaxLength(1800)]
    public string Markdown { get; set; }
    public DateTime CreatedAt { get; set; }
    [Required]
    public MailerType MailerType { get; set; }
    public ICollection<UserMailerMessage> Messages { get; set; }

    public string FormatBody(string link) {
      var engine = new MarkdownSharp.Markdown();
      var html = engine.Transform(this.Markdown.Replace("{LINK}", link));
      return html;
    }
  }
}
