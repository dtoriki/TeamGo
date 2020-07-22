using System;

namespace TeamGo.Shared.Abstracts
{
    public static class Extensions
    {
        public static T Clone<T>(this ICloneable obj)
        {
            return (T)obj.Clone();
        }
    }
}
