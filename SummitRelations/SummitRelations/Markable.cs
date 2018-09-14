using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SummitRelations
{
    [Serializable]
    public class Markable
    {
        public string Text { get; set; }
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("span")]
        public string Span { get; set; }
        [XmlAttribute("status")]
        public string Status { get; set; }
        [XmlAttribute("np_n")]
        public string NpN { get; set; }
        [XmlAttribute("np_form")]
        public string NpForm { get; set; }
        [XmlAttribute("is_anaphoric")]
        public string IsAnaphoric { get; set; }
        [XmlAttribute("pointer")]
        public string Pointer { get; set; }
        [XmlAttribute("is_bridging")]
        public string IsBridging { get; set; }
        [XmlAttribute("member")]
        public string Member { get; set; }

        [XmlIgnore]
        public int FirstWordIndex
        {
            get
            {
                string firstWord;
                if (Span.Contains(".."))
                    firstWord = Span.Split("..".ToCharArray())[0];
                else
                    firstWord = Span;

                return Convert.ToInt32(RemoveWordSufix(firstWord));
            }
        }

        [XmlIgnore]
        public List<string> SpanWordsId
        {
            get
            {
                List<string> ids = new List<string>();

                if (Span.Contains(".."))
                {
                    int firstWordIndex = this.FirstWordIndex;
                    int lastWordIndex = Convert.ToInt32(RemoveWordSufix(Span.Split("..".ToCharArray())[2]));
                    for (int index=firstWordIndex; index<=lastWordIndex; index++)
                    {
                        ids.Add("word_" + index);
                    }
                }
                else
                {
                    ids.Add(Span);
                }

                return ids;
            }
        }

        private string RemoveWordSufix(string word)
        {
            return word.Replace("word_", "");
        }
    }
}
