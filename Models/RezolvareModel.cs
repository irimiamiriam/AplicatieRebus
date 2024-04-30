using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicatieRebus.Models
{
    public class RezolvareModel
    {
        public int ColoanaStart { get; set; }
        public int LinieStart { get; set; }
        public string Orientare { get; set; }
        public string Solutie { get; set; }
        public string Definitie { get; set; }
        public int NrVocale { get; set; }
    }
}
