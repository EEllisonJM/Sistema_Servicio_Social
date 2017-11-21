using Microsoft.Win32;
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

namespace Sistema_Servicio_Social
{
    /// <summary>
    /// Lógica de interacción para Interfaz.xaml
    /// </summary>
    public partial class Interfaz : Window
    {
        public Interfaz()
        {
            InitializeComponent();
        }
        //btnRegresar
        private void btnRegresar(object sender, RoutedEventArgs e)
        {
            Login l = new Login();
            l.Show();
            //this.Close();
        }

        private void btnAbrir(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".csv";
            //ofd.Filter = "Text Document (.csv)/*.csv";
            if (ofd.ShowDialog() == true)
            {
                string filename = ofd.FileName;
                txtBox.Text = filename;
            }
        }
        private void btnCargar(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
