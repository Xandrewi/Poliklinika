using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Poliklinika
{
    public partial class MyHistoryWindow : Window
    {
        public MyHistoryWindow()
        {
            InitializeComponent();
            LoadHistory();
        }

        private void LoadHistory()
        {
            string login = App.Current.Properties["CurrentUserLogin"]?.ToString();
            if (string.IsNullOrEmpty(login)) return;

            // Запрос через таблицу Order, так как в Appointment нет PatientID
            string query = @"
                SELECT 
                    a.DateService AS [Дата], 
                    ms.TitleService AS [Услуга], 
                    e.LName + ' ' + e.FName AS [Врач]
                FROM Patient p
                JOIN [Order] o ON p.IDPatient = o.IDPatient
                JOIN Appointment a ON o.IDAppointment = a.IDAppointment
                JOIN MedicalService ms ON a.IDMedicalService = ms.IDMedicalService
                JOIN Employee e ON a.IDEmployee = e.IDEmployee
                WHERE p.Login = @Login
                ORDER BY a.DateService DESC";

            var parameters = new[] { new SqlParameter("@Login", login) };

            try
            {
                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
                dataGridHistory.ItemsSource = dt.DefaultView;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Ошибка загрузки истории: " + ex.Message);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            new MainPatientWindow().Show();
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