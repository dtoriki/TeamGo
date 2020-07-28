using System;

namespace TeamGo.Shared.Abstracts.DataProviding
{
    public interface ISoftDeleteable
    {
        public bool IsSoftDeleted { get; set; }
        DateTime? DeleteTime { get; set; }
    }
}
