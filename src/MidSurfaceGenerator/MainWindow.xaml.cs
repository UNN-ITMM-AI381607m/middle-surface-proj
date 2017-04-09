using Microsoft.Win32;
using MidSurfaceNameSpace.IO;
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
using MidSurfaceNameSpace.Solver;

namespace MidSurfaceNameSpace.MidSurfaceGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MidSurfaceNameSpace.Component.IModel model;
        private MidSurfaceNameSpace.IMidSurface mid_surface_model;
        private MidSurfaceNameSpace.Component.IView view;

        public MainWindow()
        {
            InitializeComponent();
            view = new MidSurfaceNameSpace.Component.View(
                mainCanvas);
 
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Dinar: think about changing statuses and implementation
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
                    MidSurfaceNameSpace.Component.Model model_temp = new MidSurfaceNameSpace.Component.Model();
                    model_temp.Add(new Parser().ImportFile(importDlg.FileName));
                    //TODO: Dinar: some trick, maybe not good solution
                    model = model_temp;
                    RedrawModel();
                }
                catch (Exception ex)
                {
                    //TODO: Dinar: replace 
                    MessageBox.Show("Error: Could not read file from disk, cause is: " + ex.Message);
                }
            }
            currentStatus.Content = "Ready for work";
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Dinar: prepare window with setting. Place for settings! 

        }

        private void Generate(object sender, RoutedEventArgs e)
        {
            //TODO: Dinar: prepare generating implementation
            currentStatus.Content = "Generating...";
            MidSurfaceNameSpace.Solver.IAlgorithm alg = new MidSurfaceNameSpace.Solver.Algorithm();
            mid_surface_model = alg.Run(new SolverData(model));
            RedrawMisSurface();
            currentStatus.Content = "Ready for work";
        }

        private void RedrawModel()
        {
            if (model == null) return;
            //TODO: Dinar: continue connecting parameters
            mainCanvas.Children.Clear();
            View.VisibleDataSettings settings= new View.VisibleDataSettings();
            settings.Brush = Brushes.Black;
            settings.Thikness = 2;
            View.VisibleData visible_data = new View.VisibleData(model,settings);
            view.Paint(visible_data);
        }
        private void RedrawMisSurface()
        {
            if (mid_surface_model == null) return;
            //TODO: Dinar: continue connecting parameters
            View.VisibleDataSettings settings = new View.VisibleDataSettings();
            settings.Brush = Brushes.Red;
            settings.Thikness = 1;
            View.VisibleData visible_data = new View.VisibleData(mid_surface_model, settings);
            view.Paint(visible_data);
        }
    }
}
