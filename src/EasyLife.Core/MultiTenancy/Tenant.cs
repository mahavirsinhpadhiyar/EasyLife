using Abp.MultiTenancy;
using EasyLife.Authorization.Users;

namespace EasyLife.MultiTenancy
{
    public class  
        Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
