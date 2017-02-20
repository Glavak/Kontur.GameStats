using System;

namespace Kontur.GameStats.Server
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ParameterAttribute : Attribute
    {
        public abstract object ParseFromString(string s);
    }
}
