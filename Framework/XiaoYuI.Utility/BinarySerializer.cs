namespace XiaoYuI.Utility
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    public class BinarySerializer
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializedExtendedAttributes"></param>
        /// <returns></returns>
        public static object Deserializer(byte[] serializedExtendedAttributes)
        {
            object o = null;
            Stream stream = new MemoryStream(serializedExtendedAttributes);
            BinaryFormatter formatter = new BinaryFormatter();

            o= formatter.Deserialize(stream);
            stream.Flush();
            stream.Close();
            return o;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Serialize(object value)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            formatter.Serialize(stream, value);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // ���õ�ǰ����λ��Ϊ���Ŀ�ʼ
            stream.Seek(0, SeekOrigin.Begin);
            stream.Flush();
            stream.Close();
            return bytes;
        }
    }
}

