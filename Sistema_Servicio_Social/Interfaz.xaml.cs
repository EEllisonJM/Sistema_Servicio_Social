using Microsoft.Win32;
using System;
using System.Windows;

namespace Sistema_Servicio_Social
{
    public partial class Interfaz : System.Windows.Window
    {
        string rutaPlantilla;
        public Interfaz()
        {
            InitializeComponent();
        }
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
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            // Set filter and RestoreDirectory
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Archivo de texto(*.csv)|*.csv";
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                if (openFileDialog.FileName.Length > 0)
                {
                    txtRutaArchivo.Text = openFileDialog.FileName;
                }
            }
        }
        private void btnCargarDatos(object sender, RoutedEventArgs e)
        {
            if (txtRutaArchivo.Text != "")
            {
                DateTime dateTime = DateTime.UtcNow.Date;

                string numExpI = Microsoft.VisualBasic.Interaction.InputBox(
                "Favor de ingresar el número de expediente inicial",
                "Número de expediente",
                "1000");
                string anio = Microsoft.VisualBasic.Interaction.InputBox(
                    "Favor de ingresar el año",
                    "Año",
                    dateTime.ToString("yyyy"));
                string leyenda = Microsoft.VisualBasic.Interaction.InputBox(
                    "Favor de ingresar la leyenda",
                    "Leyenda",
                    "Año del bicentenario");
                rutaPlantilla = txtRutaArchivo.Text;
                bool mostrarExitoso = true;
                try
                {
                    ConexionMySQL conexionMySQL = new ConexionMySQL();
                    conexionMySQL.leerCSV(rutaPlantilla, Int32.Parse(numExpI), Int32.Parse(anio), leyenda);
                }
                catch (Exception ex)
                {
                    mostrarExitoso = false;
                    MessageBox.Show("Error: " + ex.Message);
                }
                if (mostrarExitoso)
                {
                    MessageBox.Show("Datos Cargados Exitosamente");
                }
            }
            else
            {
                MessageBox.Show("Seleccione el archivo CSV a cargar");
            }
        }
    }
}