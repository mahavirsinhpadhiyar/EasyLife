using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace EasyLife.EntityFrameworkCore
{
    public static class EasyLifeDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<EasyLifeDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<EasyLifeDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
