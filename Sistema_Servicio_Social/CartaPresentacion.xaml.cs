﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Xps.Packaging;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using Word = Microsoft.Office.Interop.Word;
using System.Collections.Generic;

namespace Sistema_Servicio_Social
{
    public partial class CartaPresentacion : System.Windows.Window
    {
        string e_mailEnviar = "";
        string directorioGuardarDocumento = "";
        public CartaPresentacion()
        {
            InitializeComponent();
        }
        private void btnSelectWord_Click(object sender, RoutedEventArgs e)
        {// Initialize an OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Set filter and RestoreDirectory
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Word documents(*.dotx)|*.dotx";
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                if (openFileDialog.FileName.Length > 0)
                {
                    txtPlantilla.Text = openFileDialog.FileName;
                }
            }
        }
        private XpsDocument ConvertWordToXps(string wordFilename, string xpsFilename)
        {// Create a WordApplication and host word document
            Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            try
            {
                wordApp.Documents.Open(wordFilename);
                // To Invisible the word document
                wordApp.Application.Visible = false;
                // Minimize the opened word document
                wordApp.WindowState = WdWindowState.wdWindowStateMinimize;
                Document doc = wordApp.ActiveDocument;
                doc.SaveAs(xpsFilename, WdSaveFormat.wdFormatXPS);
                XpsDocument xpsDocument = new XpsDocument(xpsFilename, FileAccess.Read);
                return xpsDocument;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurs, The error message is  " + ex.ToString());
                return null;
            }
            finally
            {
                wordApp.Documents.Close();
                ((_Application)wordApp).Quit(WdSaveOptions.wdDoNotSaveChanges);
            }
        }

        void guardarDocumento(String numExpediente)
        {
            DBConnect dbConnect = new DBConnect();
            List<string> list = dbConnect.Select(numExpediente);
            WordTemplate wt = new WordTemplate(txtPlantilla.Text);
            wt.reemplazarCampo("Leyenda", list[5]);
            DateTime dateTime = DateTime.UtcNow.Date;
            wt.reemplazarCampo("Fecha", dateTime.ToString("dd/MM/yyyy"));//Agregar la fecha actual
            wt.reemplazarCampo("Anio", dateTime.ToString("yy"));
            wt.reemplazarCampo("NumeroExpediente", list[7]);
            wt.reemplazarCampo("JefeDireccion", list[8]);
            wt.reemplazarCampo("Puesto", list[9]);
            wt.reemplazarCampo("Sexo", getSexo(list[3]));

            wt.reemplazarCampo("NombreAlumno", list[1]);
            wt.reemplazarCampo("NumeroControl", list[0]);//
            wt.reemplazarCampo("Carrera", list[2]);

            wt.reemplazarCampo("Dependencia", getDependencia(list[11]));//

            wt.reemplazarCampo("Programa", list[6]);

            wt.guardarDocumento("Hola12345");//nombreDocumento
            cargarDatos(list);
            e_mailEnviar = list[4];//e_mail
        }
        string getSexo(String texto)
        {
            if (texto == "H")
            {
                return "al";
            }
            if (texto == "M")
            {
                return "a la";
            }
            return "";
        }
        string getDependencia(string texto)
        {
            var WordsArray = texto.Split();
            string aux = WordsArray[0];
            switch (aux)
            {
                case "Departamento":
                    return "ese departamento";
                case "Oficina":
                    return "esa oficina";
                case "Division":
                    return "esa división";
                case "División":
                    return "esa división";

            }
            return aux;
        }

        private void cargarDatos(List<string> list)
        {
            txbSelectedWordFile.Text = list[7];
            txtLeyenda.Text = list[5];
            //DateTime dateTime = DateTime.UtcNow.Date;
            //txtFecha.Text = dateTime.ToString("dd/MM/yyyy"));//Agregar la fecha actual
            txtNombreAlumno.Text = list[1];
            txtCarrera.Text = list[2];
            //txtSexo.Text = list[3];
            //txtNombreDependencia.Text=
            txtDireccion.Text = list[10];
            txtPrograma.Text = list[6];
            txtNombreJefeDirecto.Text = list[8];
            txtPuesto.Text = list[9];
            //txtNombreDependencia.Text = list[11].Split()[0];
            txtNombreDependencia.Text = list[11];
        }

        private void btnViewDoc_Click(object sender, RoutedEventArgs e)
        {//Mostrar Documento
            if (txbSelectedWordFile.Text != "")
            {//C:\Users\Erik\Documents\Full_CartaPresentacion.dotx
                guardarDocumento(txbSelectedWordFile.Text + "");
                //cargarDatosCartaPresentacion();
                //string wordDocument = txbSelectedWordFile.Text;
                //El documento que se guarda en el metodo "guardarDocumento" se guarda en los documentos del usuariostring wordDocument = "C:\\Users\\xxxx\\Documents\\Hola12345.doc";
                string wordDocument = "C:\\Users\\Erik\\Documents\\Hola12345.doc";
                mostrarDocumento(wordDocument);
            }
        }
        void mostrarDocumento(String wordDocument)
        {
            if (string.IsNullOrEmpty(wordDocument) || !File.Exists(wordDocument))
            {
                MessageBox.Show("Archivo invalido. Seleccione un archivo.");
            }
            else
            {
                string convertedXpsDoc = string.Concat(Path.GetTempPath(), "\\", Guid.NewGuid().ToString(), ".xps");
                XpsDocument xpsDocument = ConvertWordToXps(wordDocument, convertedXpsDoc);
                if (xpsDocument == null)
                {
                    return;
                }
                documentviewWord.Document = xpsDocument.GetFixedDocumentSequence();
            }
        }

        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (txbSelectedWordFile.Text != "")
            {//C:\Users\Erik\Documents\Full_CartaPresentacion.dotx
                int numE = Int32.Parse(txbSelectedWordFile.Text);
                guardarDocumento((numE + 1) + "");
                //cargarDatosCartaPresentacion();
                //string wordDocument = txbSelectedWordFile.Text;
                //El documento que se guarda en el metodo "guardarDocumento" se guarda en los documentos del usuariostring wordDocument = "C:\\Users\\xxxx\\Documents\\Hola12345.doc";
                string wordDocument = "C:\\Users\\Erik\\Documents\\Hola12345.doc";
                mostrarDocumento(wordDocument);
            }
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            //actualizar la base de datos con los datos modificados
            //cargar al documento
        }

        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            Correo c = new Correo();
            c.EnviarCorreo("C:\\Users\\Erik\\Documents\\Hola12345.doc", "NombreDocumento", "Soy asunto", "Soy mensaje", e_mailEnviar);
        }

        private void btnBuscarRutaDocumentoGenerar(object sender, RoutedEventArgs e)
        {
            //Seleccionar carpeta a guardar
            directorioGuardarDocumento = txtRutaDocumentoGenerar.Text;
        }
    }
}