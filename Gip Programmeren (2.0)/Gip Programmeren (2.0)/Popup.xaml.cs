﻿using GIP_Programmeren;
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
using Excel;
namespace Gip_Programmeren__2._0_
{
    /// <summary>
    /// Interaction logic for Popup.xaml
    /// </summary>
    public partial class Popup : Window
    {

        public string strIDLeerling; 



        public Popup()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string _conn = string.Format("server=84.196.202.210;user id=Denzel;database=arduino;password={0}", "Denzel");
            MySqlConnection conn = new MySqlConnection(_conn);
            MySqlCommand comm = new MySqlCommand(String.Format("DELETE FROM leerling WHERE idLeerling ={0}",strIDLeerling));
            MySqlDataReader dr = comm.ExecuteReader();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
