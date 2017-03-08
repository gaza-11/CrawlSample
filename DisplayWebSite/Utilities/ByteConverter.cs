using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace DisplayWebSite.Utilities
{
    public static class ByteConverter
    {
        public static object ByteArrayToObject(byte[] arrBytes)
        {
            using (var ms = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                ms.Write(arrBytes, 0, arrBytes.Length);
                ms.Seek(0, SeekOrigin.Begin);

                return (Object)binForm.Deserialize(ms);
            }
        }

        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);

                return ms.ToArray();
            }
        }
    }
}