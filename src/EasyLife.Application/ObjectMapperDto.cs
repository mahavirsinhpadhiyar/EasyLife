using AutoMapper;
using EasyLife.Financial.Earning;
using EasyLife.Financial.Earning.Dto;
using EasyLife.Financial.Expenses;
using EasyLife.Financial.Expenses.Dto;
using EasyLife.Financial.Investment;
using EasyLife.Financial.Investments.ShareMarket.Dto.ShareOrders;
using EasyLife.Financial.Investments.ShareMarket.ShareMaster.Dto;
using EasyLife.Personal.EncryptedImportantThings;
using EasyLife.Personal.EncryptedImportantThings.Dto;

namespace EasyLife
{
    public class ObjectMapperDto : Profile
    {
        public ObjectMapperDto()
        {
            CreateMap<CreateOrEditExpensesDto, Expenses>();
            CreateMap<CreateOrEditEarningDto, Earnings>();
            CreateMap<CreateOrEditEncryptedImportantInformationDto, EncryptedImportantInformation>();
            CreateMap<CreateOrEditShareMasterDto, EL_Financial_Investment_Share_Master>();
            CreateMap<CreateOrEditShareOrdersDto, EL_Financial_Investment_Share_Orders>();
        }
    }
}
