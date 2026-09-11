using System;
using System.Data.SqlClient;
using System.Windows;

namespace Poliklinika
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(txtSurname.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtLogin.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Заполните обязательные поля: Фамилия, Имя, Логин, Пароль.",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Проверка уникальности логина
                string checkQuery = "SELECT COUNT(*) FROM Patient WHERE Login = @Login";
                var checkParams = new[] { new SqlParameter("@Login", txtLogin.Text.Trim()) };
                int exists = Convert.ToInt32(DatabaseHelper.ExecuteScalar(checkQuery, checkParams));

                if (exists > 0)
                {
                    MessageBox.Show("Такой логин уже занят.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Определение ID пола (1=М, 2=Ж)
                int genderId = (rbMale.IsChecked == true) ? 1 : 2;

                string insertQuery = @"
                    INSERT INTO Patient 
                        (FName, LName, MName, Address, Phone, Email, 
                         Login, Password, DateOfBirthday, IDGender)
                    VALUES 
                        (@FName, @LName, @MName, @Address, @Phone, @Email,
                         @Login, @Password, @DateOfBirthday, @IDGender)";

                var parameters = new[]
                {
                    new SqlParameter("@FName", txtName.Text.Trim()),
                    new SqlParameter("@LName", txtSurname.Text.Trim()),
                    new SqlParameter("@MName", txtPatronymic.Text.Trim()),
                    new SqlParameter("@Address", txtAddress.Text.Trim()),
                    new SqlParameter("@Phone", txtPhone.Text.Trim()),
                    new SqlParameter("@Email", txtEmail.Text.Trim()),
                    new SqlParameter("@Login", txtLogin.Text.Trim()),
                    new SqlParameter("@Password", txtPassword.Password.Trim()),
                    new SqlParameter("@DateOfBirthday", dtpBirthDate.SelectedDate ?? DateTime.Now),
                    new SqlParameter("@IDGender", genderId)
                };

                DatabaseHelper.ExecuteNonQuery(insertQuery, parameters);

                MessageBox.Show("Регистрация прошла успешно!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                var authWindow = new AuthorizationWindow();
                authWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при регистрации:\n" + ex.Message, "Ошибка");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var authWindow = new AuthorizationWindow();
            authWindow.Show();
            this.Close();
        }
    }
}