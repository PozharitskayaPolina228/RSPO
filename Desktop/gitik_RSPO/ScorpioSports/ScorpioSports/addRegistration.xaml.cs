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
    /// Логика взаимодействия для addRegistration.xaml
    /// </summary>
    public partial class addRegistration : Window
    {
        private int? clientID = null;
        private List<Client> clients = ClientsTableHelper.getAllClients();
        private int couchId;
        public addRegistration(int id)
        {
            InitializeComponent();
            foreach(Client item in clients)
            {
                this.clientListBox.Items.Add($"{item.surname.Trim()} {item.name.Trim()}");
            }
            couchId = id;
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(this.dateSelector.Text == "" || this.timeStart.Text==null || this.timeEnd.Text == null)
                {
                    throw new Exception("неккоретный ввод!");
                }
                else if (clientID == null)
                {
                    throw new Exception("выберите пользователя на запись!");
                }
                else
                {
                    //
                    string[] dates = this.dateSelector.Text.Split('.');
                    DateTime date = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]));
                    //
                    TimeSpan start = new TimeSpan(int.Parse(timeStart.Text.Substring(0, 2)), int.Parse(timeStart.Text.Substring(3, 2)), 0);
                    TimeSpan end = new TimeSpan(int.Parse(timeEnd.Text.Substring(0, 2)), int.Parse(timeEnd.Text.Substring(3, 2)), 0);


                    //
                    scheduleTableHelper.addItem(new ScheduleItem
                    {
                        Date = date,
                        StartTime = start,
                        EndTime = end,
                    }, clientID ?? 0, couchId);
                    MessageBox.Show("Сеанс успешно записан!");
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void clientListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach(Client item in clients) {
                string[] names = ((ListBox)sender).SelectedItem.ToString().Split(' ');
                if (names[0] == item.surname.Trim() && names[1] == item.name.Trim()) clientID= item.id;
            }// сранение только исходя из имени и фамилии. таким образом может быть ошибка если таблица содердит тесок!
        }

        private void clientListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            foreach(Client item in clients) {
                string[] names = ((ListBox)sender).SelectedItem.ToString().Split(' ');
                if (names[0] == item.surname.Trim() && names[1] == item.name.Trim()) {
                    MessageBox.Show($"ИМЯ: {item.name},\n" +
                                    $"ФАМИЛИЯ: {item.surname}\n" +
                                    $"ДАТА РОЖДЕНИЯ: {item.birthday.Date}\n" +
                                    $"ЭЛ. ПОЧТА: {item.email}\n" +
                                    $"НОМЕР ТЕЛЕФОНА: {item.phoneNumber}\n" +
                                    $"ДАТА РЕГИСТРАЦИИ: {item.registrationDate.Date}\n", "ИНФОРМАЦИЯ ПОЛЬЗОВАТЕЛЯ", MessageBoxButton.OK);
                }
            }
        }
    }
}
