using Abp.Application.Services.Dto;
using System;

namespace EasyLife.Personal.EncryptedImportantThings.Dto
{
    public class PagedEncryptedImportantInformationResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public Guid? CategoryId { get; set; }
        public long? FilterUserId { get; set; }
    }
}
