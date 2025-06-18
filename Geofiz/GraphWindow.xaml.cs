using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

namespace GeofizApp
{
    public partial class GraphWindow : Window
    {
        private readonly int wellId;

        public GraphWindow(int wellId)
        {
            InitializeComponent();
            this.wellId = wellId;
            LoadPolygonData();
            LoadMeasurementCurve();
            LoadLogPoints();
        }
        public void AddLogPoint(double x, double y)
        {
            var logPoint = new ChartValues<LiveCharts.Defaults.ObservablePoint>
    {
        new LiveCharts.Defaults.ObservablePoint(x, y)
    };

            var logSeries = new ScatterSeries
            {
                Title = "Точка каротажа",
                Values = logPoint,
                PointGeometry = DefaultGeometries.Diamond,
                Stroke = System.Windows.Media.Brushes.Red,
                Fill = System.Windows.Media.Brushes.Red,
                MinPointShapeDiameter = 10,
                MaxPointShapeDiameter = 10
            };

            if (WellChart.Series != null)
            {
                WellChart.Series.Add(logSeries);
            }
        }


        private void LoadPolygonData()
        {
            string query = $"SELECT Coordinates, Area FROM Wells WHERE WellID = {wellId}";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Скважина не найдена.");
                return;
            }

            string coordinates = dt.Rows[0]["Coordinates"].ToString();
            string area = dt.Rows[0]["Area"].ToString();

            var coordPairs = coordinates.Split(';');
            if (coordPairs.Length < 1)
            {
                MessageBox.Show("Необходимо минимум 3 координаты для построения фигуры.");
                return;
            }

            var chartPoints = new ChartValues<LiveCharts.Defaults.ObservablePoint>();

            foreach (string pair in coordPairs)
            {
                var parts = pair.Split(',');
                if (parts.Length == 2 &&
                    double.TryParse(parts[0], out double x) &&
                    double.TryParse(parts[1], out double y))
                {
                    chartPoints.Add(new LiveCharts.Defaults.ObservablePoint(x, y));
                }
            }

            // Только если есть хотя бы 1 точка — добавляем первую в конец
            if (chartPoints.Count > 0)
            {
                chartPoints.Add(new LiveCharts.Defaults.ObservablePoint(chartPoints[0].X, chartPoints[0].Y));
            }
            else
            {
                MessageBox.Show("Не удалось разобрать координаты.");
                return;
            }

            WellChart.Series = new SeriesCollection
    {
        new LineSeries
        {
            Title = "Контур площади",
            Values = chartPoints,
            LineSmoothness = 0,
            PointGeometry = DefaultGeometries.Circle,
            StrokeThickness = 2,
            Fill = System.Windows.Media.Brushes.LightBlue,
            Stroke = System.Windows.Media.Brushes.DarkBlue
        }
    };

            // Устанавливаем заголовок с площадью
            this.Title = $"Контур скважины (ID: {wellId}) — Площадь: {area} м²";

            WellChart.AxisX[0].Title = "X (м)";
            WellChart.AxisY[0].Title = "Y (м)";
        }
        private void LoadMeasurementCurve()
        {
            string query = $@"
        SELECT Depth, MeasurementValue 
        FROM Measurements 
        WHERE WellID = {wellId}
        ORDER BY Depth";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных измерений для отображения.");
                return;
            }

            var values = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservablePoint>();

            foreach (DataRow row in dt.Rows)
            {
                values.Add(new LiveCharts.Defaults.ObservablePoint(
                    Convert.ToDouble(row["Depth"]),
                    Convert.ToDouble(row["MeasurementValue"])
                ));
            }

            DepthChart.Series = new SeriesCollection
    {
        new LineSeries
        {
            Title = "Профиль измерения",
            Values = values,
            PointGeometry = DefaultGeometries.Circle,
            LineSmoothness = 0.3,
            StrokeThickness = 2
        }
    };

            DepthChart.AxisX[0].Title = "Глубина (м)";
            DepthChart.AxisY[0].Title = "Значение измерения";
        }
        private void LoadLogPoints()
        {
            // Точки из Measurements
            string queryMeasurements = $"SELECT LoggingPoint FROM Measurements WHERE WellID = {wellId} AND LoggingPoint IS NOT NULL";
            DataTable dtMeasurements = DatabaseHelper.ExecuteQuery(queryMeasurements);

            foreach (DataRow row in dtMeasurements.Rows)
            {
                var pointStr = row["LoggingPoint"].ToString();
                if (pointStr.Contains(","))
                {
                    var parts = pointStr.Split(',');
                    if (double.TryParse(parts[0], out double x) && double.TryParse(parts[1], out double y))
                    {
                        AddLogPoint(x, y);
                    }
                }
            }

            // Точка из Wells

        }
    }
}