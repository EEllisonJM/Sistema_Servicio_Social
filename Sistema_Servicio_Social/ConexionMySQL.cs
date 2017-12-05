using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Text;
namespace Sistema_Servicio_Social
{
    class ConexionMySQL
    {
        /*Agregar en la base de datos*/
        public void leerCSV(string ruta, int expedienteI, String leyenda)
        {
            int numExp = expedienteI;
            foreach (string line in File.ReadLines(@"" + ruta))
            {
                String[] values = line.Split(',');
                if (values[0] != "\"Marca temporal\"")
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
                    if (db.CountOne(//Existe?
                        "Alumno",//Table
                        "numControl", values[2]) == 1)//numControl=values[2]?
                    {
                        db.Update(//Actualiza
                            "Alumno",//Tabla
                            "nombre = '" + values[5] + " " + values[3] + " " + values[4] + "'," +//nombre
                            "carrera = '" + values[7] + "'," +//carrera
                            "sexo = '" + values[6] + "'," +//sexo
                            "e_mail = '" + values[1] + "'," +//e-mail
                            "porcentajeAvance = " + values[8] + "," +//porcentaje de avance
                            "semestre = " + values[9],//Semestre
                            "numControl", values[2]);//Numero de control
                    }
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
                    }
                    if (db.CountOne(//Existe?
                        "Carta_Presentacion",//Tabla
                        "numControl", values[2]) == 1)//numControl=values[2]?
                    {
                        db.Update(//Actualiza
                            "Carta_Presentacion",//Tabla
                            "nombreDependencia = '" + values[10] + "'," +
                            "direccionDependencia = '" + values[12] + "'," +
                            "programa = '" + values[11] + "'," +
                            "nombreDependencia = '" + values[10] + "'," +
                            "jefeDireccion= '" + values[13] + " " + values[14] + " " + values[15] + " " + values[16] + "'," +
                            "leyenda = '" + leyenda + "'",//"leyenda = 'Soy leyenda' ",
                            "numControl", "" + values[2] + ""//Donde => numExpediente=numExp
                            );
                    }
                    else
                    {
                        db.Insert(//Insertar
                            "Carta_Presentacion",//Tabla
                            "(numExpediente,numControl,nombreDependencia,direccionDependencia,programa,jefeDireccion,puestoJefeDireccion,leyenda)",//Atributos
                            "(" +//Valores...
                            numExp + "," +//numExpediente
                            values[2] + ",'" +//numControl
                            values[10] + "','" +//nombreDependencia
                            values[12] + "','" +//direccionDependencia
                            values[11] + "','" +//programa
                            values[13] + ' ' + values[14] + ' ' + values[15] + ' ' + values[16] + "','" +//jefeDireccion
                            values[17] + "','"+leyenda+"')"//puesto y leyenda
                            );
                        numExp++;
                    }
                }
            }
        }
    }
}