using System;
using System.Windows;
using System.Windows.Controls;

namespace Sistema_Servicio_Social
{
    public partial class UserControl_LoadFileCSV : UserControl
    {
        public UserControl_LoadFileCSV()
        {
            InitializeComponent();
        }

        private void clickBtnExplorar(object sender, System.Windows.RoutedEventArgs e)
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
                    textBoxRutaArchivo.Text = openFileDialog.FileName;
                }
            }
        }

        private void clickBtnCargar(object sender, System.Windows.RoutedEventArgs e)
        {

            if (textBoxRutaArchivo.Text != "")
            {
                DateTime dateTime = DateTime.UtcNow.Date;

                string numExpI = Microsoft.VisualBasic.Interaction.InputBox(
                "Favor de ingresar el número de expediente inicial",
                "Número de expediente",
                "1000");
                if (numExpI.CompareTo("") == 0) //si numExpI está vacío (botón cancelar)
                {
                    return;
                }
                string anio = "0";
                while (anio.Length != 4)
                {
                    anio = Microsoft.VisualBasic.Interaction.InputBox(
                    "Favor de ingresar el año (debe ser un número de 4 dígitos)",
                    "Año",
                    dateTime.ToString("yyyy"));
                    if (anio.CompareTo("") == 0) //si anio está vacío (botón cancelar)
                    {
                        return;
                    }
                }
                string leyenda = Microsoft.VisualBasic.Interaction.InputBox(
                    "Favor de ingresar la leyenda",
                    "Leyenda",
                    "Año del Centenario de la Promulgación de la Constitución Política de los Estados Unidos Mexicanos");
                if (leyenda.CompareTo("") == 0) //si leyenda está vacío (botón cancelar)
                {
                    return;
                }
                //rutaPlantilla = textBoxArchivo.Text;
                bool mostrarExitoso = true;
                try
                {
                    ConexionMySQL conexionMySQL = new ConexionMySQL();
                    conexionMySQL.leerCSV(textBoxRutaArchivo.Text, Int32.Parse(numExpI), Int32.Parse(anio), leyenda);
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
