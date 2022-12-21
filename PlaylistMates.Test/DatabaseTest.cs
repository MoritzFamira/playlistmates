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
    public class DatabaseTest : IDisposable
    {
        protected readonly Context _db;

        public DatabaseTest()
        {
            var opt = new DbContextOptionsBuilder()
                .UseOracle(@"User Id=pos;Password=pos;Data Source=MyOracleDb;")
                .Options;

            _db = new Context(opt);
        }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
