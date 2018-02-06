using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Gip_Programmeren__2._0_
{
    /// <summary>
    /// Interaction logic for Verwijderen.xaml
    /// </summary>
    public partial class Verwijderen : Window
    {
        public Verwijderen()
        {
            InitializeComponent();
        }

        private void txtVerwijderen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtVerwijderen.Text == "Verwijderen")
                {
                    string _conn = string.Format("server=84.196.202.210;user id=Dylan;database=arduino;password={0}", "Devaien");
                    MySqlConnection conn = new MySqlConnection(_conn);
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM arduino.leerling", conn);
                    MessageBox.Show("Verwijderen gelukt.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("De ingegeven string komt niet overeen met &quot;Verwijderen&quot;");
                }
            }
        }
    }
}
