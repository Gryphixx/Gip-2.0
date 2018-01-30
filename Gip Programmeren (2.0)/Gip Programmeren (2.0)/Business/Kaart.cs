using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIP_Programmeren
{
    class Kaart
    {
        private String _SerialNumber;
        private String _StamboekNummer;

        public String SerialNumber
        {
            get { return _SerialNumber; }
            set { _SerialNumber = value; }
        }

        public String StamboekNummer
        {
            get { return _StamboekNummer; }
            set { _StamboekNummer = value; }
        }

        public Kaart(string SerialNumber, string Stamboeknummer)
        {
            _SerialNumber = SerialNumber;
            _StamboekNummer = Stamboeknummer;
        }

        public Kaart()
        {

        }

    }
}
