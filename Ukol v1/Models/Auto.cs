using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ukol_v1.Models
{
    public class Auto
    {
        [XmlElement("NazevModelu")]
        public string? NazevModelu { get; set; }

        [XmlElement("DatumProdeje")]
        public DateTime DatumProdeje { get; set; }
        
        [XmlElement("Cena")]
        public double Cena { get; set; }

        [XmlElement("DPH")]
        public double DPH { get; set; }

        [XmlIgnore]
        public double CenaSDPH
        {
            get
            {
                return Cena * (1 + DPH / 100);
            }
        }
    }
}
