using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Sistema_Servicio_Social
{
    class ConexionMySQL
    {
        /*Agregar en la base de datos*/
        public void leerCSV(string ruta, int expedienteI, int anio, String leyenda)
        {
            int numExp = expedienteI;
            foreach (string line in File.ReadLines(@"" + ruta))
            {
                String[] values = line.Split(',');
                if (values[0] != "Marca temporal")
                {
                    //Eliminar las " de los campos y cambiar los datos a Mayúsculas
                    for (int i = 0; i <= 17; i++)
                    {
                        values[i] = values[i].ToString().Replace('"', ' ').Trim().ToUpper();
                    }
                    //Si en el formulario dice Hombre cambiar por H y si dice Mujer cambiar por M
                    if (values[6] == "HOMBRE")
                    {
                        values[6] = "H";
                    }
                    else
                    {
                        values[6] = "M";
                    }
                    DBConnect db = new DBConnect();
                    /*======ALUMNO EXISTE===========================================*/
                    if (db.CountOne(/*Existe alumno? */
                        "Alumno",//Table
                        "numControl", values[2]) == 1)//numControl=values[2]?
                    {
                        DialogResult result = MessageBox.Show(
                            "El número de control que intenta guardar ya se encuentra en la base de datos, ¿Desea actualizar sus datos?",
                            "Número de control",
                            MessageBoxButtons.YesNoCancel);
                        if (result == DialogResult.Yes)
                        {
                            db.Update(//Actualiza
                            "Alumno",//Tabla
                            /*SET*/
                            "nombre = '" + values[5] + " " + values[3] + " " + values[4] + "'," +//nombre
                            "carrera = '" + values[7] + "'," +//carrera
                            "sexo = '" + values[6] + "'," +//sexo
                            "e_mail = '" + values[1] + "'," +//e-mail
                            "porcentajeAvance = " + values[8] + "," +//porcentaje de avance
                            "semestre = " + values[9],//Semestre
                            /*WHERE*/
                            "numControl", values[2]);//Numero de control

                        }
                        else if (result == DialogResult.No)
                        {
                            //No hacer nada
                        }
                    }
                    /*======ALUMNO NO EXISTE===========================================*/
                    else
                    {
                        db.Insert(//Insertar
                            "Alumno",//Tabla
                        "(numControl,nombre,carrera,sexo,e_mail,porcentajeAvance,semestre)",//Atributos
                        "(" +//Valores...
                        values[2] + "," +//numControl
                        " '" + values[5] + " " + values[3] + " " + values[4] + "'," +//nombre
                        " '" + values[7] + "'," +//carrera
                        " '" + values[6] + "'," +//sexo
                        " '" + values[1] + "'," +//e-mail
                        values[8] + "," +//porcentaje de avance
                        values[9] + ")"//Semestre
                        );

                        db.Insert(//Insertar
                            "Carta_Presentacion",//Tabla
                            "(numExpediente,anio,numControl,nombreDependencia,direccionDependencia,programa,jefeDireccion,puestoJefeDireccion,leyenda)",//Atributos
                            "(" +//Valores...
                            numExp + "," +//numExpediente
                            anio + "," +//Año
                            values[2] + ",'" +//numControl
                            values[10] + "','" +//nombreDependencia
                            values[12] + "','" +//direccionDependencia
                            values[11] + "','" +//programa
                            values[13] + ' ' + values[14] + ' ' + values[15] + ' ' + values[16] + "','" +//jefeDireccion
                            values[17] + "','" + leyenda + "')"//puesto y leyenda
                            );
                        numExp++;
                    }
                }
            }//Fin foreach          
        }//Fin metodo leerCSV
    }
}