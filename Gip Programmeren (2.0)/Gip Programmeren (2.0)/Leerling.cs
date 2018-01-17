using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace GIP_Programmeren
{
    class Leerling
    {
        private string _idnummer;
        private int _klasnummer;
        private string _voornaam;
        private string _achternaam;
        private string _password;
        private bool _maandag;
        private bool _dinsdag;
        private bool _donderdag;
        private bool _vrijdag;
        private string _klas;

        public string strIdnummer
        {
            get { return _idnummer; }
            set { _idnummer = value; }
        }

        public int intKlasnummer
        {
            get { return _klasnummer; }
            set { _klasnummer = value; }
        }

        public string strVoornaam
        {
            get { return _voornaam; }
            set { _voornaam = value; }
        }

        public string strAchternaam
        {
            get { return _achternaam; }
            set { _achternaam = value; }
        }

        public string strPassword
        {
            get { return _password; }
            set { _password = value; }
        }

        public bool blMaandag
        {
            get { return _maandag; }
            set { _maandag = value; }
        }

        public bool blDinsdag
        {
            get { return _dinsdag; }
            set { _dinsdag = value; }
        }

        public bool blDonderdag
        {
            get { return _donderdag; }
            set { _donderdag = value; }
        }

        public bool blVrijdag
        {
            get { return _vrijdag; }
            set { _vrijdag = value; }
        }
        public string strKlas
        {
            get { return _klas; }
            set { _klas = value; }
        }

        public override string ToString()
        {
            return strAchternaam + " " + strVoornaam;
        }

        public Leerling(string _strPW)
        {
            strPassword = _strPW;
        }

        public Leerling(string _idLeerling, string _Voornaam, string _Achternaam, int _Klasnummer, bool _Maandag, bool _Dinsdag, bool _Donderdag, bool _Vrijdag, string _Klas)
        {
            strIdnummer = _idLeerling;
            strVoornaam = _Voornaam;
            strAchternaam = _Achternaam;
            intKlasnummer = _Klasnummer;
            blMaandag = _Maandag;
            blDinsdag = _Dinsdag;
            blDonderdag = _Donderdag;
            blVrijdag = _Vrijdag;
            strKlas = _Klas;

        }
        public Leerling(string _Voornaam, string _Achternaam)
        {
            strVoornaam = _Voornaam;
            strAchternaam = _Achternaam;
        }
        public Leerling(string _Voornaam, string _Achternaam, string _Klas, int _Klasnummer)
        {
            strVoornaam = _Voornaam;
            strAchternaam = _Achternaam;
            strKlas = _Klas;
            intKlasnummer = _klasnummer;
        }


    }
}
