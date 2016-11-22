using System;

namespace KV_reloaded
{
    [AttributeUsage(AttributeTargets.Method)]
    public class KeyValue : Attribute
    {
        public string place { get; set; }
        public KeyValue(string place)
        {
            this.place = place;
        }
    }
}