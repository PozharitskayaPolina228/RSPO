using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace ScorpioSports
{
    public class ScheduleItem
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Client { get; set; }
        public string phoneNumber { get; set; }
    }

    public class Client
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime birthday { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public DateTime registrationDate { get; set; }
    }

    public class Couch
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string post { get; set; }
        public DateTime registrationDate { get; set; }
        public string contacts { get; set; }
        public double salary { get; set; }
        public string login { get; set; }
        public string password { get; set; }
    }
    
    public class CodeHelper
    {
        public static string formatPhoneNumber(string num)
        {
            try
            {
                if (num.Length != 12) throw new Exception("неверная длина строки!");
                else
                {
                    // Проверяем, что входная строка содержит только цифры
                    if (!Regex.IsMatch(num, @"^\d+$"))
                    {
                        throw new ArgumentException("Номер телефона должен содержать только цифры.", nameof(num));
                    }

                    // Убираем первую цифру, если она равна 8 или 9 (обычно это код страны)
                    if (num.Length > 10 && (num[0] == '8' || num[0] == '9'))
                    {
                        num = num.Substring(1);
                    }

                    // Добавляем префикс и разделители
                    return $"+{num.Substring(0, 3)} ({num.Substring(3, 2)}) {num.Substring(5, 3)}-{num.Substring(8, 2)}-{num.Substring(10)}";
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            return "";
        }

        public static string unformatPhoneNumber(string num)
        {
            try
            {
                string result = "";
                foreach(char item in num)
                {
                    if (Regex.IsMatch(item.ToString(), @"\d"))
                    {
                        result += item;
                    }
                }
                if (!Regex.IsMatch(result, @"^\d+$"))
                {
                    throw new ArgumentException("Номер телефона содержит не только цифры.", nameof(num));
                }
                else return result;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            return "";
        }
    }

    public class scheduleTableHelper
    {
        public static string connectionString { get; } = "Data Source=DESKTOP-C0CATE;Initial Catalog=ScorpioTwoDB;Integrated Security=True";
        public static void DeleteScheduleEntry(DateTime date, TimeSpan startTime)
        {
            string query = "DELETE FROM scheduleTable WHERE _date = @date AND _startTime = @startTime";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@startTime", startTime);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public static void removeItem(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM scheduleTable WHERE id = @id;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public static void addItem(ScheduleItem item, int userID, int empID)
        {
            try
            {
                // Строка запроса INSERT
                string query = "INSERT INTO scheduleTable (_date, _startTime, _endTime, idEmp, idUser) VALUES (@date, @startTime, @endTime, @idEmp, @idUser)";

                // Создание подключения к базе данных и выполнение запроса
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Создание команды для выполнения запроса
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Добавление параметров в запрос
                        command.Parameters.AddWithValue("@date", item.Date);
                        command.Parameters.AddWithValue("@startTime", item.StartTime);
                        command.Parameters.AddWithValue("@endTime", item.EndTime);
                        command.Parameters.AddWithValue("@idEmp", empID);
                        command.Parameters.AddWithValue("@idUser", userID);

                        // Выполнение запроса
                        int rowsAffected = command.ExecuteNonQuery();

                        // Проверка количества затронутых строк (для отладки)
                        Console.WriteLine($"Добавлено {rowsAffected} строк.");
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        
    }


    // // // // // // // // // // // // // // // // /// // /// // // // // // //
    // // // // // // // // // // // // // CLIENTS /// // /// /// // // // // //
    // // // // // // // // // // // // // // // // /// // /// // // // // // //
    public class ClientsTableHelper
    {
        public static string connectionString { get; } = "Data Source=DESKTOP-C0CATE;Initial Catalog=ScorpioTwoDB;Integrated Security=True";
        public static void AddClient(string name, string surname, DateTime birthday, string email, string phoneNumber)
        {
            string query = "INSERT INTO clientsTable (_name, _surname, _birthday, _email, _phoneNumber, _registrationDate) VALUES (@name, @surname, @birthday, @email, @phoneNumber, @reg)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@surname", surname);
                command.Parameters.AddWithValue("@birthday", birthday);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                command.Parameters.AddWithValue("@reg", DateTime.Now);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public static bool CheckIfExists(string email, string phoneNumber)
        {
            string query = "SELECT COUNT(*) FROM clientsTable WHERE _email = @email OR _phoneNumber = @phoneNumber";
            int count = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@phoneNumber", phoneNumber);

                connection.Open();
                count = (int)command.ExecuteScalar();
            }

            return count > 0;
        }


        public static void deleteClient(string phoneNumber)
        {
            string query = "DELETE FROM clientsTable WHERE _phoneNumber = @num";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@num", phoneNumber);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static List<Client> getAllClients()
        {
            try
            {
                List<Client> clients = new List<Client>();

                string query = "SELECT * FROM clientsTable";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Client client = new Client();
                        clients.Add(new Client {
                            id = Convert.ToInt32(reader["id"]),
                            name = reader["_name"].ToString(),
                            surname = reader["_surname"].ToString(),
                            birthday = Convert.ToDateTime(reader["_birthday"]),
                            email = reader["_email"].ToString(),
                            phoneNumber = reader["_phoneNumber"].ToString(),
                            registrationDate = Convert.ToDateTime(reader["_registrationDate"]),
                        });
                    }
                    reader.Close();
                }

                return clients;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            return null;
        }

        public static string getName(int id)
        {
            try
            {
                string name = null;
                string query = "SELECT _name FROM ClientsTable WHERE id = @id";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read()) { name = reader["_name"].ToString(); }
                    reader.Close();
                }
                return name;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
            return null;
        }

        public static string getFullName(int id)
        {
            try
            {
                string name = null;
                string query = "SELECT _name, _surname FROM ClientsTable WHERE id = @id";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        name = $"{reader["_name"].ToString().Trim()} {reader["_surname"].ToString().Trim()}";
                    }
                    reader.Close();
                }
                return name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        public static int GetClientIdByPhoneNumber(string phoneNumber)
        {
            string query = "SELECT id FROM clientsTable WHERE _phoneNumber = @phoneNumber";
            int clientId = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@phoneNumber", phoneNumber);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    clientId = Convert.ToInt32(reader["id"]);
                }

                reader.Close();
            }

            return clientId;
        }
        
        public static string GetClienPhoneNumberdById(int id)
        {
            try
            {
                string query = "SELECT _phoneNumber FROM clientsTable WHERE id = @id_";
                string clientPhoneNumber = "";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id_", id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        clientPhoneNumber = reader["_phoneNumber"].ToString();
                    }
                    reader.Close();
                }

                return clientPhoneNumber;
            }
            catch(Exception ex) { MessageBox.Show(ex.ToString()); }
            return "";
        }
        public static Client GetClientByPhoneNumber(string id)
        {
            try
            {
                string query = "SELECT * FROM clientsTable WHERE _phoneNumber = @id_";
                Client now = null;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id_", id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        now = new Client
                        {
                            id = Convert.ToInt32(reader["id"]),
                            name = reader["_name"].ToString(),
                            surname = reader["_surname"].ToString(),
                            birthday = Convert.ToDateTime(reader["_birthday"]),
                            email = reader["_email"].ToString(),
                            phoneNumber = reader["_phoneNumber"].ToString(),
                            registrationDate = Convert.ToDateTime(reader["_registrationDate"]),
                        };
                    }
                    reader.Close();
                }
                return now;
            }
            catch(Exception ex) { MessageBox.Show(ex.ToString()); }
            return null;
        }
        public static bool isParameterInTable(string parameter, string columnName)
        {
            try
            {
                string query = $"SELECT COUNT(*) FROM clientsTable WHERE {columnName} = @param";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@param", parameter);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
        public static void UpdateClient(int clientId, string name, string surname, DateTime birthday, string email, string phoneNumber)
        {
            string query = "UPDATE clientsTable SET _name = @name, _surname = @surname, _birthday = @birthday, _email = @email, _phoneNumber = @phoneNumber WHERE id = @clientId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@surname", surname);
                command.Parameters.AddWithValue("@birthday", birthday);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                command.Parameters.AddWithValue("@clientId", clientId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    public class employeesTableHelper
    {
        public static string connectionString { get; } = "Data Source=DESKTOP-C0CATE;Initial Catalog=ScorpioTwoDB;Integrated Security=True";

        public static List<Couch> getAllEmployees()
        {
            try
            {
                List<Couch> clients = new List<Couch>();
                string query = "SELECT * FROM employeesTable";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        /*
                        public int id { get; set; }
                        public string name { get; set; }
                        public string surname { get; set; }
                        public string post { get; set; }
                        public DateTime registrationDate { get; set; }
                        public string contacts { get; set; }
                        public float salary { get; set; }
                        public string login { get; set; }
                        public string password { get; set; }
                         */
                        //int id = (int)reader["id"];
                        //string name = ((string)reader["_name"]).Trim();
                        //string surname = ((string)reader["_surname"]).Trim();
                        //string post = ((string)reader["_post"]).Trim();
                        //DateTime registrationDate = Convert.ToDateTime(reader["_enterDate"]);
                        //string contacts = ((string)reader["_contacts"]).Trim();
                        //double salary = (double)reader["_salary"];
                        //string login = ((string)reader["_login"]).Trim();
                        //string password = ((string)reader["_password"]).Trim();

                        clients.Add(new Couch
                        {
                            id = (int)reader["id"],
                            name = ((string)reader["_name"]).Trim(),
                            surname = ((string)reader["_surname"]).Trim(),
                            post = ((string)reader["_post"]).Trim(),
                            registrationDate = Convert.ToDateTime(reader["_enterDate"]),
                            contacts = ((string)reader["_contacts"]).Trim(),
                            salary = (double)reader["_salary"],
                            login = ((string)reader["_login"]).Trim(),
                            password = ((string)reader["_password"]).Trim(),
                        });
                    }
                    reader.Close();
                }

                return clients;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            return null;
        }

        public static bool IsLoginAndPasswordCorrect(string login, string password)
        {
            string query = "SELECT COUNT(*) FROM employeesTable WHERE _login = @login AND _password = @password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);

                connection.Open();
                int count = (int)command.ExecuteScalar();

                return count > 0;
            }
        }
        public static bool isParameterInTable(string parameter, string columnName)
        {
            try
            {
                string query = $"SELECT COUNT(*) FROM employeesTable WHERE {columnName} = @param";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@param", parameter);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
        
        public static Couch getCouchByID(int ID)
        {
            if(employeesTableHelper.isParameterInTable(ID.ToString(), "id"))
            {
                string query = "SELECT * FROM employeesTable WHERE id = @id";
                Couch employee = null;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", ID);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        employee = new Couch
                        {
                            id = Convert.ToInt32(reader["id"]),
                            name = reader["_name"].ToString(),
                            surname = reader["_surname"].ToString(),
                            post = reader["_post"].ToString(),
                            registrationDate = (DateTime)reader["_enterDate"],
                            contacts = reader["_contacts"].ToString(),
                            //salary = (float)reader["_contacts"], ошиюка "Unable to cast object of type 'System.String' to type 'System.Single'."
                            salary = 1000,
                            login = reader["_login"].ToString(),
                            password = reader["_password"].ToString(),
                        };
                    }

                    reader.Close();
                }

                return employee;
            }
            else
            {
                return null;
            }
        }

        public static int getIdByLogin(string login)
        {
            string query = "SELECT * FROM employeesTable WHERE _login = @id";
            int id = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", login);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    id = (int)reader["id"];
                }

                reader.Close();
            }

            return id;
        }

        public static string getPostByLogin(string login)
        {
            string query = "SELECT * FROM employeesTable WHERE _login = @id";
            string id = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", login);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    id = (string)reader["_post"];
                }

                reader.Close();
            }

            return id;
        }


        public static Couch GetEmployeeByPhoneNumber(string id)
        {
            try
            {
                string query = "SELECT * FROM employeesTable WHERE _contacts = @id_";
                Couch now = null;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id_", id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        now = new Couch
                        {
                            id = (int)reader["id"],
                            name = ((string)reader["_name"]).Trim(),
                            surname = ((string)reader["_surname"]).Trim(),
                            post = ((string)reader["_post"]).Trim(),
                            registrationDate = Convert.ToDateTime(reader["_enterDate"]),
                            contacts = ((string)reader["_contacts"]).Trim(),
                            salary = (double)reader["_salary"],
                            login = ((string)reader["_login"]).Trim(),
                            password = ((string)reader["_password"]).Trim(),
                        };
                    }
                    reader.Close();
                }
                return now;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            return null;
        }

        public static void deleteEmployee(string phoneNumber)
        {
            string query = "DELETE FROM employeesTable WHERE _contacts = @num";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@num", phoneNumber);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public static bool checkLoginAndPhoneNumber(string _login, string _contacts)
        {
            string query = "SELECT COUNT(*) FROM employeesTable WHERE _login = @_login OR _contacts = @_contacts";
            int count = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@_login", _login);
                command.Parameters.AddWithValue("@_contacts", _contacts);

                connection.Open();
                count = (int)command.ExecuteScalar();
            }

            return count > 0;
        }
        public static void addEmployee(string name, string surname, string post, DateTime? date, string contacts, double salary, string login, string password)
        {
            try
            {
                // Строка запроса INSERT
                string query = "INSERT INTO employeesTable (_name, _surname, _post, _enterDate, _contacts, _salary, _login, _password) VALUES (@_name, @_surname, @_post, @_enterDate, @_contacts, @_salary, @_login, @_password)";

                // Создание подключения к базе данных и выполнение запроса
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Создание команды для выполнения запроса
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Добавление параметров в запрос
                        command.Parameters.AddWithValue("@_name", name);
                        command.Parameters.AddWithValue("@_surname", surname);
                        command.Parameters.AddWithValue("@_post", post);
                        command.Parameters.AddWithValue("@_enterDate", date);
                        command.Parameters.AddWithValue("@_contacts", contacts);
                        command.Parameters.AddWithValue("@_salary", salary);
                        command.Parameters.AddWithValue("@_login", login);
                        command.Parameters.AddWithValue("@_password", password);

                        // Выполнение запроса
                        int rowsAffected = command.ExecuteNonQuery();

                        // Проверка количества затронутых строк (для отладки)
                        //Console.WriteLine($"Добавлено {rowsAffected} строк.");
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
