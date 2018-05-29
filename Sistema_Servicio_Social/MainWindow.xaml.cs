using System.Windows;
using System.Windows.Controls;

namespace Sistema_Servicio_Social
{
    /*[Window Class]
     * Provides the ability to create, configure, show, and 
     * manage the lifetime of windows and dialog boxes.*/
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //usuario.Text = nombreUsuario.ToUpper();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }
        private void click_BtnSalir(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemHome":
                    usc = new UserControl_Home();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemFile":
                    usc = new UserControl_LoadFileCSV();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemBackupRestore":
                    usc = new UserControl_Backup();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemFileAccount":
                    usc = new CartaPresentacion();
                    GridMain.Children.Add(usc);
                    break;
                default:
                    break;
            }
        }
    }
}