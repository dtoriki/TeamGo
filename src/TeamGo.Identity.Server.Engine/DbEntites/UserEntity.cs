using System;
using System.Collections.Generic;
using TeamGo.Shared.Abstracts.DataProviding;

namespace TeamGo.Identity.Server.Engine.DbEntites
{
    internal class UserEntity : IDataEntity
    {
        public Guid Id { get; }

#pragma warning disable CS8618 // Поле, не допускающее значение NULL, не инициализировано. Рекомендуется объявить его как допускающее значение NULL.
        public string Email { get; set; }
        public string PasswordHash { get; set; }
#pragma warning restore CS8618 // Поле, не допускающее значение NULL, не инициализировано. Рекомендуется объявить его как допускающее значение NULL.

        public ICollection<RoleEntity> Roles { get; set; } = new List<RoleEntity>();
    }
}
