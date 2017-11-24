/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
*/
using System.Windows;
namespace Sistema_Servicio_Social
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        int numIntentos = 0;
        public Login()
        {
            InitializeComponent();
        }

        private void btnIniciarSesion(object sender, RoutedEventArgs e)
        {
            if (numIntentos == 3)
            {
                MessageBox.Show("Has excedido el número de intentos permitidos, el sistema se cerrará");
                this.Close();
            }
            else {
                numIntentos += 1;
                DBConnect db = new DBConnect();
                if (db.Count(
                    "usuario", "nombre", "'" + this.txtUser.Text + "'", "password", "'" + this.txtPassword.Password + "'") == 1)
                {
                    //if (db.Count(this.txtUser.Text, this.txtPassword.Password) == 1){
                    MessageBox.Show("Encontrado");
                    Interfaz i = new Interfaz();
                    i.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Usuario no encontrado o contraseña incorrecta");
                    this.txtUser.Text = "";
                    this.txtPassword.Password = "";
                }
            }
        }
    }
}