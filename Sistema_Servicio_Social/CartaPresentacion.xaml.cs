using System;
using System.IO;
using System.Windows;
using System.Windows.Xps.Packaging;
using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.Windows.Forms;

using Word = Microsoft.Office.Interop.Word;

namespace Sistema_Servicio_Social
{
    public partial class CartaPresentacion : System.Windows.Window
    {
        DateTime dateTime;
        string wordDocument;
        string fechaActual;
        string anioActual;
        string numControl;
        public CartaPresentacion()
        {
            InitializeComponent();
        }
        private void btnSeleccionarPlantilla(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            // Set filter and RestoreDirectory
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Plantilla de Word(*.dotx)|*.dotx";
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
                System.Windows.MessageBox.Show("Error occurs, The error message is  " + ex.ToString());
                return null;
            }
            finally
            {
                wordApp.Documents.Close();
                ((_Application)wordApp).Quit(WdSaveOptions.wdDoNotSaveChanges);
            }
        }
        /*
         * Guardar un documento que tiene como nombre el [número de control]
         * Cuya extensión ´será: [*.doc].
         * Extrae datos de la base de datos de un [número de expediente]
         */
        void guardarDocumento(String numExpediente)
        {
            DBConnect dbConnect = new DBConnect();
            List<string> list = dbConnect.Select(numExpediente);//getValues of [numExpediente]
            WordTemplate wt = new WordTemplate(txtPlantilla.Text);
            wt.reemplazarCampo("Leyenda", list[5]);
            wt.reemplazarCampo("Fecha", fechaActual);//Add current date
            wt.reemplazarCampo("Anio", anioActual);
            wt.reemplazarCampo("NumeroExpediente", list[7]);
            wt.reemplazarCampo("JefeDireccion", list[8]);
            wt.reemplazarCampo("Puesto", list[9]);
            wt.reemplazarCampo("Sexo", setSexo(list[3]));
            wt.reemplazarCampo("NombreAlumno", list[1]);
            wt.reemplazarCampo("NumeroControl", list[0]);
            wt.reemplazarCampo("Carrera", list[2]);
            wt.reemplazarCampo("Dependencia", getDependencia(list[11]));
            wt.reemplazarCampo("Programa", list[6]);

            numControl = list[0];
            wt.guardarDocumento(txtRutaDocumentoGenerar.Text, numControl);//[numControl.doc]
            cargarDatos(list);
        }
        /*
         * Retorna el texto [al] si texto es [H]
         * Retorna el texto [a la] si texto es [M].
         */
        string setSexo(String texto)
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
        /*
         * Cargar datos obtenidos de una lista a la ventana principal.
         */
        private void cargarDatos(List<string> list)
        {
            txtNumExpediente.Text = list[7];
            txtLeyenda.Text = list[5];
            txtNombreAlumno.Text = list[1];
            txtCarrera.Text = list[2];
            txtDireccion.Text = list[10];
            txtPrograma.Text = list[6];
            txtNombreJefeDirecto.Text = list[8];
            txtPuesto.Text = list[9];
            txtNombreDependencia.Text = list[11];
            cBoxSexo.Text = list[3];
        }
        /*
         * Mostrar los datos en el documento al darle click al boton
         * [Mostrar documento]
         */
        private void btnMostrarDocumento(object sender, RoutedEventArgs e)
        {
            if (txtNumExpediente.Text != "")//No vacio
            {
                DBConnect db = new DBConnect();
                if (db.CountOne(//Existe?
                            "Carta_Presentacion",//Table
                            "numExpediente", txtNumExpediente.Text) == 1)//numControl=values[2]?
                {
                    dateTime = DateTime.UtcNow.Date;
                    fechaActual = dateTime.ToString("dd/MM/yyyy");
                    anioActual = dateTime.ToString("yy");
                    Fecha.SelectedDate = DateTime.Today;

                    guardarDocumento(txtNumExpediente.Text + "");
                    wordDocument = txtRutaDocumentoGenerar.Text + "\\" + numControl + ".doc";
                    mostrarDocumento(wordDocument);
                }
                else
                {
                    System.Windows.MessageBox.Show("Número de expediente no encontrado.");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Favor de introducir el número de expediente.");
            }

        }

        void mostrarDocumento(String wordDocument)
        {
            if (string.IsNullOrEmpty(wordDocument) || !File.Exists(wordDocument))
            {
                System.Windows.MessageBox.Show("Archivo invalido. Seleccione un archivo.");
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
        /*
         * Cargar el siguiente número de expediente.
         */
        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (txtNumExpediente.Text != "")//No vacio
            {
                int numE = Int32.Parse(txtNumExpediente.Text);
                numE += 1;
                DBConnect db = new DBConnect();
                if (db.CountOne(//Existe?
                            "Carta_Presentacion",//Table
                            "numExpediente", (numE) + "") == 1)//numControl=values[2]?
                {
                    dateTime = DateTime.UtcNow.Date;
                    fechaActual = dateTime.ToString("dd/MM/yyyy");
                    anioActual = dateTime.ToString("yy");
                    Fecha.SelectedDate = DateTime.Today;
                    guardarDocumento((numE) + ""); wordDocument = txtRutaDocumentoGenerar.Text + "\\" + numControl + ".doc";
                    mostrarDocumento(wordDocument);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Número de expediente no encontrado");
            }
        }
        /*
         * Actualizar los campos editados en la pantalla principal
         */
        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (txtPlantilla.Text != "" && 
                txtRutaDocumentoGenerar.Text != "" &&
                txtNumExpediente.Text!="") {
                dateTime = DateTime.UtcNow.Date;
                anioActual = Fecha.SelectedDate.ToString().Substring(8, 2);
                //Fecha.SelectedDate = DateTime.Today;
                fechaActual = Fecha.SelectedDate.ToString().Substring(0, 10);
                //hacer insert a alumno y a carta presentacion
                DBConnect db = new DBConnect();
                db.Update(//Actualizar
                    "Alumno",//Tabla
                    "nombre = '" + txtNombreAlumno.Text + "'," +
                    "carrera = '" + txtCarrera.Text + "'," +
                    "sexo = '" + cBoxSexo.Text + "'",
                    "numControl", "'" + numControl + "'"
                    );

                db.Update(//Actualizar
                    "Carta_Presentacion",//Tabla
                    "leyenda = '" + txtLeyenda.Text + "'," +
                    "nombreDependencia = '" + txtNombreDependencia.Text + "'," +
                    "direccionDependencia = '" + txtDireccion.Text + "'," +
                    "programa = '" + txtPrograma.Text + "'," +
                    "jefeDireccion = '" + txtNombreJefeDirecto.Text + "'," +
                    "puestoJefeDireccion = '" + txtPuesto.Text + "'",
                    "numControl", "'" + numControl + "'"
                    );
                guardarDocumento(txtNumExpediente.Text + "");
                wordDocument = txtRutaDocumentoGenerar.Text + "\\" + numControl + ".doc";
                mostrarDocumento(wordDocument);
            }else
            {
                System.Windows.MessageBox.Show("Uno o más parámetro no se han seleccionado");
            }
            
        }
        private void btnBuscarRutaDocumentoGenerar(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (folderBrowserDialog.SelectedPath.Length > 0)
                {
                    txtRutaDocumentoGenerar.Text = @""+folderBrowserDialog.SelectedPath;
                }
            }
        }
        /*
         * Validacion [Solo aceptar números].
         */
        private void txtNumExpediente_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtNumExpediente.Text, "[^0-9]"))
            {
                System.Windows.MessageBox.Show("Solo es posible ingresar números");
                txtNumExpediente.Text = txtNumExpediente.Text.Remove(txtNumExpediente.Text.Length - 1);
            }
        }
    }
}