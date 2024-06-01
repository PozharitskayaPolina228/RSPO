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
using System.Windows.Shapes;

namespace ScorpioSports
{
    /// <summary>
    /// Логика взаимодействия для couchProfilePage.xaml
    /// </summary>
    public partial class couchProfilePage : Window
    {
        Couch current = null;
        public couchProfilePage(int id)
        {
            InitializeComponent();
            current = employeesTableHelper.getCouchByID(id);
            this.fioTB.Text = $"{current.name.Trim()} {current.surname.Trim()}";
            this.postTB.Text = current.post.Trim();
            this.loginTB.Text = current.login.Trim();
            this.dateTB.Text = $"{current.registrationDate:d}";
            this.contactsTB.Text = CodeHelper.formatPhoneNumber(current.contacts.Trim());
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
