using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace templater.contracts
{
    public static class ObjectToDictionaryHelper
    {
        public static IDictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        public static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                throw new ArgumentNullException("Unable to convert object to a dictionary. The source object is null.");

            if (source is IDictionary<string, T> result)
                return result;

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
                if (!dictionary.ContainsKey(property.Name))
                {
                    object value = property.GetValue(source);
                    if (value is T t)
                        dictionary.Add(property.Name, t);
                }
            return dictionary;
        }
    }
}