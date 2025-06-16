using System.Xml.Serialization;

namespace Ukol_v1.Models
{
    [XmlRoot("Automobily")]
    public class Automobily
    {
        [XmlElement("Auto")]
        public List<Auto> Auta { get; set; }

        public List<AutoSummary> GetSummaries()
        {
            return [.. Auta
                .GroupBy(a => a.NazevModelu)
                .Select(g => new AutoSummary
                {
                    NazevModelu = g.Key,
                    CelkovaCena = g.Sum(a => a.Cena),
                    CelkovaCenaSDPH = g.Sum(a => a.CenaSDPH),
                    Pocet = g.Count()
                })
                .OrderBy(s => s.NazevModelu)];
        }
    }

}
