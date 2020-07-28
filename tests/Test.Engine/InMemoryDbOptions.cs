using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Test.Engine
{
    internal static class InMemoryDbOptions
    {
        public static DbContextOptions<T> GetDBInMemoryOptions<T>()
            where T: DbContext
        {
            string rnd = "db_" + Guid
                .NewGuid()
                .ToString();

            return new DbContextOptionsBuilder<T>()
                .UseInMemoryDatabase(rnd)
                .Options;
        }
    }
}
