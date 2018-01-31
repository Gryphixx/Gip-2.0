using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIP_Programmeren;
using MySql.Data.MySqlClient;
using Excel;

namespace Gip_Programmeren__2._0_
{
    class Persistence
    {
        private string _strDatabaseIP;
        private string _strDatabaseName;
        private string _strDatabaseUsername;
        private string _strDatabasePassword;
        private string _strConnectionString;
        
        MySqlConnection msqlConn = new MySqlConnection();

        public string strDatabaseIP
        {
            get { return _strDatabaseIP; }
            set { _strDatabaseIP = value; }
        }

        public string strDatabaseName
        {
            get { return _strDatabaseName; }
            set { _strDatabaseName = value; }
        }

        public string strDatabaseUsername
        {
            get { return _strDatabaseUsername; }
            set { _strDatabaseUsername = value; }
        }

        public string strDatabasePassword
        {
            get { return _strDatabasePassword; }
            set { _strDatabasePassword = value; }
        }

        private string strConnectionString
        {
            get { return _strConnectionString; }
            set { _strConnectionString = value; }
        }


        public Persistence(string dbIP, string dbName, string dbUsername, string dbPasword)
        {
            strDatabaseIP = dbIP;
            strDatabaseName = dbName;
            strDatabaseUsername = dbUsername;
            strDatabasePassword = dbPasword;

            strConnectionString = string.Format("server={};user id={};database={};password={3}", strDatabaseIP, strDatabaseUsername, strDatabaseName, strDatabasePassword);
        }


        private List<Leerling> CreateLeerlingLijstWithAll()
        {
            List<Leerling> lstLeerling = new List<Leerling>();



            return lstLeerling;
        }

        private void GetLeerling()
        {
            msqlConn.Open();
            string _cmd = string.Format("SELECT * from leerling");
            MySqlCommand cmd = new MySqlCommand(_cmd, msqlConn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]), Convert.ToString(dr[10]));
               
            }
            msqlConn.Close();
        }







    }
}
