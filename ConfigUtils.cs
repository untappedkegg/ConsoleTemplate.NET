using System.Xml;

namespace ConsoleTemplate
{
    public static class ConfigUtils
    {
        public static string GetConfigValue(string attrName)
        {
            return GetNode(attrName).InnerText;
        }

        public static bool GetConfigBool(string attrName)
        {
            return bool.Parse(GetNode(attrName).InnerText);
        }

        public static string[] GetConfigValues(string attrName)
        {
            var children = GetNode(attrName).ChildNodes;
            string[] retVal = new string[children.Count];

            for (int i = 0; i < children.Count; i++)
            {
                retVal[i] = children[i].InnerText;
            }

            return retVal;
        }

        private static XmlNode GetNode(string attrName)
        {
            var xmlReader = new XmlDocument();
            xmlReader.Load("Config.xml");
            return xmlReader.SelectSingleNode("ConfigOptions").SelectSingleNode(attrName);
        }

        internal static int GetConfigInt(string attrName)
        {
            return int.Parse(GetNode(attrName).InnerText);
        }
    }
}
