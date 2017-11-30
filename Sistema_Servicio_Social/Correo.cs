using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Sistema_Servicio_Social
{
    class Correo
    {
        public void EnviarCorreo(string ruta, string nombre, string asunto, string mensaje,string e_mail)
        {
            try
            {
                // Create the Outlook application by using inline initialization.
                Outlook.Application oApp = new Outlook.Application();

                //Create the new message by using the simplest approach.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

                //Add a recipient.
                // TODO: Change the following recipient where appropriate.
                Outlook.Recipient oRecip = (Outlook.Recipient)oMsg.Recipients.Add(e_mail);
                oRecip.Resolve();

                //Set the basic properties.
                oMsg.Subject = asunto;
                oMsg.Body = mensaje;

                //Add an attachment.
                // TODO: change file path where appropriate
                String sSource = ruta;//"C:\\Users\\RICHARD\\Desktop\\ito\\CARRERAS\\ELECTRICA\\Objetivo IELE-2010-209.pdf";
                String sDisplayName = nombre;//"MyFirstAttachment.pdf";
                int iPosition = (int)oMsg.Body.Length + 1;
                int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                Outlook.Attachment oAttach = oMsg.Attachments.Add(sSource, iAttachType, iPosition, sDisplayName);

                // If you want to, display the message.
                // oMsg.Display(true);  //modal

                //Send the message.
                oMsg.Save();
                oMsg.Send();

                //Explicitly release objects.
                oRecip = null;
                oAttach = null;
                oMsg = null;
                oApp = null;
            }

            // Simple error handler.
            catch (Exception e)
            {
                //Console.WriteLine("{0} Exception caught: ", e);
                MessageBox.Show("{0} Exception caught: ");
            }
            //Default return value.
            //return 0;
        }
    }
}
