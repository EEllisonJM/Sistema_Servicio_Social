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
            List<string>[] list = new List<string>[11];
            list = dbConnect.Select();
            WordTemplate wt = new WordTemplate(txtPlantilla.Text);
            
            wt.reemplazarCampo("NumeroControl", list[0].ToString()+"");
            wt.reemplazarCampo("Carrera", list[1]+"");
            //wt.reemplazarCampo("Carrera", list[1] + "");
            /*list[0] = new List<string>();//NumeroControl
            list[1] = new List<string>();//NombreAlumno
            list[2] = new List<string>();//Carrera
            list[3] = new List<string>();//Sexo
            list[4] = new List<string>();//E_mail
            list[5] = new List<string>();//leyenda
            list[6] = new List<string>();//Programa
            list[7] = new List<string>();//numExpediente
            list[8] = new List<string>();//Jefe direccion
            list[9] = new List<string>();//Puesto
            list[10] = new List<string>();//DireccionDependencia*/
            //----
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