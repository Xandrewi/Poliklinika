using System;
using System.Data.SqlClient;
using System.Windows;

namespace Poliklinika
{
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Используем таблицу Patient
                string query = "SELECT COUNT(*) FROM Patient WHERE Login = @Login AND Password = @Password";
                var parameters = new[]
                {
                    new SqlParameter("@Login", login),
                    new SqlParameter("@Password", password)
                };

                object result = DatabaseHelper.ExecuteScalar(query, parameters);
                int count = Convert.ToInt32(result);

                if (count > 0)
                {
                    // Сохраняем логин текущего пользователя глобально
                    App.Current.Properties["CurrentUserLogin"] = login;

                    var mainWindow = new MainPatientWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к БД:\n" + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var regWindow = new RegistrationWindow();
            regWindow.Show();
            this.Close();
        }
    }
}