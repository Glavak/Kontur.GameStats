using System.Collections.Generic;

namespace Kontur.GameStats.Server
{
    public static class DictionaryExtension
    {
        public static void IncrementValue(this IDictionary<string, int> dictionary, string key)
        {
            int oldValue;
            if (dictionary.TryGetValue(key, out oldValue))
            {
                dictionary[key] = oldValue + 1;
            }
            else
            {
                dictionary.Add(key, 1);
            }
        }
    }
}
