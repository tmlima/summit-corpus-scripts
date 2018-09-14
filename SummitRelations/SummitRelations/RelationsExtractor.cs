using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace SummitRelations
{
    public class RelationsExtractor
    {
        private string textsFolderPath;

        public RelationsExtractor(string textsFolderPath)
        {
            this.textsFolderPath = textsFolderPath;
        }

        public List<Markable> GetMarkables()
        {
            List<Markable> markables = new List<Markable>();
            string[] textsFolder = Directory.GetDirectories(textsFolderPath);
            foreach (string textFolder in textsFolder)
            {
                markables.AddRange(GetTextMarkables(textFolder));
            }

            return markables;
        }

        public List<Pos> GetPos()
        {
            List<Pos> pos = new List<Pos>();
            string[] textsFolder = Directory.GetDirectories(textsFolderPath);
            foreach (string textFolder in textsFolder)
            {
                pos.AddRange(GetTextPos(textFolder));
            }


            return pos;
        }

        private List<Markable> GetTextMarkables(string textFolderPath)
        {
            string textName = Path.GetFileName(textFolderPath);
            string markablesXmlPath = Directory.GetFiles(textFolderPath).Where(x => Path.GetFileName(x) == textName + ".txt.markables.xml").Single();
            return GetMarkablesFromFile(textName, markablesXmlPath);
        }

        private List<Markable> GetMarkablesFromFile(string textName, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MarkableCollection));
            StreamReader reader = new StreamReader(filePath);
            List<Markable> markables = ((MarkableCollection)serializer.Deserialize(reader)).Markables.ToList();
            markables.ForEach(x => x.Text = textName);
            reader.Close();
            return markables;
        }

        private List<Pos> GetTextPos(string textFolderPath)
        {
            List<Pos> posList = new List<Pos>();
            string textName = Path.GetFileName(textFolderPath);
            string posXmlPath = Directory.GetFiles(textFolderPath).Where(x => Path.GetFileName(x) == textName + ".txt.pos.xml").Single();
            XmlDocument document = new XmlDocument();
            document.Load(posXmlPath);
            foreach (XmlElement element in document.GetElementsByTagName("word"))
            {
                Pos pos = new Pos(element.GetAttributeNode("id").Value, textName);

                XmlNode numberNode = element.SelectSingleNode("./*[@number]");
                if (numberNode != null)
                {
                    string numberNodeValue = numberNode.Attributes["number"].Value;
                    if (!string.IsNullOrEmpty(numberNodeValue))
                        pos.Number = Convert.ToChar(numberNodeValue);
                }

                XmlNode canonNode = element.SelectSingleNode("./*[@canon]");
                if (canonNode != null)
                    pos.Canon = canonNode.Attributes["canon"].Value;
                posList.Add(pos);
            }
            return posList;
        }
    }
}
