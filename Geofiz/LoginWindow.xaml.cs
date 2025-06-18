using System.Windows;
using System.Windows.Controls;

namespace GeofizApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string username = UsernameBox.Text.Trim();

            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Введите имя и выберите роль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                MainWindow mainWindow = new(role, username);
                Application.Current.MainWindow = mainWindow; // <- сделать его главным окном
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при запуске главного окна:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
