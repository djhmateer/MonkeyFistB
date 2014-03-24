using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFist.Models {
    public class UserActivityLog {
        public UserActivityLog() {
            this.CreatedAt = DateTime.Now;
        }
        public int ID { get; set; }
        [MaxLength(255)]
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Entry { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public virtual User User { get; set; }
    }
}
