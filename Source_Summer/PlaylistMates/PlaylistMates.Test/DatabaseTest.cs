using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlaylistMates.Application.Infrastructure;
using Oracle.EntityFrameworkCore;

namespace PlaylistMates.Test
{
    public class DatabaseTest
    {
        protected readonly Context _db;

        public DatabaseTest()
        {
            var opt = new DbContextOptionsBuilder()
                .UseNpgsql("Host=localhost;Database=postgres;Username=pos;Password=pos;")
                .Options;
                /*.UseOracle("User Id=pos;Password=pos;Data Source=localhost:1521/XEPDB1;")
                .Options;*/

            _db = new Context(opt);

        }

    }
}
