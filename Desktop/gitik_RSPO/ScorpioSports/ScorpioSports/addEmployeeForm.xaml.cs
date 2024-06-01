using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для addEmployeeForm.xaml
    /// </summary>
    public partial class addEmployeeForm : Window
    {
        public addEmployeeForm()
        {
            InitializeComponent();
        }

        private void confirmButton_click(object sender, RoutedEventArgs e)
        {
            /*
            try
            {
                string name, surname, email, phoneNumber;
                DateTime? date;
                if (Regex.IsMatch(email = emailTB.Text, @".+@{1}(gmail|mail|yandex)\.(ru|com|us|en)"))
                {
                    if (Regex.IsMatch(name = nameTB.Text, @"\w{2,}"))
                    {
                        if (Regex.IsMatch(surname = surnameTB.Text, @"\w{2,}"))
                        {
                            if ((phoneNumber = phoneNumberTB.Text).Length == 12)
                            {
                                if ((date = dateSelector.SelectedDate).HasValue)
                                {
                                    if (!ClientsTableHelper.CheckIfExists(email, phoneNumber))
                                    {
                                        ClientsTableHelper.AddClient(name, surname, date ?? DateTime.Now, email, phoneNumber);
                                        MessageBox.Show("Клиент успешно добавлен");
                                    }
                                    else throw new Exception("Номер телефона или почта уже заняты!");
                                }
                                else throw new Exception("выберите дату рождения!");
                            }
                            else throw new Exception("Номер телефона введен неверно!");
                        }
                        else throw new Exception("Фамилия введена неверно!");
                    }
                    else throw new Exception("Имя введено неверно!");
                }
                else throw new Exception("Почта введена неверно!");
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            */
            try
            {
                string name, surname, post = (postCB.SelectedItem as ComboBoxItem).Content.ToString() ?? "Администратор", phoneNumber, login, password = passwordTB.Text;
                DateTime? date;
                double salary = 0;
                try { salary = double.Parse(salaryTB.Text); } catch (Exception) { MessageBox.Show("Зарплата введена некорректно!"); return; }
                if (Regex.IsMatch(name = nameTB.Text, @"\w{2,}"))
                {
                    if (Regex.IsMatch(surname = surnameTB.Text, @"\w{2,}"))
                    {
                        if ((phoneNumber = phoneNumberTB.Text).Length == 12)
                        {
                            if(Regex.IsMatch(login = loginTB.Text.ToLower(), @"^\w+$"))
                            {
                                if ((date = dateSelector.SelectedDate).HasValue)
                                {
                                    if(!employeesTableHelper.checkLoginAndPhoneNumber(login, phoneNumber))
                                    {
                                        employeesTableHelper.addEmployee(name, surname, post, date, phoneNumber, salary, login, password);
                                        MessageBox.Show("Сотрудник успешно добавлен");
                                    }
                                    else throw new Exception("Логин или номер телефона уже заняты!");
                                }
                                else throw new Exception("выберите дату рождения!");
                            }
                            else throw new Exception("Логин введен неверно!");
                        }
                        else throw new Exception("Номер телефона введен неверно!");
                    }
                    else throw new Exception("Фамилия введена неверно!");
                }
                else throw new Exception("Имя введено неверно!");
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
    }
}
