using Abp.Application.Services;
using Abp.Application.Services.Dto;
using EasyLife.Personal.EncryptedImportantThings.Dto;
using System;
using System.Threading.Tasks;

namespace EasyLife.Personal.EncryptedImportantThings
{
    public interface IEncryptedImportantInformationAppService : IAsyncCrudAppService<CreateOrEditEncryptedImportantInformationDto, Guid, PagedEncryptedImportantInformationResultRequestDto, CreateOrEditEncryptedImportantInformationDto, CreateOrEditEncryptedImportantInformationDto>
    {
        /// <summary>
        /// Edit information
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CreateOrEditEncryptedImportantInformationDto> GetEncryptedImportantInformationForEdit(EntityDto<Guid> input);
    }
}
