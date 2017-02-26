using System.Collections.Generic;

namespace Kontur.GameStats.Server
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// Increments integer value of entry with given key, or creates new
        /// entry with value of 1, if key not exists in dictionary
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
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
