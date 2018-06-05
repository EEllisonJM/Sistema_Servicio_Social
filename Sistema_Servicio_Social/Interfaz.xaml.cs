﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        private void btnRegresarLogin(object sender, RoutedEventArgs e)
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
        private void btnAbrirArchivoCSV(object sender, RoutedEventArgs e)
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
        private void btnDuplicidadCSV(object sender, RoutedEventArgs e)
        {
            if (txtRutaArchivo.Text != "")
            {
                DuplicidadCSV duplicidadCSV = new DuplicidadCSV();
                duplicidadCSV.analizarCSV(txtRutaArchivo.Text);
            }
            else
            {
                MessageBox.Show("Seleccione el archivo CSV a analizar");
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

        private void btnDescargarCSV(object sender, RoutedEventArgs e)
        {
            List<string> lista = Solicitud.get("https://docs.google.com/spreadsheets/d/12mePz3imNk_LiG4PWkB4KmNJ0pycqaZzTytamR-7B0s/export?format=csv");
            if (lista.Count == 0)
            {
                Console.WriteLine("Sin Datos");
            } else
            {
                String rutaDescargaCSV = "";
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                // Set filter and RestoreDirectory
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Filter = "Archivo de texto(*.csv)|*.csv";
                bool? result = saveFileDialog.ShowDialog();
                if (result == true)
                {
                    if (saveFileDialog.FileName.Length > 0)
                    {
                        rutaDescargaCSV = saveFileDialog.FileName;
                        Solicitud.guardarCSV(rutaDescargaCSV, lista);
                    }
                }
            }
        }

        private void btnCrearRespaldo(object sender, RoutedEventArgs e)
        {
            String rutaRespaldo = "";
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            // Set filter and RestoreDirectory
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = "Script de base de datos(*.sql)|*.sql";
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                if (saveFileDialog.FileName.Length > 0)
                {
                    rutaRespaldo = saveFileDialog.FileName;
                    Console.WriteLine(rutaRespaldo);
                    DBConnect db = new DBConnect();
                    db.Backup(rutaRespaldo);
                }
            }
            //string comando = "\"C:\\Program Files\\MySQL\\MySQL Server 5.7\\bin\\mysqldump.exe\" --databases servicio_social -u root -p servicio_social --result-file=Respaldo.sql";
            //System.Diagnostics.Process.Start("CMD.exe", comando);
        }

        private void btnUsuarios(object sender, RoutedEventArgs e)
        {

        }
    }
}