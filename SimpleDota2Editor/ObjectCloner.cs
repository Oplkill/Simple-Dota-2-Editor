using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleDota2Editor
{
    public static class ObjectCloner
    {
        public static T DeepClone<T>(this T source) where T : class
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, source);
            stream.Position = 0;
            return (T)formatter.Deserialize(stream);
        }
    }
}