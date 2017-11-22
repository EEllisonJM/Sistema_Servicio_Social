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
        private void btnSiguiente(object sender, RoutedEventArgs e)
        { }
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
        //---

        /// <summary>
        ///  Select Word file 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectWord_Click(object sender, RoutedEventArgs e)
        {
            // Initialize an OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Set filter and RestoreDirectory
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Word documents(*.doc;*.docx)|*.doc;*.docx";
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                if (openFileDialog.FileName.Length > 0)
                {
                    txbSelectedWordFile.Text = openFileDialog.FileName;
                }
            }
        }

        /// <summary>
        ///  Convert the word document to xps document
        /// </summary>
        /// <param name="wordFilename">Word document Path</param>
        /// <param name="xpsFilename">Xps document Path</param>
        /// <returns></returns>
        private XpsDocument ConvertWordToXps(string wordFilename, string xpsFilename)
        {
            // Create a WordApplication and host word document
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
        /// <summary>
        ///  View Word Document in WPF DocumentView Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewDoc_Click(object sender, RoutedEventArgs e)
        {
            string wordDocument = txbSelectedWordFile.Text;
            if (string.IsNullOrEmpty(wordDocument) || !File.Exists(wordDocument))
            {
                MessageBox.Show("The file is invalid. Please select an existing file again.");
            }
            else
            {
                string convertedXpsDoc = string.Concat(System.IO.Path.GetTempPath(), "\\", Guid.NewGuid().ToString(), ".xps");
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
