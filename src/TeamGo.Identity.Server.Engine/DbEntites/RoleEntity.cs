using System;
using TeamGo.Shared.Abstracts.DataProviding;

namespace TeamGo.Identity.Server.Engine.DbEntites
{
    internal class RoleEntity : IDataEntity
    {
        public Guid Id { get; set;  }

#pragma warning disable CS8618 // Поле, не допускающее значение NULL, не инициализировано. Рекомендуется объявить его как допускающее значение NULL.
        public string Role { get; set; }
#pragma warning restore CS8618 // Поле, не допускающее значение NULL, не инициализировано. Рекомендуется объявить его как допускающее значение NULL.

        public Guid? UserId { get; set; }
        public UserEntity? User { get; set; }
    }
}
