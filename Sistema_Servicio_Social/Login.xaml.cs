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
                if (db.Count("usuario",//Tabla
                    "nombre", "'" + this.txtUser.Text + "'",//Nombre
                    "password", "'" + this.txtPassword.Password + "'"//Contrasenia
                    ) == 1)//Existe?
                {
                    MessageBox.Show("Bienvenid@ al sistema");
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