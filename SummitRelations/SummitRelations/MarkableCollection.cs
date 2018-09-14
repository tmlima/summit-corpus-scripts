using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SummitRelations
{
    [Serializable]
    [XmlRoot("markables")]
    public class MarkableCollection
    {
        [XmlElement("markable")]
        public Markable[] Markables { get; set; }
    }
}
