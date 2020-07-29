using System;
using Microsoft.AspNetCore.Identity;

namespace TeamGo.Identity.Server.Engine.Data.Entities
{
    internal class UserEntity<TKey> : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public bool IsActive { get; set; }
        public DateTime DisactivateDate { get; set; }
    }
}
