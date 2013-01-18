using System;
using System.Collections;
using System.Collections.Generic;
using Nancy;
using Nancy.Json;

namespace DQF.Platform.Nancy
{
    public class EnumSerializer : JavaScriptConverter
    {
        // TODO: need to handle nullable enums too
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new[] {typeof(Enum)}; }
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            return new DynamicValue(obj.ToString());
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private class DynamicValue : DynamicDictionaryValue, IDictionary<string, object>
        {
            public DynamicValue(object value) : base(value)
            {
            }

            public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(KeyValuePair<string, object> item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(KeyValuePair<string, object> item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public bool Remove(KeyValuePair<string, object> item)
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsReadOnly
            {
                get { throw new NotImplementedException(); }
            }

            public bool ContainsKey(string key)
            {
                throw new NotImplementedException();
            }

            public void Add(string key, object value)
            {
                throw new NotImplementedException();
            }

            public bool Remove(string key)
            {
                throw new NotImplementedException();
            }

            public bool TryGetValue(string key, out object value)
            {
                throw new NotImplementedException();
            }

            public object this[string key]
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public ICollection<string> Keys
            {
                get { throw new NotImplementedException(); }
            }

            public ICollection<object> Values
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}