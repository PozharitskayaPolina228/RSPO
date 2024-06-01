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
    /// Логика взаимодействия для authPage.xaml
    /// </summary>
    public partial class authPage : Page
    {
        public authPage()
        {
            InitializeComponent();
            this.LoginTB.Text = "matvei2013";
            this.PasswordTB.Text = "qwerty123";
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = this.LoginTB.Text;
                string password = this.PasswordTB.Text;
                if (!(login == "" || password == "" || login == null || password == null))
                {
                    if (employeesTableHelper.IsLoginAndPasswordCorrect(login, password))
                    {
                        if (employeesTableHelper.getPostByLogin(login).Trim() == "Тренер")
                        {
                            NavigationService.Navigate(new mainPage(employeesTableHelper.getIdByLogin(login)));
                        }
                        else { MessageBox.Show("Тренер с соответствующими данными не найден!"); }
                    }
                    else MessageBox.Show("пароль или логин введены неправильно!");
                }
                else
                {
                    MessageBox.Show("введите ваши данные в поля!");
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.ToString()); }

        }

        private void adminButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new adimAuthPage());
        }
    }
}
