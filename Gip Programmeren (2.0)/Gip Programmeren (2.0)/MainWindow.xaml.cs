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
using Microsoft.Win32;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using System.IO.Ports;
using System.IO;
using Excel;

namespace Gip_Programmeren__2._0_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Wat+Werkwoord+Hoe

        static string _conn = string.Format("server=84.196.202.210;user id=Denzel;database=arduino;password={0}", "Denzel");
        static MySqlConnection conn = new MySqlConnection(_conn);

        List<Leerling> lstLeerlingLijst = new List<Leerling>();
        List<Klas> lstKlasLijst = new List<Klas>();
        //Opletten internet kan uitvallen en dan wil men nog steeds mensen opslaan.
        
       //Excel
        DataSet result;
        DataGrid dataGrid = new DataGrid();

        public delegate void NoArgDelegate();
        SerialPort Sp;
        string strCardData;
        string portName = "COM3";
        bool blIsScanning = true;
#region Startup
        public MainWindow()
        {
            InitializeComponent();
            
            FillCreditsPicture(img1300124, "1300124");
            FillCreditsPicture(img1300154, "1300154");
            FillCreditsPicture(img1400089, "1400089");


            if (TryConnectionWithDataBase())
            {
                StatusDatabase.Fill = Brushes.Green;
                ListLeerlingFillWithDB();
                ListboxRefreshWithList(lstWeekindelingLeerlingen, cboDagKlassen, lstLeerlingLijst);
                ListboxRefreshWithList(lstLeerlinglijst, cboAanwezigheden, lstLeerlingLijst);
                OpvullenWissenLeerlingLijst();
                ListKlasFillWithDB(lstKlasLijst);
                CboKlassenFillWithList(cboToevoegKlas, lstKlasLijst);
                CboKlassenFillWithList(cboDagKlassen, lstKlasLijst);
                CboKlassenFillWithList(cboAanwezigheden, lstKlasLijst);
                ListboxFillLeerlingZonderKaart(lstListBoxLink);
            }
            else
            {
                StatusDatabase.Fill = Brushes.Red;
            }

            TryConnectionWithScanner();

        }

        //Try Database Connection
        private bool TryConnectionWithDataBase()
        {
            MySqlConnection connection = new MySqlConnection(_conn);
            try
            {
                connection.Open();
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Try scanner Connection
        private void TryConnectionWithScanner()
        {
            if (CheckComPorts(portName))
            {
                ScannerStatus.Fill = Brushes.Green;
                OpenArduinoCon();
            }
            else
            {
                ScannerStatus.Fill = Brushes.Red;
            }
        }

        //Scan Method
        private void OpenArduinoCon()
        {
            Sp = new SerialPort();
            Sp.PortName = portName;
            Sp.BaudRate = 115200;
            Sp.Parity = Parity.None;
            Sp.StopBits = StopBits.One;
            Sp.DataBits = 8;
            Sp.Handshake = Handshake.None;
            Sp.Open();
            Sp.DataReceived += new SerialDataReceivedEventHandler(_OnDataRecieved);
        }

        private void _OnDataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            base.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, (NoArgDelegate)delegate
            {
                lblOverzichtNaam.Content = "";

                SerialPort Sp = (SerialPort)sender;
                strCardData = Sp.ReadExisting();

                if (blIsScanning)
                {
                    lblOverzichtNaam.Content = strCardData.ToString();
                }
                else
                {
                    Leerling objLeerling = (Leerling)lstListBoxLink.SelectedItem;
                    CardAddToLeerlingOnScan(strCardData, objLeerling);
                    LeerlingRemoveFromListboxOnScan(lstListBoxLink);
                }
            });
        }

        //Check COM-poorten
        private bool CheckComPorts(string portname)
        {
            string[] strComPorts = SerialPort.GetPortNames();
            foreach (string port in strComPorts)
            {
                if (port == portname)
                {
                    return true;
                }
            }
            return false;
        }
