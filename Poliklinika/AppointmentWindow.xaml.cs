using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Poliklinika
{
    public partial class AppointmentWindow : Window
    {
        public AppointmentWindow()
        {
            InitializeComponent();
            LoadDoctors();
            LoadServices();
            dtpDate.SelectedDate = DateTime.Today;
        }

        private void LoadDoctors()
        {
            try
            {
                // Загружаем сотрудников (врачей)
                string query = "SELECT IDEmployee, LName + ' ' + FName AS FullName FROM Employee";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                cmbDoctor.ItemsSource = dt.DefaultView;
                cmbDoctor.DisplayMemberPath = "FullName";
                cmbDoctor.SelectedValuePath = "IDEmployee";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки врачей:\n" + ex.Message, "Ошибка");
            }
        }

        private void LoadServices()
        {
            try
            {
                // Загружаем услуги (TitleService)
                string query = "SELECT IDMedicalService, TitleService FROM MedicalService";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                cmbService.ItemsSource = dt.DefaultView;
                cmbService.DisplayMemberPath = "TitleService";
                cmbService.SelectedValuePath = "IDMedicalService";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки услуг:\n" + ex.Message, "Ошибка");
            }
        }

        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDoctor.SelectedItem == null || cmbService.SelectedItem == null || dtpDate.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите врача, услугу и дату.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Вставка в таблицу Appointment
                // Столбцы: IDEmployee, IDMedicalService, DateService
                string insertQuery = @"
                    INSERT INTO Appointment (IDEmployee, IDMedicalService, DateService)
                    VALUES (@EmpID, @ServID, @Date)";

                var parameters = new[]
                {
                    new SqlParameter("@EmpID", Convert.ToInt32(cmbDoctor.SelectedValue)),
                    new SqlParameter("@ServID", Convert.ToInt32(cmbService.SelectedValue)),
                    new SqlParameter("@Date", dtpDate.SelectedDate.Value)
                };

                DatabaseHelper.ExecuteNonQuery(insertQuery, parameters);

                MessageBox.Show("Вы успешно записаны!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при записи:\n" + ex.Message, "Ошибка");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            new MainPatientWindow().Show();
            this.Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}