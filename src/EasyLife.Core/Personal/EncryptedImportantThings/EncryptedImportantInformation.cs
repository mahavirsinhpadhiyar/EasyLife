using Abp.Domain.Entities.Auditing;
using EasyLife.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyLife.Personal.EncryptedImportantThings
{
    [Table("EncryptedImportantInformation")]
    public class EncryptedImportantInformation
        : FullAuditedEntity<Guid>
    {
        public string Title { get; set; }
        public string EncryptedText { get; set; }

        #region Foreign Keys
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }
        #endregion Foreign Keys
    }
}
