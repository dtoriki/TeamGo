using System;
using TeamGo.Shared.Abstracts.DataProviding;

namespace TeamGo.Shared.Tests.Unit.TestClasses
{
    public class TestEntity : IDataEntity
    {
        public Guid Id { get; set; }
        public int IntData { get; set; }
        public string? StringData { get; set; }
    }
}
