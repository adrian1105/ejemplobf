using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vivaldi.View
{
    /// <summary>
    /// Lógica de interacción para ReporteControl.xaml
    /// </summary>
    public partial class ReporteControl : UserControl
    {
        public ReporteControl()
        {
            InitializeComponent();
            LoadPieChartData();
            LoadBarChartData();
            LoadBar2ChartData();
        }

        private void LoadPieChartData()
        {
            ((PieSeries)mcChart.Series[0]).ItemsSource = new KeyValuePair<string, int>[] {
                new KeyValuePair<string, int>("Project Manager", 12),  
                new KeyValuePair<string, int>("CEO", 25),  
                new KeyValuePair<string, int>("Software Engg.", 5),  
                new KeyValuePair<string, int>("Team Leader", 6),  
                new KeyValuePair<string, int>("Project Leader", 10),  
                new KeyValuePair<string, int>("Developer", 4)
           };
        }

        private void LoadBarChartData()
        {
            ((ColumnSeries)mcChart2.Series[0]).ItemsSource = new KeyValuePair<string, int>[] {
                new KeyValuePair<string, int>("Omitidos", 12),
                new KeyValuePair<string, int>("No match", 25),
                new KeyValuePair<string, int>("Match", 30),
           };
        }

        private void LoadBar2ChartData()
        {
            ((ColumnSeries)mcChart3.Series[0]).ItemsSource = new KeyValuePair<string, int>[] {
                new KeyValuePair<string, int>("Individual", 20),
                new KeyValuePair<string, int>("Aleatoria", 100),
           };
        }
    }
}
