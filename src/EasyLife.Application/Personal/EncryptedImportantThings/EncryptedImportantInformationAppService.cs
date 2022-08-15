using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using EasyLife.Authorization;
using EasyLife.Authorization.Users;
using EasyLife.Personal.EncryptedImportantThings.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyLife.Helper;

namespace EasyLife.Personal.EncryptedImportantThings
{
    /// <summary>
    /// Manages personal important information
    /// </summary>
    [AbpAuthorize(PermissionNames.Pages_Personal_EncryptedImportantInformation)]
    public class EncryptedImportantInformationAppService : AsyncCrudAppService<Personal.EncryptedImportantThings.EncryptedImportantInformation, CreateOrEditEncryptedImportantInformationDto, Guid, PagedEncryptedImportantInformationResultRequestDto, CreateOrEditEncryptedImportantInformationDto, CreateOrEditEncryptedImportantInformationDto>, IEncryptedImportantInformationAppService
    {
        private readonly IRepository<Personal.EncryptedImportantThings.EncryptedImportantInformation, Guid> _encryptedImportantInformationRepository;
        private readonly UserManager _userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="encryptedImportantInformationRepository"></param>
        /// <param name="userManager"></param>
        public EncryptedImportantInformationAppService(IRepository<Personal.EncryptedImportantThings.EncryptedImportantInformation, Guid> encryptedImportantInformationRepository, UserManager userManager) : base(encryptedImportantInformationRepository)
        {
            _encryptedImportantInformationRepository = encryptedImportantInformationRepository;
            _userManager = userManager;
        }
        /// <summary>
        /// Will return the list of paging wise encrypted important information with filtered result
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<CreateOrEditEncryptedImportantInformationDto>> GetAllFiltered(PagedEncryptedImportantInformationResultRequestDto input)
        {
            var encryptedImportantInformationList = new List<Personal.EncryptedImportantThings.EncryptedImportantInformation>();
            if (input.FilterUserId == null || input.FilterUserId == 0)
                input.FilterUserId = AbpSession.UserId;
            var query = _encryptedImportantInformationRepository.GetAll().Where(x => x.UserId == input.FilterUserId);

            query = ApplySorting(query, input);

            encryptedImportantInformationList = query
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Title.Contains(input.Keyword))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderByDescending(e => e.CreationTime)
                .ToList();

            var pageCount = query
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Title.Contains(input.Keyword))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderByDescending(e => e.CreationTime)
                .ToList()
                .Count();

            var encryptedImportantInformationFilterList = encryptedImportantInformationList.Select(r => new CreateOrEditEncryptedImportantInformationDto()
            {
                Id = r.Id,
                EncryptedText = r.EncryptedText,
                Title = r.Title,
                UserId = r.UserId
            }).ToList();

            var result = new PagedResultDto<CreateOrEditEncryptedImportantInformationDto>(query.Count(), encryptedImportantInformationFilterList);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Overridden ApplySorting
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<Personal.EncryptedImportantThings.EncryptedImportantInformation> ApplySorting(IQueryable<Personal.EncryptedImportantThings.EncryptedImportantInformation> query, PagedEncryptedImportantInformationResultRequestDto input)
        {
            if (input.Sorting.IsNullOrEmpty() || input.Sorting == "0 asc")
            {
                input.Sorting = "CreationTime desc";
            }
            return base.ApplySorting(query, input);
        }

        /// <summary>
        /// Create encrypted important information entry
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<CreateOrEditEncryptedImportantInformationDto> CreateAsync(CreateOrEditEncryptedImportantInformationDto input)
        {
            try
            {
                input.UserId = AbpSession.UserId.Value;
                input.EncryptedText = EncryptDecryptString.EncryptString(input.EncryptedText);
                var encryptedImportantInformation = ObjectMapper.Map<Personal.EncryptedImportantThings.EncryptedImportantInformation>(input);
                await _encryptedImportantInformationRepository.InsertAsync(encryptedImportantInformation);
                return MapToEntityDto(encryptedImportantInformation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update encrypted important information
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<CreateOrEditEncryptedImportantInformationDto> UpdateAsync(CreateOrEditEncryptedImportantInformationDto input)
        {
            try
            {
                input.UserId = AbpSession.UserId.Value;
                input.EncryptedText = EncryptDecryptString.EncryptString(input.EncryptedText);
                var encryptedImportantInformation = await _encryptedImportantInformationRepository.GetAsync(input.Id);
                ObjectMapper.Map(input, encryptedImportantInformation);
                await _encryptedImportantInformationRepository.UpdateAsync(encryptedImportantInformation);
                return MapToEntityDto(encryptedImportantInformation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Edit personal important information
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CreateOrEditEncryptedImportantInformationDto> GetEncryptedImportantInformationForEdit(EntityDto<Guid> input)
        {
            var output = await _encryptedImportantInformationRepository.FirstOrDefaultAsync(s => s.Id == input.Id);
            var ClassObj = ObjectMapper.Map<CreateOrEditEncryptedImportantInformationDto>(output);
            ClassObj.EncryptedText = EncryptDecryptString.DecryptString(ClassObj.EncryptedText);
            return ClassObj;
        }
    }
}
