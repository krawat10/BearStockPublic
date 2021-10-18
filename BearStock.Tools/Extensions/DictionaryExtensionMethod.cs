using System.Collections.Generic;
using System.Linq;

namespace BearStock.Tools.Extensions
{
    public static class DictionaryExtensionMethod
    {
        public static T GetValueByKeyContains<T>(this Dictionary<string, T> dictionary, string keyword, T defaultValue = default)
        {
            foreach (var keyValuePair in dictionary)
                if (keyValuePair.Key.Contains(keyword))
                    return keyValuePair.Value;

            return default;
        }
    }
}