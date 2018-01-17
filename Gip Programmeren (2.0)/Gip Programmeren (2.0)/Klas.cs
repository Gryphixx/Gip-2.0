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
        private TimeSpan _tijdstip;
        private String _richting;
        private int _jaar;
        private int _id;

        public TimeSpan dtTijdstip
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
        public int intId
        {
            get { return _id; }
            set { _id = value; }
        }

        public override string ToString()
        {
            return _richting;
        }

        public Klas(String richting, TimeSpan tijdstip, int id)
        {
           strRichting = richting;
           dtTijdstip = tijdstip;
            intJaar = Convert.ToInt16(richting.Substring(0, 1));
            intId = id;
        }

        public Klas()
        {

        }
    }
}
