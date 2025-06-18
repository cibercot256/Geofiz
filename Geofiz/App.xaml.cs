using System.Windows;

namespace GeofizApp
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Запускаем окно логина
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}