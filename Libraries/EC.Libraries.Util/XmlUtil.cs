using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace EC.Libraries.Util
{
    /// <summary>
    /// �����Xml���л��뷴���л�
    /// </summary>
    public class SerializationUtil
    {
        /// <summary>
        /// ��һ���������л���xml�ַ���
        /// </summary>
        /// <typeparam name="T">���л�������</typeparam>
        /// <param name="item">���л��Ķ���</param>
        /// <returns>�������л����xml��ʽ�ַ���</returns>
        public static string XmlSerialize<T>(T item)
        {
            var serializer = new XmlSerializer(typeof(T));
            var stringBuilder = new StringBuilder();
            using (var writer = new StringWriter(stringBuilder))
            {
                serializer.Serialize(writer, item);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// ��һ���������л���xml�ַ���
        /// </summary>
        /// <param name="item">���л��Ķ���</param>
        /// <returns>�������л����xml��ʽ�ַ���</returns>
        public static string XmlSerialize(object item)
        {
            Type type = item.GetType();
            XmlSerializer serializer = new XmlSerializer(type);
            StringBuilder stringBuilder = new StringBuilder();
            using (StringWriter writer = new StringWriter(stringBuilder))
            {
                serializer.Serialize(writer, item);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// ��xml�����л����ʵ������͵Ķ���
        /// </summary>
        /// <typeparam name="T">�����л������Ͷ���.</typeparam>
        /// <param name="xmlData">�����Xml�ַ���.</param>
        /// <returns>�����л���Ķ���</returns>
        public static T XmlDeserialize<T>(string xmlData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(xmlData))
            {
                T entity = (T)serializer.Deserialize(reader);
                return entity;
            }
        }

        /// <summary>
        /// �����л�
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="filename">�ļ�·��</param>
        public static T XmlDeserialize<T>(string filename,Type type=null)
        {
            type = type ?? typeof (T);
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return (T)serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        /// <summary>
        /// �������л�����
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="filename">�ļ�·��</param>
        public static bool Save(object obj, string filename)
        {
            bool success = false;
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
                success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return success;
        }

        /// <summary>
        /// ���� XML�ַ��� �ڵ�value
        /// </summary>
        /// <param name="xmlDoc">XML��ʽ ����</param>
        /// <param name="xmlNode">�ڵ�</param>
        /// <returns>�ڵ�value</returns>
        public static string GetStrForXmlDoc(string xmlDoc, string xmlNode)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlDoc);
            XmlNode xn = xml.SelectSingleNode(xmlNode);
            return xn.InnerText;
        }
    } 
}
