using System;
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

        void guardarDocumento() {            
            DBConnect dbConnect = new DBConnect();
            List<string> list = dbConnect.Select();
            WordTemplate wt = new WordTemplate(txtPlantilla.Text);
            //string aux = list[0][0];
            //wt.reemplazarCampo("Leyenda", aux);
            
            wt.reemplazarCampo("Leyenda", list[5]);
            wt.reemplazarCampo("Fecha", "Fecha actual");//Agregar la fecha actual
            wt.reemplazarCampo("NumeroExpediente", list[7]);
            wt.reemplazarCampo("JefeDireccion", list[8]);
            wt.reemplazarCampo("Puesto", list[9]);
            wt.reemplazarCampo("Sexo", list[3]);
            wt.reemplazarCampo("NombreAlumno", list[1]);
            wt.reemplazarCampo("NumeroControl", list[0]);//
            wt.reemplazarCampo("Carrera", list[2]);
            wt.reemplazarCampo("Dependencia", list[10]);
            wt.reemplazarCampo("Programa", list[6]);            
            /*list[4] = new List<string>();//E_mail*/
            wt.guardarDocumento("Hola12345");
        }
        private void btnViewDoc_Click(object sender, RoutedEventArgs e)
        {//Mostrar Documento
            //Hacer una consulta para traer el número de expediente
            //C:\Users\Erik\Documents\Full_CartaPresentacion.dotx
            //Crear archivo con valores traidos de la base de datos
            guardarDocumento();

            //string wordDocument = txbSelectedWordFile.Text;
            //El documento que se guarda en el metodo "guardarDocumento" se guarda en los documentos del usuariostring wordDocument = "C:\\Users\\xxxx\\Documents\\Hola12345.doc";
            string wordDocument = "C:\\Users\\Erik\\Documents\\Hola12345.doc";
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
    }
}