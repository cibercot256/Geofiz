using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace GeofizApp
{
    public partial class ChangeProjectWindow : Window
    {
        public int? SelectedWellID { get; private set; }

        public ChangeProjectWindow()
        {
            InitializeComponent();
            LoadWells();
        }

        private void LoadWells()
        {
            try
            {
                string query = "SELECT WellID, UniqueCode AS [Код скважины], Area AS [Площадь], Coordinates AS [Координаты] FROM Wells";
                WellsList.ItemsSource = DatabaseHelper.ExecuteQuery(query).DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки скважин: " + ex.Message);
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (WellsList.SelectedItem is DataRowView row)
            {
                SelectedWellID = Convert.ToInt32(row["WellID"]);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите скважину.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
