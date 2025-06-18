using System;
using System.Data;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GeofizApp
{
    public partial class EditProjectWindow : Window
    {
        public EditProjectWindow()
        {
            InitializeComponent();
            LoadLoggingTypes();

  
        }

        private void LoadLoggingTypes()
        {
            string query = "SELECT LoggingTypeID, TypeName FROM LoggingTypes";
            LoggingTypeComboBox.ItemsSource = DatabaseHelper.ExecuteQuery(query).DefaultView;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string logXStr = LogXBox.Text.Trim();
            string logYStr = LogYBox.Text.Trim();

            if (!double.TryParse(logXStr, out double logX) || !double.TryParse(logYStr, out double logY))
            {
                MessageBox.Show("Введите корректные координаты точки каротажа (X и Y).");
                return;
            }

            try
            {
                string profile = ProfileBox.Text.Trim();
                string profileDesc = ProfileDescBox.Text.Trim();

                // Скважина
                string code = CodeBox.Text.Trim();
                string coords = CoordBox.Text.Trim();
                string area = AreaBox.Text.Trim();

                // Измерение
                int loggingTypeId = Convert.ToInt32(LoggingTypeComboBox.SelectedValue);
                decimal depth = decimal.Parse(DepthBox.Text.Trim());
                decimal value = decimal.Parse(ValueBox.Text.Trim());
                string operatorName = OperatorBox.Text.Trim();

                // Проверка и сборка даты и времени
                if (MeasurementDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Выберите дату измерения.");
                    return;
                }

                if (!int.TryParse(HourBox.Text, out int hour) || hour < 0 || hour > 23 ||
                    !int.TryParse(MinuteBox.Text, out int minute) || minute < 0 || minute > 59)
                {
                    MessageBox.Show("Введите корректные часы (0–23) и минуты (0–59).");
                    return;
                }

                DateTime measurementDateTime = MeasurementDatePicker.SelectedDate.Value
                    .AddHours(hour)
                    .AddMinutes(minute);

                string measurementDateTimeStr = measurementDateTime.ToString("yyyy-MM-ddTHH:mm:ss");


                // Проверка обязательных полей
                if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(coords) || string.IsNullOrEmpty(area) ||
                    string.IsNullOrEmpty(operatorName))
                {
                    MessageBox.Show("Заполните все поля.");
                    return;
                }

                // 1. Добавляем скважину
                string insertWell = $@"
                INSERT INTO Wells (UniqueCode, Coordinates, Area, Profile, ProfileDescription) 
                VALUES ('{code}', '{coords}', '{area}', '{profile}', '{profileDesc}');";
                DatabaseHelper.ExecuteNonQuery(insertWell);

                // 2. Получаем ID новой скважины
                string getIdQuery = "SELECT TOP 1 WellID FROM Wells ORDER BY WellID DESC";
                DataTable result = DatabaseHelper.ExecuteQuery(getIdQuery);
                int wellId = Convert.ToInt32(result.Rows[0]["WellID"]);

                // 3. Добавляем измерение
                string insertMeasurement = $@"
                INSERT INTO Measurements 
                (WellID, LoggingTypeID, Depth, MeasurementValue, MeasurementDateTime, Operator, LoggingPoint)
                VALUES 
                ({wellId}, {loggingTypeId}, {depth}, {value}, '{measurementDateTimeStr}', '{operatorName}', '{logX},{logY}')";
                DatabaseHelper.ExecuteNonQuery(insertMeasurement);


                DatabaseHelper.ExecuteNonQuery(insertMeasurement);

                MessageBox.Show("Скважина и измерение добавлены.");
                GraphWindow graphWindow = new GraphWindow(wellId);
                graphWindow.Show();
                graphWindow.AddLogPoint(logX, logY);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

    }
}
