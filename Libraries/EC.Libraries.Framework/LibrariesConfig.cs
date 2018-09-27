using System;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace EC.Libraries.Framework
{
    /// <summary>
    /// ServiceConfig处理对特定的配置节的访问
    /// </summary>
    public partial class LibrariesConfig : IConfigurationSectionHandler
    {
        /// <summary>
        /// 缓存节点信息
        /// </summary>
        static XmlNode Section{ set; get; }

        /// <summary>
        /// 创建配置节处理程序
        /// </summary>
        /// <param name="parent">父对象.</param>
        /// <param name="configContext">配置上下文对象.</param>
        /// <param name="section">节 XML 节点.</param>
        /// <returns>创建的节处理程序对象.</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            if (section != null)
            {
                Section = section;
            }
            return new LibrariesConfig();

        }

        /// <summary>
        /// 通过xml获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section"></param>
        /// <param name="nodeName"></param>
        T GetObjByXml<T>(XmlNode section, string nodeName) where T : new()
        {
            var config = section.SelectSingleNode(nodeName);
            if (config != null)
            {
                return XmlDeserialize<T>(config.OuterXml);
            }
            return new T();
        }


        /// <summary>
        /// 通过xml获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodeName"></param>
        public T GetObjByXml<T>(string nodeName) where T : new()
        {
            var config = Section.SelectSingleNode(nodeName);
            if (config != null)
            {
                return XmlDeserialize<T>(config.OuterXml);
            }
            return new T();
        }

        /// <summary>
        /// 从xml反序列化到适当的类型的对象
        /// </summary>
        /// <typeparam name="T">反序列化的类型对象.</typeparam>
        /// <param name="xmlData">对象的Xml字符串.</param>
        /// <returns>反序列化后的对象</returns>
        T XmlDeserialize<T>(string xmlData)
        {
            try
            {
                using (TextReader reader = new StringReader(xmlData))
                {
                    return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
