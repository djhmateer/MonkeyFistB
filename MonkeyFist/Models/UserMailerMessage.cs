using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFist.Models {
  public class UserMailerMessage {
    public UserMailerMessage() {
      this.CreatedAt = DateTime.Now;
    }

    public int ID { get; set; }
    public virtual UserMailerTemplate Mailer { get; set; }
    [Required]
    [MaxLength(255)]
    public string Subject { get; set; }
    [Required]
    public string Body { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    [Required]
    public bool Successful { get; set; }
    public string ResultMessage { get; set; }

    public static UserMailerMessage CreateFromTemplate(UserMailerTemplate template, string link) {
      var message = new UserMailerMessage();
      message.Subject = template.Subject;
      message.Body = template.FormatBody(link);
      message.Mailer = template;
      return message;
    }

    public UserMailerMessage SendTo(User user) {

      var message = new MailMessage();

      message.To.Add(new MailAddress(user.Email));
      message.Subject = this.Subject;
      message.Body = this.Body;
      message.IsBodyHtml = true;
      this.Successful = false;

      SmtpClient client = new SmtpClient();
      try {
        client.Send(message);
        this.Successful = true;
      } catch (SmtpFailedRecipientException x) {
        this.ResultMessage = x.Message;
        user.Status = UserStatus.InvalidEmail;
      } catch (SmtpException x) {
        this.ResultMessage = x.Message;
      }
      //drop this into the user's mailers
      user.MailerLogs.Add(this);

      return this;
    }
  }
}
