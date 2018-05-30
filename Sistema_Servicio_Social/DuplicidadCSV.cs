using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Servicio_Social
{
    class DuplicidadCSV
    {
        public void analizarCSV(String ruta)
        {
            bool duplicidad = false;
            //Cargando el contenido del CSV en un arreglo de listas
            List<String>[] lista = new List<String>[18];
            for (int i = 0; i < 18; i++)
            {
                lista[i] = new List<String>();
            }
            foreach (string line in File.ReadLines(@"" + ruta))
            {
                String[] values = line.Split(',');
                for (int j = 0; j < 18; j++)
                {
                    lista[j].Add(values[j]);
                }
            }
            //Analizando archivo CSV y eliminando duplicidad
            int alumnos = lista[2].Count;
            for (int i = 0; i < alumnos; i++)
            {
                for (int j = i+1; j < alumnos; j++)
                {
                    //Mientras el numero de control en i sea igual que en j
                    while (j<alumnos && lista[2][i].CompareTo(lista[2][j])==0)
                    {
                        duplicidad = true;
                        //Eliminando la fila i (en un CSV la fila j es más reciente que la i)
                        for (int columnaCSV = 0; columnaCSV < 18; columnaCSV++)
                        {
                            lista[columnaCSV].RemoveAt(i);
                        }
                        alumnos = lista[2].Count;
                        j = i + 1;
                    }
                }
            }
            //Si hubo duplicidad muestra un mensaje de confirmación para actualizar el archivo CSV
            if (duplicidad)
            {
                MessageBoxResult result = MessageBox.Show("Hay alumnos que enviaron sus datos más de una vez, el archivo conservará el último registro enviado por cada alumno.\n\n¿Desea actualizar el archivo CSV?", "Mensaje de confirmación", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    actualizarCSV(lista, ruta);
                }
            }
            else
            {
                MessageBox.Show("No hay alumnos repetidos en el archivo CSV");
            }
        }

        private void actualizarCSV(List<String>[] lista, String ruta)
        {
            //Creando un archivo temporal
            using (StreamWriter file = new StreamWriter(@"" + ruta+".temp.csv", true))
            {
                //Escribiendo contenido del buffer en el archivo temporal
                for (int fila = 0; fila < lista[0].Count; fila++)
                {
                    String linea = lista[0][fila];
                    for(int columnaCSV = 1; columnaCSV < 18; columnaCSV++)
                    {
                        linea += ("," + lista[columnaCSV][fila]);
                    }
                    file.WriteLine(linea);
                }
                file.Close();
            }
            try
            {
                //Actualizando archivo CSV
                File.Delete(@"" + ruta);
                File.Move(@"" + ruta + ".temp.csv", @"" + ruta);
                MessageBox.Show("La duplicidad ha sido corregida");
            } catch(Exception e)
            {
                MessageBox.Show("El archivo no pudo ser actualizado, verifique que otro programa no lo esté utilizando");
            }
        }
    }
}