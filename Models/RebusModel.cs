using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicatieRebus.Models
{
    public class RebusModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NrColoane { get; set; }
        public int NrLinii { get; set; }
        public int NrSecunde { get; set; }

    }
}
