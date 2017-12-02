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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using GIP_Programmeren;

namespace Gip_Programmeren__2._0_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String _Connection = String.Format("server=84.196.202.210;user id=Dylan;database=arduino;password=Devaien");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpvullenListBoxen(Leerling _objLeerling)
        {
            MySql Aanwezigheid = new MySql(string.Format("SELECT * from aanwezigheidslijst where Leerling_idLeerlingen = {0}", _objLeerling.strIdnummer));
            Aanwezigheid.Select<Leerling>(lstLeerlinglijst,4);
        }

        private void lstAanwezigheidslijst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstAanwezigheidslijst.Items.Clear();
            Leerling objLeerling = (Leerling)lstLeerlinglijst.SelectedItem;
            OpvullenListBoxen(objLeerling);
        }
    }
}
