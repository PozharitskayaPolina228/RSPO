using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScorpioSports
{
    /// <summary>
    /// Логика взаимодействия для adminMainPage.xaml
    /// </summary>
    public partial class adminMainPage : Page
    {
        private int adminID;
        public adminMainPage(int adminID)
        {
            InitializeComponent();
            this.adminID = adminID;
            this.mainFrame.Navigate(new employeesPage());
        }

        private void profile_Click(object sender, RoutedEventArgs e)
        {
            couchProfilePage win = new couchProfilePage(adminID);
            win.ShowDialog();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new adminMainPage(this.adminID));
        }

        private void couch_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new employeesPage());
        }

        private void clients_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new usersPage());
        }
    }
}
