using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeamGo.Identity.Server.Engine.Data.Entities;

namespace TeamGo.Identity.Server.Engine.Data
{
    internal class UserStoreContext : IdentityDbContext<UserEntity<Guid>, IdentityRole<Guid>, Guid>
    {
        public UserStoreContext(DbContextOptions<UserStoreContext> options) : base (options)
        {

        }
    }
}
