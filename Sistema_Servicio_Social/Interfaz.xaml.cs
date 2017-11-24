using Microsoft.Win32;
using System;
using System.Windows;
using System.IO;
using System.Windows.Xps.Packaging;
using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;

namespace Sistema_Servicio_Social
{
    /// <summary>
    /// Lógica de interacción para Interfaz.xaml
    /// </summary>
    public partial class Interfaz : System.Windows.Window
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
            this.Close();
        }
        private void btnCartaPresentacion(object sender, RoutedEventArgs e)
        {
            CartaPresentacion i = new CartaPresentacion();
            i.Show();
            this.Close();
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
            string numExpI = Microsoft.VisualBasic.Interaction.InputBox(
                "Favor de ingresar el número de expediente inicial",
                "Número de expediente",
                "1000");
            
                string ruta = txtBox.Text;
               bool mostrarExitoso = true;
               try {
                    ConexionMySQL conexionMySQL = new ConexionMySQL();
                    conexionMySQL.leerCSV(ruta, Int32.Parse(numExpI));
               } catch(Exception ex) {
                    mostrarExitoso = false;
                    MessageBox.Show("Error: "+ex.Message);
               }
               if (mostrarExitoso){
                    MessageBox.Show("Datos Cargados Exitosamente");
               }
            
        }
    }
}
