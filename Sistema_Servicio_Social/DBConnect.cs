﻿using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Windows;
namespace Sistema_Servicio_Social
{
    class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string port;
        //Constructor
        public DBConnect()
        {
            Initialize();
        }        
        private void Initialize()//Inicializar valores
        {
            server = "localhost";
            database = "servicio_social";
            uid = "root";
            port = "3306";
            password = "1234";
            string connectionString;
            connectionString = "PORT=" + port + ";" + "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }
        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;
                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }
        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        //Insert statement
        public void Insert(string tabla,string atributos,string valores)
        {//"INSERT INTO usuario (nombre,password) VALUES('juan', '12345');";
            //tabla => nombre de la tabla
            //atributos => Separado por coma => (nombre,password)
            //valores => los valores a insertar en la tabla => ('jaime',121)            
            string query = "INSERT INTO " + tabla + " " + atributos + " " +
                " VALUES " + valores + " ;";
            //open connection
            if (this.OpenConnection() == true)
            {//create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Execute command
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }
        //Update statement
        public void Update(string tabla,string atributosValores,string atributo,string valor)
        {//"UPDATE Usuario SET nombre='Joe'WHERE name='John'";
            // => nombre='Juan' , password='123'
            string query =
                "UPDATE " + tabla + " SET " + atributosValores +
                " WHERE " + atributo + " = " + valor+ " ;";
            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;
                //Execute query
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }
        public void Delete(string table,string atributo,string value)
        {//"DELETE FROM tableinfo WHERE name='John Smith'";
            string query =
                "DELETE FROM " + table +
                " WHERE " + atributo + " = " + value + " ;";
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }
        //Count statement
        public int Count(string table,string atributo1,string value1, string atributo2,string value2)
        {//string query = "SELECT Count(*) FROM usuario;";
            string query =
                "SELECT Count(*) FROM " + table +
                " WHERE " + atributo1 + " = " + value1 +
                " and " + atributo2 + " = "+ value2 + " ;";
            //SELECT Count(*) FROM usuario where nombre='John Smith' and password='33';
            int Count = -1;
            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //ExecuteScalar will return one value                
                Count = int.Parse(cmd.ExecuteScalar() + "");
                //close Connection
                this.CloseConnection();
                //Return value => -1 (Not exist) => 1 (Exist)
                return Count;
            }
            else
            {
                return Count;
            }
        }
        public int CountOne(string table, string atributo1, string value1)
        {//string query = "SELECT Count(*) FROM usuario;";
            string query =
                "SELECT Count(*) FROM " + table +
                " WHERE " + atributo1 + " = " + value1+" ;";
            //SELECT Count(*) FROM usuario where nombre='John Smith' and password='33';
            int Count = -1;
            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //ExecuteScalar will return one value                
                Count = int.Parse(cmd.ExecuteScalar() + "");
                //close Connection
                this.CloseConnection();
                //Return value => -1 (Not exist) => 1 (Exist)
                return Count;
            }
            else
            {
                return Count;
            }
        }
        public int CountOne(string table, string atributo1, string value1, string atributo2, string value2)
        {//string query = "SELECT Count(*) FROM usuario;";
            string query =
                "SELECT Count(*) FROM " + table +
                " WHERE " + atributo1 + " = " + value1 +
                " AND " + atributo2 + " = " + value2 + " ;";
            //SELECT Count(*) FROM usuario where nombre='John Smith' and password='33';
            int Count = -1;
            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //ExecuteScalar will return one value                
                Count = int.Parse(cmd.ExecuteScalar() + "");
                //close Connection
                this.CloseConnection();
                //Return value => -1 (Not exist) => 1 (Exist)
                return Count;
            }
            else
            {
                return Count;
            }
        }
        //Select statement
        public List<string> Select(string numExpediente, string anio)
        {
            string query = "SELECT " +
                "A.numControl, A.nombre, A.carrera, A.sexo, A.e_mail,"+
                "CP.leyenda, CP.programa, CP.numExpediente, CP.jefeDireccion, CP.puestoJefeDireccion, CP.direccionDependencia, CP.nombreDependencia, CP.anio "+
                "FROM alumno as A INNER JOIN carta_presentacion as CP "+
                "ON A.numControl= CP.numControl "+
                "WHERE CP.numExpediente ="+numExpediente+
                " AND CP.anio ="+anio+";";
            //Create a list to store the result
            List<string> list = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list.Add(dataReader["numControl"] + "");//0
                    list.Add(dataReader["nombre"] + "");//1
                    list.Add(dataReader["carrera"] + "");//2
                    list.Add(dataReader["sexo"] + "");//3
                    list.Add(dataReader["e_mail"] + "");//4

                    list.Add(dataReader["leyenda"] + "");//5
                    list.Add(dataReader["programa"] + "");//6
                    list.Add(dataReader["numExpediente"] + "");//7
                    list.Add(dataReader["jefeDireccion"] + "");//8
                    list.Add(dataReader["puestoJefeDireccion"] + "");//9
                    list.Add(dataReader["direccionDependencia"] + "");//10 
                    list.Add(dataReader["nombreDependencia"] + "");//11

                    list.Add(dataReader["anio"] + "");//12
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        /*//Backup
        public void Backup()
        {
        }

        //Restore
        public void Restore()
        {
        }*/
    }
}