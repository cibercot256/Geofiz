using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace GeofizApp
{
    public partial class MainWindow : Window
    {
        private readonly string role;
        private readonly string username;
        private int? selectedWellID = null;

        public MainWindow(string role, string username)
        {
            InitializeComponent();
            this.role = role;
            this.username = username;
            LoadProjects();
            UpdateButtonPermissions();
        }

        private void LoadProjects()
        {
            try
            {
                string query = @"
SELECT 
    w.WellID,
    w.UniqueCode AS [Код скважины],
    w.Coordinates AS [Координаты площади],
    w.Area AS [Площадь],
    w.Profile AS [Профиль],
    w.ProfileDescription AS [Описание профиля],
    lt.TypeName AS [Тип каротажа],
    m.Depth AS [Глубина (м)],
    m.MeasurementValue AS [Значение измерения],
    m.MeasurementDateTime AS [Дата и время],
    m.Operator AS [Исполнитель],
    m.Comment AS [Комментарий]
FROM Wells w
LEFT JOIN Measurements m ON w.WellID = m.WellID
LEFT JOIN LoggingTypes lt ON m.LoggingTypeID = lt.LoggingTypeID";


                ProjectDataGrid.ItemsSource = DatabaseHelper.ExecuteQuery(query).DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchBox.Text.Trim();
            string query = $"SELECT * FROM Wells WHERE UniqueCode LIKE '%{keyword}%'";
            ProjectDataGrid.ItemsSource = DatabaseHelper.ExecuteQuery(query).DefaultView;
        }

        private void OpenGraph_Click(object sender, RoutedEventArgs e)
        {
            if (selectedWellID == null)
            {
                MessageBox.Show("Пожалуйста, выберите проект перед открытием графика.");
                return;
            }

            GraphWindow graphWindow = new((int)selectedWellID);
            graphWindow.ShowDialog();
        }

        private void SwitchProject_Click(object sender, RoutedEventArgs e)
        {
            if (role == "Админ" || role == "Аналитик")
            {
                var window = new ChangeProjectWindow();
                if (window.ShowDialog() == true)
                {
                    selectedWellID = window.SelectedWellID;
                    MessageBox.Show($"Выбрана скважина ID: {selectedWellID}");
                }
            }
            else
            {
                MessageBox.Show("Недостаточно прав для смены проекта.");
            }
        }

        private void EditProject_Click(object sender, RoutedEventArgs e)
        {
            if (role == "Админ" || role == "Геофизик")
            {
                new EditProjectWindow().ShowDialog();
            }
            else
            {
                MessageBox.Show("Недостаточно прав");
            }
        }

        private void DeleteProject_Click(object sender, RoutedEventArgs e)
        {
            if (role != "Админ")
            {
                MessageBox.Show("Удаление доступно только администратору.");
                return;
            }

            if (ProjectDataGrid.SelectedItem is DataRowView row)
            {
                int id = Convert.ToInt32(row["WellID"]);
                string query = $"DELETE FROM Wells WHERE WellID = {id}";
                DatabaseHelper.ExecuteNonQuery(query);
                LoadProjects();
            }
        }

        private void UpdateButtonPermissions()
        {
            SwitchProjectButton.IsEnabled = role == "Админ" || role == "Аналитик";
            EditProjectButton.IsEnabled = role == "Админ" || role == "Геофизик";
            DeleteProjectButton.IsEnabled = role == "Админ";
        }
    }
}
