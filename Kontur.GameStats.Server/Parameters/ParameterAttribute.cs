using System;

namespace Kontur.GameStats.Server
{
    /// <summary>
    /// Attribute should be added to properties in IParameters class,
    /// to mark them as parameters, that should be parsed from request
    /// url, and to specify how to parse them
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ParameterAttribute : Attribute
    {
        public abstract object ParseFromString(string s);
    }
}
