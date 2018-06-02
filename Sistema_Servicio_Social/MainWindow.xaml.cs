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
            ContenedorPrincipal.Children.Add(new UserControl_Home());
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
            ContenedorPrincipal.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemHome":
                    usc = new UserControl_Home();
                    ContenedorPrincipal.Children.Add(usc);
                    break;
                case "ItemFile":
                    usc = new UserControl_LoadFileCSV();
                    ContenedorPrincipal.Children.Add(usc);
                    break;
                case "ItemBackupRestore":
                    usc = new UserControl_Backup();
                    ContenedorPrincipal.Children.Add(usc);
                    break;
                case "ItemFileAccount":
                    usc = new CartaPresentacion();
                    ContenedorPrincipal.Children.Add(usc);
                    break;
                default:
                    break;
            }
        }
    }
}