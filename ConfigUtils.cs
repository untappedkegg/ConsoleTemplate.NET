using ConsoleTemplate.Database;
using System.Xml;

namespace ConsoleTemplate
{
    public static class ConfigUtils
    {
        public static string GetConfigValue(string tagName)
        {
            return GetNode(tagName).InnerText;
        }

        public static string GetConfigValue(string outerTag, string childTag)
        {
            return GetNode(outerTag).SelectSingleNode(childTag).InnerText;
        }

        private static string GetInnerNodeText(XmlNode node, string childTag)
        {
            return node.SelectSingleNode(childTag)?.InnerText ?? string.Empty;
        }

        public static bool GetConfigBool(string tagName)
        {
            return bool.Parse(GetNode(tagName).InnerText);
        }

        public static bool GetConfigBool(string outerTag, string childTag)
        {
            return bool.Parse(GetNode(outerTag).SelectSingleNode(childTag).InnerText);
        }

        public static string[] GetConfigValues(string outerTag, string childTag)
        {
            var children = GetNode(outerTag).SelectNodes(childTag);
            string[] retVal = new string[children.Count];

            for (int i = 0; i < children.Count; i++)
            {
                retVal[i] = children[i].InnerText;
            }

            return retVal;
        }

        private static XmlNode GetNode(string tagName, string? rootNode = "ConfigOptions")
        {
            var xmlReader = new XmlDocument();
            xmlReader.Load("Config.xml");
            if (string.IsNullOrEmpty(rootNode))
            {
                return xmlReader.SelectSingleNode(tagName);
            }
            // else go deeper
            return xmlReader.SelectSingleNode(rootNode).SelectSingleNode(tagName);
        }

        internal static int GetConfigInt(string tagName)
        {
            return int.Parse(GetNode(tagName).InnerText);
        }

        internal static int GetConfigInt(string outerTag, string childTag)
        {
            return int.Parse(GetNode(outerTag).SelectSingleNode(childTag).InnerText);
        }

        internal static T ParseConnectionSettings<T>(string configTag = "ConfigOptions") where T : DbConnectionSettings, new()
        {
            var configNode = GetNode(configTag);

            return new T
            {
                Database = GetInnerNodeText(configNode, "database"),
                UserName = GetInnerNodeText(configNode, "user"),
                Password = GetInnerNodeText(configNode, "password"),
                Server = GetInnerNodeText(configNode, "server"),
                Schema = GetInnerNodeText(configNode, "schema"),
                Table = GetInnerNodeText(configNode, "tableName")
            };
        }
    }
}