using Abp.Domain.Entities.Auditing;
using EasyLife.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLife.Personal.EncryptedImportantThings
{
    [Table("EncryptedImportantInformation")]
    public class EncryptedImportantInformation : FullAuditedEntity<Guid>
    {
        public string Title { get; set; }
        public string EncryptedText { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
