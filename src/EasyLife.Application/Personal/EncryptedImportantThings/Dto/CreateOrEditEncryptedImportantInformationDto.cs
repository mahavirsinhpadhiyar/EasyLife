using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLife.Personal.EncryptedImportantThings.Dto
{
    [AutoMapFrom(typeof(Personal.EncryptedImportantThings.EncryptedImportantInformation))]
    public class CreateOrEditEncryptedImportantInformationDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public string EncryptedText { get; set; }
        public long UserId { get; set; }
    }
}
