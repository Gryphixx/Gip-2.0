using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace GIP_Programmeren
{
    class Klas
    {
        private DateTime _tijdstip;
        private String _richting;
        private int _jaar;

        public DateTime dtTijdstip
        {
            get { return _tijdstip; }
            set { _tijdstip = value; }
        }

        public String strRichting
        {
            get { return _richting; }
            set { _richting = value; }
        }

        public int intJaar
        {
            get { return _jaar; }
            set { _jaar = value; }
        }

        public override string ToString()
        {
            return _richting;
        }

        public Klas(String richting, int jaar, DateTime tijdstip)
        {
            _richting = richting;
            _tijdstip = tijdstip;
            _jaar = jaar;
        }

        public Klas()
        {

        }
    }
}
