using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Controls;

namespace Gip_Programmeren__2._0_
{
    class MySql
    {
        private String _conn = string.Format("server=84.196.202.210;user id=Dylan;database=arduino;password=Devaien");
        private String _query;

        public String query
        {
            get { return _query; }
            set { _query = value; }
        }

        public void Select<T>(ListBox lstbox, int Kolommen)
        {
            string[] info = new string[Kolommen];
            MySqlConnection conn = new MySqlConnection(_conn);
            conn.Open();
            string _cmd = string.Format(_query);
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            var objKlasse = Activator.CreateInstance(typeof(T), new object[] { info });
            while (dr.Read())
            {
                for (int i = 0; i < Kolommen; i++)
                {
                    info[i] = (dr[Kolommen].ToString());
                }  
                lstbox.Items.Add(objKlasse);
            }
            conn.Close();
        }

        public void Update()
        {
            MySqlConnection conn = new MySqlConnection(_conn);
            conn.Open();
            string _cmd = string.Format(_query);
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public MySql(String Query)
        {
            _query = Query;
        }
    }
}
