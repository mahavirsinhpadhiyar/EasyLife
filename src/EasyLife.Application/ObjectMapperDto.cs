using AutoMapper;
using EasyLife.Financial.Expenses;
using EasyLife.Financial.Expenses.Dto;

namespace EasyLife
{
    public class ObjectMapperDto : Profile
    {
        public ObjectMapperDto()
        {
            CreateMap<CreateOrEditExpensesDto, Expenses>();
        }
    }
}
