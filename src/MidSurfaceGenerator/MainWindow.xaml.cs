using Microsoft.Win32;
using MidSurface.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MidSurfaceGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MidSurface.Component.IModel model;
        private MidSurface.Component.IView view;

        public MainWindow()
        {
            InitializeComponent();
            MidSurface.Component.ICanvas canvas = new MidSurface.Component.Canvas(mainCanvas);
            view = new MidSurface.Component.View(canvas);
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            //TODO: think about changing statuses and implementation
            currentStatus.Content = "Imporing model...";

            OpenFileDialog importDlg = new OpenFileDialog();
            importDlg.InitialDirectory = "C:\\";
            importDlg.Filter = "All files (*.*)|*.*|XML Models (*.xml)|*.xml";
            importDlg.FilterIndex = 2;
            importDlg.RestoreDirectory = true;
            if (importDlg.ShowDialog() != null)
            {
                try
                {
                    MidSurface.Component.Model model_temp = new MidSurface.Component.Model();
                    model_temp.Add(new Parser().ImportFile(importDlg.FileName));
                    model = model_temp;
                    View.VisibleData visible_data = new View.VisibleData(model);
                    view.Paint(visible_data);
                }
                catch (Exception ex)
                {
                    //TODO : replace 
                    MessageBox.Show("Error: Could not read file from disk, cause is: " + ex.Message);
                }
            }
            currentStatus.Content = "Ready for work";
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            //TODO: prepare window with setting. Place for settings! 
        }

        private void Generate(object sender, RoutedEventArgs e)
        {
            //TODO: prepare generating implementation
        }
    }
}
