using EasyLife.Financial.Investments.SIP.Dto.SIPMaster;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLife.Web.Models.Financial.Investment
{
    public class SIPMasterListViewModel
    {
        public CreateOrEditSIPMasterDto SIPMasterDto { get; set; }
        public List<SelectListItem> UsersList { get; set; }
        public string TotalInvestments { get; set; }
    }
}
