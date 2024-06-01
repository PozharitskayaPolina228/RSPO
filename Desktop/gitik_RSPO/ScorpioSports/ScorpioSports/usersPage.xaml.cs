﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace ScorpioSports
{
    /// <summary>
    /// Логика взаимодействия для usersPage.xaml
    /// </summary>
    public partial class usersPage : Page
    {
        private int? userID = null;
        private struct User{
            public string Name { get; set; }
            public string Contact { get; set; }
        }
        public usersPage()
        {
            InitializeComponent();
            var clients = ClientsTableHelper.getAllClients();
            foreach (var item in clients)
            {
                UsersListBox.Items.Add(new User {Name = $"{item.name.Trim()} {item.surname.Trim()}", Contact=CodeHelper.formatPhoneNumber(item.phoneNumber) });
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClientsTableHelper.deleteClient(CodeHelper.unformatPhoneNumber(((dynamic)UsersListBox.SelectedItem).Contact.ToString()));
                NavigationService.Navigate(new usersPage());
            }
            catch(Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            addClientWindow win = new addClientWindow();
            win.ShowDialog();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name, surname, email, phoneNumber;
                string[] names = this.nameTB.Text.Split(' '), dates = this.birthTB.Text.Split(new char[] { ' ', ',', '.','-','/','\\'});
                DateTime? date = null;


                if (names.Length < 2) throw new Exception("имя введено неверно. ПРИМЕР: Иванов Иван");
                if (Regex.IsMatch(birthTB.Text, @"(0[1-9]|1[0-9]|2[0-9]|3[0-1])[\s.,\\/-]{1}(0[1-9]|1[0-2])[\s.,\\/-]{1}(20[0-1]{1}[0-9]{1}|19\d{2})"))
                {
                    if (dates.Length < 3) throw new Exception("дата рождения введена неверно. ПРИМЕР: ДД:ММ:ГГГГ");
                }
                else throw new Exception("дата рождения введена неверно. ПРИМЕР: ДД:ММ:ГГГГ");


                try { date = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0])); }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                MessageBox.Show(date.ToString());

                if(userID != null)
                {
                    if (Regex.IsMatch(email = emailTB.Text, @".+@{1}(gmail|mail|yandex)\.(ru|com|us|en)"))
                    {
                        if (Regex.IsMatch(name = names[1], @"\w{2,}"))
                        {
                            if (Regex.IsMatch(surname = names[0], @"\w{2,}"))
                            {
                                if ((phoneNumber = CodeHelper.unformatPhoneNumber(phoneTB.Text)).Length == 12)
                                {
                                    if (date != null)
                                    {
                                        ClientsTableHelper.UpdateClient(userID ?? 0, names[1], names[0], date ?? DateTime.Now, email, phoneNumber);
                                        //ClientsTableHelper.AddClient(name, surname, date ?? DateTime.Now, email, phoneNumber);
                                        MessageBox.Show("Пользователь сохранен!");
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
                else throw new Exception("выберите пользователя!");
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                Client current = ClientsTableHelper.GetClientByPhoneNumber(CodeHelper.unformatPhoneNumber(((dynamic)UsersListBox.SelectedItem).Contact.ToString()));
                if (current != null)
                {
                    this.nameTB.Text = $"{current.name.Trim()} {current.surname.Trim()}";
                    this.birthTB.Text = $"{current.birthday:d}";
                    this.emailTB.Text = current.email.Trim();
                    this.phoneTB.Text = CodeHelper.formatPhoneNumber(current.phoneNumber);
                    this.registrationDateTB.Text = $"{current.registrationDate:d}";
                }
                else throw new Exception("null pointer error!");
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void UsersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string contact = CodeHelper.unformatPhoneNumber((((dynamic)((ListBox)sender).SelectedItem).Contact).ToString());
            try
            {
                Client current = ClientsTableHelper.GetClientByPhoneNumber(CodeHelper.unformatPhoneNumber(((dynamic)((ListBox)sender).SelectedItem).Contact.ToString()));
                if (current != null)
                {
                    this.nameTB.Text = $"{current.name.Trim()} {current.surname.Trim()}";
                    this.birthTB.Text = $"{current.birthday:d}";
                    this.emailTB.Text = current.email;
                    this.phoneTB.Text = CodeHelper.formatPhoneNumber(current.phoneNumber);
                    this.registrationDateTB.Text = $"{current.registrationDate:d}";
                    userID = current.id;
                }
                else throw new Exception("null pointer error!");
            }
            catch(Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        /*
        MessageBox.Show($"ИМЯ: {item.name},\n" +
        $"ФАМИЛИЯ: {item.surname}\n" +
        $"ДАТА РОЖДЕНИЯ: {item.birthday.Date}\n" +
        $"ЭЛ. ПОЧТА: {item.email}\n" +
        $"НОМЕР ТЕЛЕФОНА: {item.phoneNumber}\n" +
        $"ДАТА РЕГИСТРАЦИИ: {item.registrationDate.Date}\n", "ИНФОРМАЦИЯ ПОЛЬЗОВАТЕЛЯ", MessageBoxButton.OK);
        */
    }
}
