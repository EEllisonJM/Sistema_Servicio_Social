using System;
using Microsoft.Office.Interop.Word;

namespace Sistema_Servicio_Social
{
    class WordTemplate
    {
        Object oMissing;
        Object oTemplatePath;//Ruta Plantilla
        Application wordApp;
        Document wordDoc;
        String fieldText;//Campo de texto en plantilla [Mergefield]
        public WordTemplate(String rutaPlantilla)
        {
            oMissing = System.Reflection.Missing.Value;
            oTemplatePath = rutaPlantilla;
            wordApp = new Application();
            wordDoc = new Document();
            wordDoc = wordApp.Documents.Add(ref oTemplatePath, ref oMissing, ref oMissing, ref oMissing);
        }
        public void reemplazarCampo(String campo, String texto)
        {
            foreach (Field myMergeField in wordDoc.Fields)
            {
                Range rngFieldCode = myMergeField.Code;
                fieldText = rngFieldCode.Text;                
                if (fieldText.StartsWith(" MERGEFIELD"))//encuentra campo [MERGEFIELD]
                {
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
            wordDoc.SaveAs(nombre + ".doc");//Documento con formato incluido
            //wordApp.Documents.Open(nombre+".doc");
            wordApp.Application.Quit();
        }
        public void abrirDocumento(String nombre)
        {
            wordApp.Documents.Open(nombre);
            //wordApp.Application.Quit();
        }
    }
}