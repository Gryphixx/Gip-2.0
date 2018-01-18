﻿using System;
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

         List<Leerling> lstLeerlingLijst = new List<Leerling>();


        public MainWindow()
        {
            InitializeComponent();

            InsertPicturesInstellingen(img1300124, "1300124");
            InsertPicturesInstellingen(img1300154, "1300154");
            InsertPicturesInstellingen(img1400089, "1400089");

            ScannerStatus.Fill = Brushes.Red;


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
                OpvullenLeerlingLijst();
                OpvullenDagInstelling();
                OpvullenWissenLeerlingLijst();
                OpvullenCboKlassen();
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
                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]), Convert.ToString(dr[10]));
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

                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]), Convert.ToString(dr[10]));
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

                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]), Convert.ToString(dr[10]));
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
                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]), Convert.ToString(dr[10]));
                lstLeerlingLijst.Add(objLeerling);
            }

            foreach (Leerling item in lstLeerlingLijst)
            {
                lstWeekindelingLeerlingen.Items.Add(item);
            }
            conn.Close();
        }

        private void OpvullenDagInstellingKlassen()
        {
            conn.Open();
            string _cmd = string.Format("SELECT * from klassen");
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Klas objKlas = new Klas(dr[0].ToString(), (TimeSpan)dr[2], (int)dr[0]);
                cboDagKlassen.Items.Add(objKlas);
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
                objLeerling.blDinsdag = Convert.ToBoolean(chkDinsdag.IsChecked);
                UpdateDBDag("Tuesday", objLeerling.blDinsdag, objLeerling);
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
                objLeerling.blDonderdag = Convert.ToBoolean(chkDonderdag.IsChecked);
                UpdateDBDag("Thursday", objLeerling.blDonderdag, objLeerling);
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
                objLeerling.blVrijdag = Convert.ToBoolean(chkVrijdag.IsChecked);
                UpdateDBDag("Friday", objLeerling.blVrijdag, objLeerling);
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

                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]), Convert.ToString(dr[10]));
                lstLeerlinglijst.Items.Add(objLeerling);
            }

            conn.Close();
        }

        private void cboDagKlassen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cboDagKlassen.Items.Clear();
            Klas objKlas = (Klas)cboDagKlassen.SelectedItem;

            foreach (Leerling item in lstLeerlingLijst)
            {
                if (item.intKlasnummer == objKlas.intJaar)
                {
                    lstWeekindelingLeerlingen.Items.Add(item);
                }
            }

        }


        // Begin ToevoegInstelling

        private void OpvullenCboKlassen()
        {
            conn.Open();
            string _cmd = String.Format("SELECT * FROM arduino.klassen;");
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Klas objKlas = new Klas(dr[1].ToString(),(TimeSpan)dr[2],(int)dr[0]);
                cboToevoegKlas.Items.Add(objKlas);
            }
            conn.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            Klas objKlas = (Klas)cboToevoegKlas.SelectedItem;
            string _cmd = string.Format("INSERT INTO `arduino`.`leerling` (`idLeerlingen`, `LeerlingVNaam`, `LeerlingANaam`, `LeerlingKlasNummer`, `Monday`, `Tuesday`, `Thursday`, `Friday`, `klassen_idKlassen`) VALUES ('{4}', '{2}', '{1}', '{3}', '{6}', '{7}', '{8}', '{9}', '{5}');", txtVoornaam, txtAchternaam, txtKlasnummer, txtStamboeknummer, objKlas.intId, chkMa.IsChecked, chkDi.IsChecked, chkDo.IsChecked, chkVr.IsChecked);
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {

        }

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
                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]), Convert.ToString(dr[10]));
                lstLeerling.Items.Add(objLeerling);
            }

            conn.Close();
        }

        private void txtLeerling_KeyUp(object sender, KeyEventArgs e)
        {
            conn.Open();
            string _cmd = string.Format("SELECT * from leerling where LeerlingVNaam like '{0}%' or LeerlingANaam like '{0}%' ", txtWissen.Text);
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            lstLeerling.Items.Clear();
            while (dr.Read())
            {

                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]), Convert.ToString(dr[10]));
                lstLeerling.Items.Add(objLeerling);
            }

            conn.Close();
        }

        private void lstLeerling_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblNaam.Content = null;
            lblAchternaam.Content = null;
            lblNummer.Content = null;
            lblKlas.Content = null;
            IMGWissen.Source = null;

            Leerling objLeerling = (Leerling)lstLeerling.SelectedItem;
            lblNaam.Content = objLeerling.strVoornaam;
            lblAchternaam.Content = objLeerling.strAchternaam;
            lblKlas.Content = objLeerling.strKlas;
            lblNummer.Content = objLeerling.intKlasnummer;

           

        }

        private void InsertPicturesInstellingen(Image imgSetting, string strFileName)
        {
            string strPath;

            strPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Images/", strFileName + ".jpg");
            Uri imageUri = new Uri(strPath);
            //imgSetting.Source = new BitmapImage(imageUri);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Popup Popup = new Popup();
            Popup.Show();
            


        }



    }
}