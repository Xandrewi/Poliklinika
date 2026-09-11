using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Poliklinika
{
    public partial class UpcomingAppointmentsWindow : Window
    {
        public UpcomingAppointmentsWindow()
        {
            InitializeComponent();
            LoadUpcomingAppointments();
        }

        private void LoadUpcomingAppointments()
        {
            string login = App.Current.Properties["CurrentUserLogin"]?.ToString();
            if (string.IsNullOrEmpty(login)) return;

            try
            {
                // Запрос для получения будущих записей текущего пациента
                // Используем JOIN через таблицу Order, так как в Appointment нет PatientID
                string query = @"
                    SELECT 
                        a.DateService,
                        ms.TitleService,
                        e.LName + ' ' + SUBSTRING(e.FName, 1, 1) + '.' AS DoctorName
                    FROM Patient p
                    JOIN [Order] o ON p.IDPatient = o.IDPatient
                    JOIN Appointment a ON o.IDAppointment = a.IDAppointment
                    JOIN MedicalService ms ON a.IDMedicalService = ms.IDMedicalService
                    JOIN Employee e ON a.IDEmployee = e.IDEmployee
                    WHERE p.Login = @Login AND a.DateService >= GETDATE()
                    ORDER BY a.DateService ASC";

                var parameters = new[] { new SqlParameter("@Login", login) };
                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Ближайшей записи нет.", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    dgAppointments.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки записей:\n" + ex.Message, "Ошибка");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // Возврат на главную страницу
            MainPatientWindow mainPage = new MainPatientWindow();
            mainPage.Show();
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