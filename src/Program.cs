using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace XmlTojson
{
    public class LocaleResource
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            LoadEn();
            LoadTr();
        }

        private static void LoadTr()
        {
            Load("tr", "language_pack.tr-TR.xml");
        }

        private static void LoadEn()
        {
            Load("en", "defaultResources.nopres.xml");
        }

        private static void Load(string languageName, string filePath)
        {
            File.Delete($"../../../outputs/{languageName}.json");

            var doc = new XmlDocument();
            doc.Load(Path.Combine("../../../inputs/", filePath));

            var resources = doc.SelectSingleNode("Language");
            var dynamicList = new List<dynamic>();
            foreach (XmlNode locale in resources.ChildNodes)
            {
                var name = locale.Attributes["Name"].Value;
                var value = locale.ChildNodes[0].FirstChild.Value;
                var x = new ExpandoObject() as IDictionary<string, object>;
                x.Add(name, value);
                dynamicList.Add(x);
            }

            File.WriteAllText($"../../../outputs/{languageName}.json", JsonConvert.SerializeObject(dynamicList, Newtonsoft.Json.Formatting.Indented));

            //string json = JsonConvert.SerializeXmlNode(resources);

            //dynamic dyn = JsonConvert.DeserializeObject<ExpandoObject>(json);
        }
    }
}
