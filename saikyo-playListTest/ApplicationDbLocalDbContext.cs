using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saikyo_playListTest
{
    public class ApplicationDbLocalDbContext : ApplicationDbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder().AddUserSecrets<ApplicationDbFixture>().Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ApplicationDbContextConnection_Test"));

        }
    }
}
