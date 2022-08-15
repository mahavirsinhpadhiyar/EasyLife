using EasyLife.Financial.Investments.ShareMarket.ShareMaster.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLife.Web.Models.Financial.Investment
{
    public class ShareMasterListViewModel
    {
        public CreateOrEditShareMasterDto ShareMasterDto { get; set; }
        public List<SelectListItem> UsersList { get; set; }
        public string TotalInvestments { get; set; }
    }
}
