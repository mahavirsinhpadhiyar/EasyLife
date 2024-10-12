using EasyLife.Financial.Investments.ShareMarket.Dto.ShareOrders;
using System;

namespace EasyLife.Web.Models.Financial.Investment
{
    public class ShareOrdersListViewModel
    {
        public CreateOrEditShareOrdersDto ShareOrdersDto { get; set; }
        public Guid ShareMasterId { get; set; }
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
        public string ShareMasterName { get; set; }
    }
}
