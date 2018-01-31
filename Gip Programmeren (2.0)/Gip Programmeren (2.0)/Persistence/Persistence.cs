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

        private List<Aanwezigheid> FillAanwezigheidLijst()
        {
            List<Aanwezigheid> lstAanwezigheid = new List<Aanwezigheid>();

            msqlConn.Open();
            string msqlCmd = string.Format("SELECT * from aanwezigheidslijst");
            MySqlCommand _msqlCmd = new MySqlCommand(msqlCmd, msqlConn);
            MySqlDataReader DataReader = _msqlCmd.ExecuteReader();
            while (DataReader.Read())
            {
                Aanwezigheid objAanwezigheid = new Aanwezigheid(DataReader[0].ToString(), DataReader[1].ToString(), DataReader[2].ToString(), DataReader[3].ToString());
                lstAanwezigheid.Add(objAanwezigheid);
            }
            msqlConn.Close();

            return lstAanwezigheid;
        }







    }
}
