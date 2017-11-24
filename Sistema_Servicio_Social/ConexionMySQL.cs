using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Text;
namespace Sistema_Servicio_Social
{
    class ConexionMySQL
    {
        /*
        String servidor = "localhost";
        String usuario = "root";
        String contrasenia = "";
        String baseDeDatos = "Servicio_Social";

        string parametrosServidor()
        {
            return "server =" + servidor + ";" +
                   "user id =" + usuario + ";" +
                   "password = " + contrasenia + ";" +
                   "database = " + baseDeDatos;
        }
        void consultaInsert(String sql)
        {
            String conexionMySQL = parametrosServidor();
            MySqlConnection conn = new MySqlConnection(conexionMySQL);
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();//Cerrar la conexion con el servidor
            }
        }
        //----
        void consultaSelect(String servidor, String idUser, String password, String dataBase, String sql)
        {
            String conexionMySQL = parametrosServidor();
            MySqlConnection conn = new MySqlConnection(conexionMySQL);
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            try
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader["numControl"].ToString());
                    Console.WriteLine(reader["nombre"].ToString());
                    Console.WriteLine(reader["sexo"].ToString());
                    Console.WriteLine(reader["e_mail"].ToString());
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();//Cerrar la conexion con el servidor
            }
        }*/
        public void leerCSV(string ruta, int expedienteI)
        {
            int numExp = expedienteI;
            foreach (string line in File.ReadLines(@"" + ruta))
            {
                String[] values = line.Split(',');
                if (values[0] != "\"Marca temporal\"")
                {
                    //Eliminar las " de los campos
                    for (int i = 0; i <= 17; i++)
                    {
                        values[i] = values[i].ToString().Replace('"', ' ').Trim();
                    }
                    /*Agregar en la base de datos*/
                    //Si en el formulario dice Hombre cambiar por H y si dice Mujer cambiar por M
                    if (values[6] == "Hombre")
                    {
                        values[6] = "H";
                    }
                    else
                    {
                        values[6] = "F";
                    }
                    /*Ir guardando en la base de datos*/
                    DBConnect db = new DBConnect();
                    if (db.CountOne("Alumno", "numControl", values[2]) == 1)
                    {//existe
                        //db.Delete("Alumno", "numControl", values[2]);
                        db.Update("Alumno",
                            "nombre = '" + values[5] + " " + values[4] + " " + values[3] + "'," +//nombre                           
                            "carrera = '" + values[7] + "'," +//carrera
                            "sexo = '" + values[6] + "'," +//sexo
                            "e_mail = '" + values[1] + "'," +//e-mail
                            "porcentajeAvance = " + values[8] + "," +//porcentaje de avance
                            "semestre = " + values[9],//Semestre
                            "numControl", values[2]);
                    }
                    else
                    {
                        db.Insert("Alumno",
                        "(numControl,nombre,carrera,sexo,e_mail,porcentajeAvance,semestre)", "(" +
                        values[2] + "," +//numControl
                        " '" + values[5] + " " + values[4] + " " + values[3] + "'," +//nombre
                        " '" + values[7] + "'," +//carrera
                        " '" + values[6] + "'," +//sexo
                        " '" + values[1] + "'," +//e-mail
                        values[8] + "," +//porcentaje de avance
                        values[9] + ")"//Semestre
                        );
                    }
                    if (db.CountOne("Carta_Presentacion", "numControl", values[2]) == 1)
                    {//existe
                        numExp--;
                        //Update(tabla,atributosValores,atributo,string valor)
                        db.Update(
                            "Carta_Presentacion",//Actualiza la Tabla
                            "nombreDependencia = '" + values[10] + "'," +
                            "direccionDependencia = '" + values[12] + "'," +
                            "programa = '" + values[11] + "'," +
                            "nombreDependencia = '" + values[10] + "'," +
                            "jefeDireccion= '" + values[13] + " " + values[14] + " " + values[15] + " " + values[16] + "'," +
                            "leyenda = 'Soy leyenda' ",

                            "numControl", "" + values[2] + ""//Donde => numExpediente=numExp
                            );
                    }
                    else
                    {
                        db.Insert(
                            "Carta_Presentacion",
                            "(numExpediente,numControl,nombreDependencia,direccionDependencia,programa,jefeDireccion,puestoJefeDireccion,leyenda)", "(" +
                            numExp + "," +//numExpediente
                            values[2] + ",'" +//numControl
                            values[10] + "','" +//nombreDependencia
                            values[12] + "','" +//direccionDependencia
                            values[11] + "','" +//programa
                            values[13] + ' ' + values[14] + ' ' + values[15] + ' ' + values[16] + "','" +//jefeDireccion
                            values[17] + "','Esta es mi leyenda...')"//puesto y leyenda


                            );
                        numExp++;
                    }
                }
            }
            //Console.ReadKey(true); //Deja quieta la consola por si queremos revisar los mensajes de error (espera a que se presione una tecla)
        }
    }
}