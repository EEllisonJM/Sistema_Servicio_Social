using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Text;
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
                if (values[0] != "\"Marca temporal\"")/*Nombre columna[1] archivo*/
                {
                    //Eliminar las comillas["] de los campos y cambiar los datos a Mayúsculas
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
                    /*ALUMNO EXISTE, ACTUALIZAR DATOS*/
                    if (db.CountOne(//Existe?
                        "alumno",//Table
                        "numControl", values[2]) == 1)//numControl=values[2]?
                    {
                        db.Update(//Actualiza
                            "alumno",//Tabla
                            "nombre = '" + values[5] + " " + values[3] + " " + values[4] + "'," +//nombre
                            "carrera = '" + values[7] + "'," +//carrera
                            "sexo = '" + values[6] + "'," +//sexo
                            "e_mail = '" + values[1] + "'," +//e-mail
                            "porcentajeAvance = " + values[8] + "," +//porcentaje de avance
                            "semestre = " + values[9],//Semestre
                            "numControl", values[2]);//Numero de control

                        if (db.Count(//Existe?
                        "carta_presentacion",//Tabla
                        "numControl", values[2],
                        "anio", anio + "") == 1)//numControl=values[2]?
                        {
                            System.Windows.MessageBox.Show(
                                "El número de control: " + values[2] + ", ya se encentra registrado en el sistema con los campos establecidos previamente.");
                        }
                    }
                    else
                    {/*ALUMNO NO EXISTE, INSERTAR DATOS*/
                        db.Insert(//Insertar
                            "alumno",//Tabla
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
                        /*CREAR CARTA PRESENTACION*/
                        db.Insert(//Insertar
                            "carta_presentacion",//Tabla
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
            }
        }
    }
}