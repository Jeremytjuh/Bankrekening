using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankrekening.classes
{
    class Spaarrekening : Rekening
    {
        public double Rente { get; set; }
        public double Bedrag { get; set; }
        public double Boete { get; set; }
    }
}
