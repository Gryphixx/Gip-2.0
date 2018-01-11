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
        static string _conn = string.Format("server=84.196.202.210;user id=Denzel;database=arduino;password={0}", "Denzel");
        static MySqlConnection conn = new MySqlConnection(_conn);
        public MainWindow()
        {
            InitializeComponent();
            OpvullenLeerlingLijst();
            OpvullenDagInstelling();
            OpvullenWissenLeerlingLijst();




            bool result = false;
            MySqlConnection connection = new MySqlConnection(_conn);
            try
            {
                connection.Open();
                result = true;
                connection.Close();
            }
            catch
            {
                result = false;
            }

            if (result == true)
            {
                StatusDatabase.Fill = Brushes.Green;
            }
            else
            {
                StatusDatabase.Fill = Brushes.Red;
            }
            
        }
        // Begin StatusIntelling

        private void OpvullenLeerlingLijst()
        {
            conn.Open();
            string _cmd = string.Format("SELECT * from leerling");
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]));
                lstLeerlinglijst.Items.Add(objLeerling);
            }

            conn.Close();
        }

        private void txtZoekNaam_KeyUp(object sender, KeyEventArgs e)
        {
            conn.Open();
            string _cmd = string.Format("SELECT * from leerling where LeerlingVNaam like '{0}%' ", txtLeerlingNaam.Text);
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            lstLeerlinglijst.Items.Clear();
            while (dr.Read())
            {

                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]));
                lstLeerlinglijst.Items.Add(objLeerling);
            }

            conn.Close();
        }

        private void OpvullenAanwezigheid(Leerling _objLeerling)
        {
            conn.Open();
            string _cmd = string.Format("SELECT * from aanwezigheidslijst where Leerling_idLeerlingen = {0}", _objLeerling.strIdnummer);
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Aanwezigheid objAanwezigheid = new Aanwezigheid(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString());
                lstAanwezigheidslijst.Items.Add(objAanwezigheid);
            }

            conn.Close();
        }

        private void lstAanwezigheidslijst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rbAanwezig.IsChecked = false;
            rbTeLaat.IsChecked = false;
            rbTeLaatReden.IsChecked = false;
            rbAfwezig.IsChecked = false;
            Aanwezigheid objAanwezigheid = (Aanwezigheid)lstAanwezigheidslijst.SelectedItem;
            if (objAanwezigheid == null)
            {
                return;
            }
            switch (objAanwezigheid.strStatusId)
            {
                case "1":
                    rbAanwezig.IsChecked = true;
                    break;

                case "2":
                    rbTeLaat.IsChecked = true;
                    break;

                case "3":
                    rbTeLaatReden.IsChecked = true;
                    break;

                case "4":
                    rbAfwezig.IsChecked = true;
                    break;
            }
        }

        private void UpdateDBStatus(String _CkStatus, Leerling _objLeerling)
        {
            conn.Open();
            string _cmd = string.Format("update aanwezigheidslijst set Status_idStatus = {0} where Leerling_idLeerlingen = {1}", _CkStatus, _objLeerling.strIdnummer);
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    
        private void lstLeerlinglijst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstAanwezigheidslijst.Items.Clear();
            Leerling objLeerling = (Leerling)lstLeerlinglijst.SelectedItem;
            OpvullenAanwezigheid(objLeerling);
        }

        private void rbAanwezig_Checked(object sender, RoutedEventArgs e)
        {
            Leerling objLeerling = (Leerling)lstLeerlinglijst.SelectedItem;
            if (objLeerling == null)
            {
                return;
            }
            else
            {
                UpdateDBStatus("1", objLeerling);
            }
        }

        private void rbTeLaatReden_Checked(object sender, RoutedEventArgs e)
        {
            Leerling objLeerling = (Leerling)lstLeerlinglijst.SelectedItem;
            if (objLeerling == null)
            {
                return;
            }
            else
            {
                UpdateDBStatus("3", objLeerling);
            }
        }

        private void rbTeLaat_Checked(object sender, RoutedEventArgs e)
        {
            Leerling objLeerling = (Leerling)lstLeerlinglijst.SelectedItem;
            if (objLeerling == null)
            {
                return;
            }
            else
            {
                UpdateDBStatus("2", objLeerling);
            }
        }

        private void rbAfwezig_Checked(object sender, RoutedEventArgs e)
        {
            Leerling objLeerling = (Leerling)lstLeerlinglijst.SelectedItem;
            if (objLeerling == null)
            {
                return;
            }
            else
            {
                UpdateDBStatus("4", objLeerling);
            }
        }

        private void txtWeekindelingNaam_KeyUp(object sender, KeyEventArgs e)
        {
            conn.Open();
            string _cmd = string.Format("SELECT * from leerling where LeerlingVNaam like '{0}%' or LeerlingANaam like '{0}%' ", txtWeekindelingNaam.Text);
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            lstWeekindelingLeerlingen.Items.Clear();
            while (dr.Read())
            {

                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]));
                lstWeekindelingLeerlingen.Items.Add(objLeerling);
            }

            conn.Close();
        }

        // Begin DagInstelling

        private void OpvullenDagInstelling()
        {
            conn.Open();
            string _cmd = string.Format("SELECT * from leerling");
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]));
                lstWeekindelingLeerlingen.Items.Add(objLeerling);
            }

            conn.Close();
        }

        private void lstWeekindelingLeerlingen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Leerling objLeerling = (Leerling)lstWeekindelingLeerlingen.SelectedItem;
            chkMaandag.IsChecked = objLeerling.blMaandag;
            chkDinsdag.IsChecked = objLeerling.blDinsdag;
            chkDonderdag.IsChecked = objLeerling.blDonderdag;
            chkVrijdag.IsChecked = objLeerling.blVrijdag;
        }

        private void UpdateDBDag(string _DBDag, bool _blDag, Leerling _objLeerling)
        {
            conn.Open();
            string _cmd = string.Format("update leerling set {0} = {1} where idLeerlingen = {2}", _DBDag, _blDag, _objLeerling.strIdnummer);
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void chkMaandag_Click(object sender, RoutedEventArgs e)
        {
            Leerling objLeerling = (Leerling)lstWeekindelingLeerlingen.SelectedItem;
            if (objLeerling == null)
            {
                return;
            }
            else
            {
                objLeerling.blMaandag = Convert.ToBoolean(chkMaandag.IsChecked);
                UpdateDBDag("Monday", objLeerling.blMaandag, objLeerling);
            }
        }

        private void chkDinsdag_Click (object sender, RoutedEventArgs e)
        {
            Leerling objLeerling = (Leerling)lstWeekindelingLeerlingen.SelectedItem;
            if (objLeerling == null)
            {
                return;
            }
            else
            {
                objLeerling.blMaandag = Convert.ToBoolean(chkMaandag.IsChecked);
                UpdateDBDag("Tuesday", objLeerling.blMaandag, objLeerling);
            }
        }

        private void chkDonderdag_Click(object sender, RoutedEventArgs e)
        {
            Leerling objLeerling = (Leerling)lstWeekindelingLeerlingen.SelectedItem;
            if (objLeerling == null)
            {
                return;
            }
            else
            {
                objLeerling.blMaandag = Convert.ToBoolean(chkMaandag.IsChecked);
                UpdateDBDag("Thursday", objLeerling.blMaandag, objLeerling);
            }
        }

        private void chkVrijdag_Click(object sender, RoutedEventArgs e)
        {
            Leerling objLeerling = (Leerling)lstWeekindelingLeerlingen.SelectedItem;
            if (objLeerling == null)
            {
                return;
            }
            else
            {
                objLeerling.blMaandag = Convert.ToBoolean(chkMaandag.IsChecked);
                UpdateDBDag("Friday", objLeerling.blMaandag, objLeerling);
            }
        }

        private void txtLeerlingNaam_KeyUp(object sender, KeyEventArgs e)
        {
            conn.Open();
            string _cmd = string.Format("SELECT * from leerling where LeerlingVNaam like '{0}%' ", txtLeerlingNaam.Text);
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            lstLeerlinglijst.Items.Clear();
            while (dr.Read())
            {

                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]));
                lstLeerlinglijst.Items.Add(objLeerling);
            }

            conn.Close();
        }

        // Begin ToevoegInstelling

        // Begin Beheer Kaarten

        // Begin Wissen

        public void OpvullenWissenLeerlingLijst()
        {
            conn.Open();
            string _cmd = string.Format("SELECT * from leerling");
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]));
                lstLeerling.Items.Add(objLeerling);
            }

            conn.Close();
        }

        private void txtLeerling_KeyUp(object sender, KeyEventArgs e)
        {
            conn.Open();
            string _cmd = string.Format("SELECT * from leerling where LeerlingVNaam like '{0}%' or LeerlingANaam like '{0}%' ", txtWeekindelingNaam.Text);
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            lstLeerling.Items.Clear();
            while (dr.Read())
            {

                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]));
                lstLeerling.Items.Add(objLeerling);
            }

            conn.Close();
        }
    }
}