using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema_Servicio_Social
{
     class ConexionMySQL
     {
          String servidor;
          String usuario;
          String contrasenia;
          String baseDeDatos;

          void consultaInsert(String servidor, String idUser, String password, String dataBase, String sql)
          {
               String conexionMySQL = //server = localhost; user id = root; password =; database = Servicio_Social
                   "server =" + servidor + ";" +
                   "user id =" + idUser + ";" +
                   "password = " + password + ";" +
                   "database = " + dataBase;

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
               String conexionMySQL = //server = localhost; user id = root; password =; database = Servicio_Social
                   "server =" + servidor + ";" +
                   "user id =" + idUser + ";" +
                   "password = " + password + ";" +
                   "database = " + dataBase;
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
                    //cmd.ExecuteNonQuery();//No neesario, por que ya esta esto : MySqlDataReader reader = cmd.ExecuteReader();
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
          public void leerCSV(string ruta, int expedienteI)
          {
               int numExp = expedienteI;
               foreach (string line in File.ReadLines(@""+ruta))
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
                         //Console.WriteLine(values[0] + " - " + values[1] + " - " + values[2] + " - " + values[3]);
                         //---------
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
                         consultaInsert(//server = localhost; user id = root; password =; database = Servicio_Social
                             "localhost",
                             "root",
                             "",
                             "Servicio_Social",
                             "INSERT INTO Alumno (numControl,nombre,carrera,sexo,e_mail,porcentajeAvance,semestre) value " +
                             "(" +
                             values[2] + ",'" +//numControl
                             values[3] + ' ' + values[4] + ' ' + values[5] + "','" +//nombre
                             values[7] + "','" +//carrera
                             values[6] + "','" +//sexo
                             values[1] + "','" +//e-mail
                             values[8] + "','" +//porcentaje de avance
                             values[9] + "')"//semestre
                         );
                         //---------
                         consultaInsert(//server = localhost; user id = root; password =; database = Servicio_Social
                             "localhost",
                             "root",
                             "",
                             "Servicio_Social",
                             "INSERT INTO Carta_Presentacion (numExpediente,numControl,nombreDependencia,direccionDependencia,programa,jefeDireccion,puestoJefeDireccion,leyenda) value " +
                             "(" +
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
               //Console.ReadKey(true); //Deja quieta la consola por si queremos revisar los mensajes de error (espera a que se presione una tecla)
          }
          //----
          //static void Main(string[] args)
          //{
          //     ConexionMySQL conexionMySQL = new ConexionMySQL();
          //     conexionMySQL.leerCSV();
          //}

     }
}