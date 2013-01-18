using System;
using System.ComponentModel;
using System.Linq;

namespace DQF.Platform.Extensions
{
    public static class AttributeExt
    {
        public static T GetAttribute<T>(this object value) where T : Attribute
        {
            if (value == null)
                return null;

            var fi = value.GetType().GetField(value.ToString());
            if (fi == null) return null;
            var attributes = (T[])fi.GetCustomAttributes(typeof(T), false);

            return attributes.SingleOrDefault(x => x.GetType() == typeof(T));
        }

        public static string GetDescription(this object value)
        {
            if (value == null)
                return null;

            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}