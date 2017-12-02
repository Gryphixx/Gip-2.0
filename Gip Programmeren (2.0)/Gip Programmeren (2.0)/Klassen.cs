using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GIP_Programmeren
{
    class Klassen
    {
        private string _Klassen;
        private int _idKlassen;

        public int intidKlassen
        {
            get { return _idKlassen; }
            set { _idKlassen = value; }
        }

        public string strKlasNaam
        {
            get { return _Klassen; }
            set { _Klassen = value; }
        }

        public void VoegKlassenToeAanListBox(ListBox LitBox)
        {
            LitBox.Items.Add(this);
        }
        public Klassen()
        {

        }
        public Klassen(string _Klassen, int _idKlassen)
        {
            strKlasNaam = _Klassen;
            intidKlassen = _idKlassen;
        }
        public override string ToString()
        {
            return strKlasNaam;
        }

    }
}
