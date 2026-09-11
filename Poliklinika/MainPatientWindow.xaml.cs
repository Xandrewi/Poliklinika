using System.Windows;

namespace Poliklinika
{
    public partial class MainPatientWindow : Window
    {
        public MainPatientWindow()
        {
            InitializeComponent();
        }

        private void btnMyData_Click(object sender, RoutedEventArgs e)
        {
            new MyDataWindow().Show();
            this.Close();
        }

        private void btnMyHistory_Click(object sender, RoutedEventArgs e)
        {
            new MyHistoryWindow().Show();
            this.Close();
        }

        private void btnAppointment_Click(object sender, RoutedEventArgs e)
        {
            new AppointmentWindow().Show();
            this.Close();
        }

        private void picBell_Click(object sender, RoutedEventArgs e)
        {
            new UpcomingAppointmentsWindow().Show();
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