#endregion
        // Begin StatusIntelling

        private void OpvullenLeerlingLijst()
        {
            conn.Open();
            string _cmd = string.Format("SELECT * from leerling");
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]), Convert.ToString(dr[10]), Convert.ToInt16(dr[8]), Convert.ToString(dr[9]));
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

        private void UpdateDBStatus(String _CkStatus, Leerling _objLeerling)
        {
            conn.Open();
            string _cmd = string.Format("update aanwezigheidslijst set Status_idStatus = {0} where Leerling_idLeerlingen = {1}", _CkStatus, _objLeerling.strIdnummer);
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
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

        // Begin ToevoegInstelling

        private void ListLeerlingFillWithDB()
        {
            conn.Open();
            string _cmd = string.Format("SELECT * from leerling");
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]), Convert.ToString(dr[10]), Convert.ToInt16(dr[8]), Convert.ToString(dr[9]));
                lstLeerlingLijst.Add(objLeerling);
            }
            conn.Close();
        }
        private void ListKlasFillWithDB(List<Klas> objListKlas)
        {
            conn.Open();
            string _cmd = String.Format("SELECT * FROM arduino.klassen;");
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Klas objKlas = new Klas(dr[1].ToString(), (TimeSpan)dr[2], (int)dr[0]);
                objListKlas.Add(objKlas);
            }
            conn.Close();
        }

        private void CboKlassenFillWithList(ComboBox cboBox, List<Klas> objListKlas)
        {
            cboBox.Items.Add("Alle Klassen");
            cboBox.SelectedIndex = 0;
            foreach (Klas klas in objListKlas)
            {
                cboBox.Items.Add(klas);
            }
        }

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

        private void InsertPicturesInstellingen(Image imgSetting, string strFileName)
        {
            string strPath;

            strPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Images/", strFileName + ".jpg");
            Uri imageUri = new Uri(strPath);
            imgSetting.Source = new BitmapImage(imageUri);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void WisLeerling()
        {

        }
        
        //Aanmaken Leerling List
        private void ListboxRefreshWithList(ListBox lstListBox, ComboBox cboKlasBox, List<Leerling> objListLeerling)
        {
            cboKlasBox.SelectedIndex = 0;
            lstListBox.Items.Clear();
            foreach (Leerling leerling in objListLeerling)
            {
                lstListBox.Items.Add(leerling);
            }
        }

        //Refresh listbox
        private void ListboxRefreshOnSearch(ListBox lstLeerlingListBox, TextBox txtSearchBox, ComboBox cboKlasBox)
        {
            if (cboKlasBox.SelectedItem == null)
            {
                return;
            }
            if (cboKlasBox.SelectedValue.ToString() == "Alle Klassen")
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                string _cmd = string.Format("SELECT * from leerling where LeerlingVNaam like '{0}%' or LeerlingANaam like '{0}%'", txtSearchBox.Text);
                MySqlCommand cmd = new MySqlCommand(_cmd, conn);
                MySqlDataReader dr = cmd.ExecuteReader();
                lstLeerlingListBox.Items.Clear();
                while (dr.Read())
                {
                    Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]), Convert.ToString(dr[10]));
                    lstLeerlingListBox.Items.Add(objLeerling);
                }
                conn.Close();
            }
            else
            {
                Klas objKlas = (Klas)cboKlasBox.SelectedItem;
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                string _cmd = string.Format("SELECT * from leerling where (LeerlingVNaam like '{0}%' or LeerlingANaam like '{0}%')  and klassen_idKlassen = {1}", txtSearchBox.Text, objKlas.intId);
                MySqlCommand cmd = new MySqlCommand(_cmd, conn);
                MySqlDataReader dr = cmd.ExecuteReader();
                lstLeerlingListBox.Items.Clear();
                while (dr.Read())
                {
                    Leerling objLeerling = new Leerling(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), Convert.ToInt16(dr[3]), Convert.ToBoolean(dr[4]), Convert.ToBoolean(dr[5]), Convert.ToBoolean(dr[6]), Convert.ToBoolean(dr[7]), Convert.ToString(dr[10]));
                    lstLeerlingListBox.Items.Add(objLeerling);
                }
                conn.Close();
            }
            
        }

        //Fill linking listbox
        private void ListboxFillLeerlingZonderKaart(ListBox lstLeerlingListbox)
        {
            foreach (Leerling leerling in lstLeerlingLijst)
            {
                if (leerling.strIdKaart == "" || leerling.strIdKaart == null)
                {
                    lstLeerlingListbox.Items.Add(leerling);
                }
            }
            lstLeerlingListbox.SelectedIndex = 0;
        }

        //Fill Credits Pictures

        private void FillCreditsPicture(Image imgCredits, string strImage)
        {
            Uri uri = new Uri(String.Format("./fotos/{0}.jpg", strImage), UriKind.Relative);
            imgCredits.Source = new BitmapImage(uri);
        }

        private void AanwezigheidAddToDB(Leerling objLeerling)
        {
            DateTime dteNow = DateTime.Now;
            conn.Open();
            string _cmd = string.Format("INSERT INTO 'arduino'.'aanwezigheid' (' Datum', 'Leerling_idLeerlingen', 'Status_idStatus') VALUES ('{0}', '{1}', '{2}', '{3}');", dteNow, objLeerling.strIdnummer, "1");
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            MySqlDataReader dr = cmd.ExecuteReader();
        }

        private void LeerlingRemoveFromListboxOnScan(ListBox lstBox)
        {
            int intIndexList = lstBox.SelectedIndex;
            if (intIndexList < 0)
            {
                return;
            }
            lstBox.Items.RemoveAt(intIndexList);
            lstBox.SelectedIndex = intIndexList;
        }

        private void CardAddToLeerlingOnScan(string strCardID, Leerling objLeerling)
        {
            string _cmd = string.Format("UPDATE arduino.leerling SET LeerlingKaartID = '{0}' WHERE idLeerlingen = '{1}'", strCardID.ToString(), Convert.ToInt32(objLeerling.strIdnummer));
            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        #region Events
        #region Button Click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            Klas objKlas = (Klas)cboToevoegKlas.SelectedItem;
            string _cmd = string.Format("INSERT INTO `arduino`.`leerling` (`idLeerlingen`, `LeerlingVNaam`, `LeerlingANaam`, `LeerlingKlasNummer`, `Monday`, `Tuesday`, `Thursday`, `Friday`, `klassen_idKlassen`) VALUES ('{4}', '{2}', '{1}', '{3}', '{6}', '{7}', '{8}', '{9}', '{5}');",
            txtVoornaam, txtAchternaam, txtKlasnummer, txtStamboeknummer, objKlas.intId, chkMa.IsChecked, chkDi.IsChecked, chkDo.IsChecked, chkVr.IsChecked);


            MySqlCommand cmd = new MySqlCommand(_cmd, conn);
        }
        
        private void btnStartLink_Click(object sender, RoutedEventArgs e)
        {
            blIsScanning = false;
        }

        private void btnStopLink_Click(object sender, RoutedEventArgs e)
        {
            blIsScanning = true;
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Leerling objLeerling = (Leerling)lstLeerling.SelectedItem;
            if (objLeerling == null)
            {
                return;
            }
            Popup Popup = new Popup();
            Popup.strIDLeerling = objLeerling.strIdnummer;
            Popup.ShowDialog();
        }

        private void btnImport_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog FileDia1 = new OpenFileDialog();

            FileDia1.DefaultExt = ".xlsx, .xls";
            FileDia1.Filter = "excel|*.xlsx";

            if (FileDia1.ShowDialog() == true)
            {
                FileStream fs = File.Open(FileDia1.FileName, FileMode.Open, FileAccess.Read);
                IExcelDataReader reader = ExcelReaderFactory.CreateBinaryReader(fs);
                reader.IsFirstRowAsColumnNames = true;
                result = reader.AsDataSet();

                reader.Close();

                Select.Text = FileDia1.FileName;
            }
        }

        private void btnImport1_Click(object sender, RoutedEventArgs e)
        {



            //String name = "Items";
            //String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
            //                "C:\\Sample.xlsx" +
            //                ";Extended Properties='Excel 12.0 XML;HDR=YES;';";

            //OleDbConnection con = new OleDbConnection(constr);
            //OleDbCommand oconn = new OleDbCommand("Select * From [" + name + "$]", con);
            //con.Open();




            string Conn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Select.Text + ";Extended Properties = \"Excel 12.0 Xml;HDR=YES\"; ";
            OleDbConnection conn = new OleDbConnection(Conn);

            OleDbCommand oconn = new OleDbCommand("Select * from [Sheet1$]", conn);
            conn.Open();

            //var dr = oconn.ExecuteReader();

            OleDbDataAdapter sda = new OleDbDataAdapter(oconn);
            DataTable data = new DataTable();
            sda.Fill(data);
            dataGrid.ItemsSource = data.DefaultView;
        }
        #endregion

        #region Listbox SelectionChanged
        private void lstWeekindelingLeerlingen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Leerling objLeerling = (Leerling)lstWeekindelingLeerlingen.SelectedItem;
            if (objLeerling == null)
                return;
            chkMaandag.IsChecked = objLeerling.blMaandag;
            chkDinsdag.IsChecked = objLeerling.blDinsdag;
            chkDonderdag.IsChecked = objLeerling.blDonderdag;
            chkVrijdag.IsChecked = objLeerling.blVrijdag;
        }

        private void lstLeerlinglijst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstAanwezigheidslijst.Items.Clear();
            Leerling objLeerling = (Leerling)lstLeerlinglijst.SelectedItem;
            if (objLeerling == null)
                return;
            else
                OpvullenAanwezigheid(objLeerling);
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

        private void lstLeerling_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblNaam.Content = null;
            lblAchternaam.Content = null;
            lblNummer.Content = null;
            lblKlas.Content = null;
            IMGWissen.Source = null;

            Leerling objLeerling = (Leerling)lstLeerling.SelectedItem;
            string strPath = String.Format("//hubble/leerlingfotos$/{0}.jpg", objLeerling.strIdnummer);
            lblNaam.Content = objLeerling.strVoornaam;
            lblAchternaam.Content = objLeerling.strAchternaam;
            lblKlas.Content = objLeerling.strKlas;
            lblNummer.Content = objLeerling.intKlasnummer;
            Uri uri = new Uri(strPath, UriKind.Absolute);

            try
            {
                IMGWissen.Source = new BitmapImage(uri);
            }

            catch (Exception)
            {
                Uri uri2 = new Uri("//hubble/leerlingfotos$/1400059.jpg");
                IMGWissen.Source = new BitmapImage(uri2);
            }
        }


        #endregion

        #region Combobox SelectionChanged
        private void cboAanwezigheden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtLeerlingNaam.Clear();
            ListboxRefreshOnSearch(lstLeerlinglijst, txtLeerlingNaam, cboAanwezigheden);
        }

        private void cboDagKlassen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtWeekindelingNaam.Clear();
            ListboxRefreshOnSearch(lstWeekindelingLeerlingen, txtWeekindelingNaam, cboDagKlassen);
        }


        #endregion

        #region TextBox KeyUp
        private void txtWissen_KeyUp(object sender, KeyEventArgs e)
        {
            ListboxRefreshOnSearch(lstLeerling, txtWissen, cboWissen);
        }

        private void txtLeerlingNaam_KeyUp(object sender, KeyEventArgs e)
        {
            ListboxRefreshOnSearch(lstLeerlinglijst, txtLeerlingNaam, cboAanwezigheden);
        }

        private void txtWeekindelingNaam_KeyUp(object sender, KeyEventArgs e)
        {
            ListboxRefreshOnSearch(lstWeekindelingLeerlingen, txtWeekindelingNaam, cboDagKlassen);
        }


        #endregion

        #region /
        private void TabControl_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }




        #endregion

        #endregion

        private void btnRetryDBCon_Click(object sender, RoutedEventArgs e)
        {
            TryConnectionWithDataBase();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string newConn = string.Format("{0};user id={1};database={2};password={3}", txtInstellingenServerIP.Text, txtInstallingenUsername.Text, txtInstallingenDatabaseName.Text, txtInstellingenPassword.Text);
            _conn = newConn;
            TryConnectionWithDataBase();
        }
    }
}