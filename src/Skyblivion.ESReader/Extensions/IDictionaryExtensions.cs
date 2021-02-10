using System;
using System.Collections.Generic;

namespace Skyblivion.ESReader.Extensions
{
    public static class IDictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> addValueFactory, out bool containedKey)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                containedKey = true;
                return value;
            }
            containedKey = false;
            value = addValueFactory();
            dictionary.Add(key, value);
            return value;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> addValueFactory)
        {
            return dictionary.GetOrAdd(key, addValueFactory, out _);
        }

        public static TValue? GetWithFallbackNullable<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue?> fallbackValue) where TValue : class
        {
            TValue? value;
            return dictionary.TryGetValue(key, out value) ? value : fallbackValue();
        }
    }
}