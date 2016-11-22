using System;
using System.Reflection;

namespace KV_reloaded
{
    public static class KVSettings
    {
        public static string MakeClassInKV(Type t, string headerKV)
        {
            //KeyValue kvAttributes = (KeyValue)Attribute.GetCustomAttribute(t, typeof(KeyValue));

            var fields = t.GetFields();
            KVToken token = new KVToken(string.IsNullOrWhiteSpace(headerKV) ? "Settings" : headerKV);

            foreach (var f in fields)
            {
                KeyValue kvAtr = (KeyValue)Attribute.GetCustomAttribute(f, typeof(KeyValue));
            }

            return token.ToString();
        }
    }
}