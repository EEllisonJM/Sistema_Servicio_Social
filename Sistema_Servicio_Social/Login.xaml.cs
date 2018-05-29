using System.Windows;
namespace Sistema_Servicio_Social
{
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
            else
            {
                numIntentos += 1;
                DBConnect db = new DBConnect();
                if (db.Count(/*Existe?*/
                    "usuario",
                    "nombre", "'" + this.txtUser.Text + "'",
                    "password", "'" + this.txtPassword.Password + "'"
                    ) == 1)
                {/*TRUE*/
                    MainWindow i = new MainWindow();
                    i.Show();
                    this.Close();
                }
                else
                {/*FALSE*/
                    MessageBox.Show("Usuario no encontrado o contraseña incorrecta, verifique.");
                    this.txtUser.Text = "";
                    this.txtPassword.Password = "";
                }
            }
        }
    }
}