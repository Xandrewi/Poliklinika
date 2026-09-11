using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Poliklinika
{
    public partial class MyDataWindow : Window
    {
        public MyDataWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string login = App.Current.Properties["CurrentUserLogin"]?.ToString();
                if (string.IsNullOrEmpty(login)) return;

                // ИЗМЕНЕНО: Добавляем JOIN с таблицей Gender, чтобы получить название пола
                string query = @"SELECT p.*, g.GenderName 
                                 FROM Patient p
                                 JOIN Gender g ON p.IDGender = g.IDGender
                                 WHERE p.Login = @Login";

                var parameters = new[] { new SqlParameter("@Login", login) };
                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    // Формируем ФИО
                    txtFio.Text = $"{row["LName"]} {row["FName"]} {row["MName"]}";

                    // Дата рождения
                    if (row["DateOfBirthday"] != DBNull.Value)
                        txtBirthDate.Text = Convert.ToDateTime(row["DateOfBirthday"]).ToShortDateString();

                    txtPhone.Text = row["Phone"].ToString();

                    // ТЕПЕРЬ БЕРЕМ НАЗВАНИЕ ПОЛА ИЗ ТАБЛИЦЫ GENDER
                    txtGender.Text = row["GenderName"].ToString();

                    txtAddress.Text = row["Address"].ToString();
                    txtEmail.Text = row["Email"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных:\n" + ex.Message, "Ошибка");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            new MainPatientWindow().Show();
            this.Close();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            new EditDataWindow().Show();
            this.Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Выйти из приложения?", "Выход",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }
    }
}