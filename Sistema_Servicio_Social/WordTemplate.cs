using System;
using Microsoft.Office.Interop.Word;

namespace Sistema_Servicio_Social
{
    class WordTemplate
    {
        //OBJECT OF MISSING "NULL VALUE"
        Object oMissing;//System.Reflection.Missing.Value;
        Object oTemplatePath;//= "C:\\Users\\Erik\\Documents\\CartaPresentacion.dotx";
        Application wordApp;// = new Application();
        Document wordDoc;//= new Document();

        public WordTemplate(String rutaDocumento)
        {
            oMissing = System.Reflection.Missing.Value;
            oTemplatePath = rutaDocumento;
            wordApp = new Application();
            wordDoc = new Document();
            wordDoc = wordApp.Documents.Add(ref oTemplatePath, ref oMissing, ref oMissing, ref oMissing);
        }
        public void reemplazarCampo(String campo, String texto)
        {

            foreach (Field myMergeField in wordDoc.Fields)
            {
                Range rngFieldCode = myMergeField.Code;
                String fieldText = rngFieldCode.Text;
                // ONLY GETTING THE MAILMERGE FIELDS
                if (fieldText.StartsWith(" MERGEFIELD"))
                {
                    // THE TEXT COMES IN THE FORMAT OF
                    // MERGEFIELD  MyFieldName  \\* MERGEFORMAT
                    // THIS HAS TO BE EDITED TO GET ONLY THE FIELDNAME "MyFieldName"
                    Int32 endMerge = fieldText.IndexOf("\\");
                    Int32 fieldNameLength = fieldText.Length - endMerge;

                    String fieldName = fieldText.Substring(11, endMerge - 11);

                    // GIVES THE FIELDNAMES AS THE USER HAD ENTERED IN .dot FILE
                    fieldName = fieldName.Trim();
                    // **** FIELD REPLACEMENT IMPLEMENTATION GOES HERE ****//
                    // THE PROGRAMMER CAN HAVE HIS OWN IMPLEMENTATIONS HERE
                    if (fieldName == campo)//if (fieldName == "Leyenda")
                    {
                        myMergeField.Select();
                        wordApp.Selection.TypeText(texto);//wordApp.Selection.TypeText("Soy leyenda");
                    }
                }
            }
        }
        public void guardarDocumento(String nombre)
        {
            wordDoc.SaveAs(nombre+".doc");//wordDoc.SaveAs("myfile.doc");                                   
            //wordApp.Documents.Open(nombre+".doc");
            wordApp.Application.Quit();
        }
        public void abrirDocumento(String nombre) {
            wordApp.Documents.Open(nombre);
            //wordApp.Application.Quit();
        }
        /*static void Main(string[] args) {
            WordTemplate wt = new WordTemplate("C:\\Users\\Erik\\Documents\\CartaPresentacion.dotx");
            wt.reemplazarCampo("Leyenda","Soy leyenda");
            wt.guardarDocumento("Hola.doc");
        }*/
    }
}