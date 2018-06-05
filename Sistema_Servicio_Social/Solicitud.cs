using System;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace Sistema_Servicio_Social
{
    class Solicitud
    {
        public static List<string> get(string sURL)
        {
            List<string> lista = new List<string>();
            try
            {
                WebRequest wrGETURL;
                wrGETURL = WebRequest.Create(sURL);

                WebProxy myProxy = new WebProxy("myproxy", 80);
                myProxy.BypassProxyOnLocal = true;

                wrGETURL.Proxy = WebProxy.GetDefaultProxy();

                Stream objStream;
                objStream = wrGETURL.GetResponse().GetResponseStream();

                StreamReader objReader = new StreamReader(objStream);

                string sLine = "";
                int i = 0;

                while (sLine != null)
                {
                    i++;
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                        //Console.WriteLine("{0}:{1}", i, sLine);
                        lista.Add(sLine);
                }
                Console.WriteLine("Descargado con éxito");
            } catch (System.Net.WebException e)
            {
                Console.WriteLine("No se pudo obtener el archivo");
            }
            return lista;
        }

        public static void guardarCSV(string ruta, List<string> renglones)
        {
            using (StreamWriter sw = File.AppendText(ruta))         //se crea el archivo
            {
                for (int i=0;i<renglones.Count;i++)
                {
                    sw.WriteLine(renglones[i]);
                }
                sw.Close();
            }
        }
    }
}
