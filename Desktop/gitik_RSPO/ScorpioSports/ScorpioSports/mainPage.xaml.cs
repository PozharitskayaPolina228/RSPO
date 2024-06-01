using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
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
    /// Логика взаимодействия для mainPage.xaml
    /// </summary>
    public partial class mainPage : Page
    {
        public int couchID;
        public ObservableCollection<ScheduleItem> ScheduleItems { get; set; }
        public mainPage(int couchID)
        {
            try
            {
                InitializeComponent();
                this.couchID= couchID;
                //MessageBox.Show(couchID.ToString());
                string connectionString = "Data Source=DESKTOP-C0CATE;Initial Catalog=ScorpioTwoDB;Integrated Security=True";
                string query = "SELECT * FROM scheduleTable";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    ScheduleItems = new ObservableCollection<ScheduleItem>();
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if ((int)reader["idEmp"] == 4) continue;
                                if ((int)reader["idEmp"] == couchID)
                                {
                                    //Console.WriteLine($"ID: {reader["id"]}, Date: {reader["_date"]}, Start Time: {reader["_startTime"]}, End Time: {reader["_endTime"]}, Employee ID: {reader["idEmp"]}, User ID: {reader["idUser"]}");
                                    ScheduleItems.Add(new ScheduleItem
                                    {
                                        //ID = (int)reader["id"],
                                        Date = ((DateTime)reader["_date"]).Date,
                                        StartTime = (TimeSpan)reader["_startTime"],
                                        EndTime = (TimeSpan)reader["_endTime"],
                                        Client = ClientsTableHelper.getFullName((int)reader["idUser"]),
                                        phoneNumber = CodeHelper.formatPhoneNumber(ClientsTableHelper.GetClienPhoneNumberdById((int)reader["idUser"]))
                                    });
                                }
                                
                            }
                        }
                    }
                }
                scheduleDataGrid.ItemsSource = ScheduleItems;
            }
            catch(Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void profile_Click(object sender, RoutedEventArgs e)
        {
            couchProfilePage win = new couchProfilePage(couchID);
            win.ShowDialog();
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            var item = (dynamic)scheduleDataGrid.SelectedItem;
            if(item != null)
            {
                //scheduleTableHelper.removeItem(ClientsTableHelper.GetClientIdByPhoneNumber(CodeHelper.unformatPhoneNumber( item.phoneNumber)));
                scheduleTableHelper.DeleteScheduleEntry(item.Date, item.StartTime);
            }
            else
            {
                MessageBox.Show("выберите запись из таблицы");
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            addRegistration addd = new addRegistration(this.couchID);
            addd.ShowDialog();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new mainPage(this.couchID));
        }
    }
}
