using System;
using TeamGo.Shared.Abstracts.DataProviding;

namespace Test.Engine
{
    internal class TestEntity : IDataEntity, ISoftDeleteable
    {
        public Guid Id { get; set; }
        public int IntData { get; set; }
        public string? StringData { get; set; }
        public bool IsSoftDeleted { get; set; }
        public DateTime? DeleteTime { get; set; }
    }
}
