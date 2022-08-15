using EasyLife.Financial.Investments.ShareMarket.Dto.ShareOrders;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

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
