using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Poliklinika
{
    public partial class EditDataWindow : Window
    {
        public EditDataWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string login = App.Current.Properties["CurrentUserLogin"]?.ToString();
                string query = "SELECT * FROM Patient WHERE Login = @Login";
                var parameters = new[] { new SqlParameter("@Login", login) };
                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtSurname.Text = row["LName"].ToString();
                    txtName.Text = row["FName"].ToString();
                    txtPatronymic.Text = row["MName"].ToString();
                    txtAddress.Text = row["Address"].ToString();
                    txtPhone.Text = row["Phone"].ToString();
                    txtEmail.Text = row["Email"].ToString();

                    if (row["DateOfBirthday"] != DBNull.Value)
                        dtpBirthDate.SelectedDate = Convert.ToDateTime(row["DateOfBirthday"]);

                    int genderId = Convert.ToInt32(row["IDGender"]);
                    // Предполагаем: 1 - Мужской, 2 - Женский. Проверьте ваши ID в таблице Gender
                    if (genderId == 1) rbMale.IsChecked = true;
                    else rbFemale.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки:\n" + ex.Message, "Ошибка");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = App.Current.Properties["CurrentUserLogin"]?.ToString();
                int genderId = (rbMale.IsChecked == true) ? 1 : 2;

                string query = @"
                    UPDATE Patient SET
                        LName = @LName, FName = @FName, MName = @MName,
                        Address = @Address, Phone = @Phone, Email = @Email,
                        DateOfBirthday = @DateOfBirthday, IDGender = @IDGender
                    WHERE Login = @Login";

                var parameters = new[]
                {
                    new SqlParameter("@LName", txtSurname.Text.Trim()),
                    new SqlParameter("@FName", txtName.Text.Trim()),
                    new SqlParameter("@MName", txtPatronymic.Text.Trim()),
                    new SqlParameter("@Address", txtAddress.Text.Trim()),
                    new SqlParameter("@Phone", txtPhone.Text.Trim()),
                    new SqlParameter("@Email", txtEmail.Text.Trim()),
                    new SqlParameter("@DateOfBirthday", dtpBirthDate.SelectedDate ?? DateTime.Now),
                    new SqlParameter("@IDGender", genderId),
                    new SqlParameter("@Login", login)
                };

                DatabaseHelper.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Данные сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                new MyDataWindow().Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения:\n" + ex.Message, "Ошибка");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            new MyDataWindow().Show();
            this.Close();
        }

        // ДОБАВЛЕННЫЙ МЕТОД ДЛЯ КНОПКИ ВЫХОДА
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Выйти из приложения?", "Выход",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }
    }
}