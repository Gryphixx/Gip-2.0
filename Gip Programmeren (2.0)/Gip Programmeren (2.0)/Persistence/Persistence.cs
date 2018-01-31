using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIP_Programmeren;
using MySql.Data.MySqlClient;

namespace Gip_Programmeren__2._0_
{
    class Persistence
    {
        private string strDatabaseIP;
        private string strDatabaseName;
        private string strDatabaseUsername;
        private string strDatabasePassword;
        private string strConnectionString;
        

        public Persistence()
        {
            
        }

        private List<Leerling> FillLeerlingLijstWithAll()
        {
            List<Leerling> lstLeerling = new List<Leerling>();



            return lstLeerling;
        }

        private void GetLeerling()
        {

        }

        private List<Aanwezigheid> CreateAanwezigheidLijst()
        {
            List<Aanwezigheid> lstAanwezigheid = new List<Aanwezigheid>();

            msqlConn.Open();
            string _cmd = string.Format("SELECT * from aanwezigheidslijst");
            MySqlCommand cmd = new MySqlCommand(_cmd, msqlConn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Aanwezigheid objAanwezigheid = new Aanwezigheid(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString());
                lstAanwezigheid.Add(objAanwezigheid);
            }
            msqlConn.Close();

            return lstAanwezigheid;
        }

        private List<Klas> CreateKlassenLijst()
        {
            List<Klas> lstKlas = new List<Klas>();

            msqlConn.Open();
            string _cmd = String.Format("SELECT * FROM arduino.klassen;");
            MySqlCommand cmd = new MySqlCommand(_cmd, msqlConn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Klas objKlas = new Klas(dr[1].ToString(), (TimeSpan)dr[2], (int)dr[0]);
                lstKlas.Add(objKlas);
            }
            msqlConn.Close();

            return lstKlas;
        }





    }
}
