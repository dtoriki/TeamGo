using System;
using System.Collections.Generic;
using TeamGo.Shared.Abstracts.DataProviding;

namespace TeamGo.Identity.Server.Engine.DbEntites
{
    internal class UserEntity : IDataEntity, ISoftDeleteable
    {
        public Guid Id { get; set; }

#pragma warning disable CS8618 // Поле, не допускающее значение NULL, не инициализировано. Рекомендуется объявить его как допускающее значение NULL.
        public string Email { get; set; }
        public string PasswordHash { get; set; }
#pragma warning restore CS8618 // Поле, не допускающее значение NULL, не инициализировано. Рекомендуется объявить его как допускающее значение NULL.

        public DateTime CreateTime { get; set; }
        public bool IsSoftDeleted { get; set; }
        public DateTime? DeleteTime { get; set; }

        public ICollection<RoleEntity> Roles { get; set; } = new List<RoleEntity>();
    }
}
