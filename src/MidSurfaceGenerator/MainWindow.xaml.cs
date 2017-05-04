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
using System.IO;
using System.Globalization;

namespace MidSurfaceNameSpace.MidSurfaceGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Component.IModel model;
        private IMidSurface mid_surface_model;
        private Component.IView view;

        public MainWindow()
        {
            InitializeComponent();
            view = new Component.View(mainCanvas);
 
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
                    Component.Model model_temp = new Component.Model();
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
            if (model == null) return;
            RedrawModel();
            currentStatus.Content = "Generating...";

            //TODO: Move to some input checking
            double splitterAccuracy = 0;
            double detalizerAccuracy = 0;
            try
            {
                splitterAccuracy = double.Parse(textBox_Splitter_Accuracy.Text, CultureInfo.InvariantCulture);
                detalizerAccuracy = double.Parse(textBox_Detalizer_Accuracy.Text, CultureInfo.InvariantCulture);
            }
            catch(Exception)
            {
                return;
            }

            IAlgorithm alg = new Algorithm(splitterAccuracy, detalizerAccuracy);
            mid_surface_model = alg.Run(new SolverData(model));
            RedrawMisSurface();
            currentStatus.Content = "Ready for work";
        }

        private void RedrawModel()
        {
            if (model == null) return;
            //TODO: Dinar: continue connecting parameters
            mainCanvas.Children.Clear();
            View.VisibleDataSettings settings = new View.VisibleDataSettings()
            {
                Brush = Brushes.Black,
                Thikness = 2
            };
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

        private void Go_all_tests(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog FBD = new System.Windows.Forms.FolderBrowserDialog();
            if (FBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] allFoundFiles = Directory.GetFiles(FBD.SelectedPath, "*.xml", SearchOption.AllDirectories);
                foreach (string path in allFoundFiles)
                {
                    Component.Model model_temp = new Component.Model();
                    model_temp.Add(new Parser().ImportFile(path));
                    model = model_temp;
                    RedrawModel();

                    //TODO: Move to some input checking
                    double splitterAccuracy = 0;
                    double detalizerAccuracy = 0;
                    try
                    {
                        splitterAccuracy = double.Parse(textBox_Splitter_Accuracy.Text, CultureInfo.InvariantCulture);
                        detalizerAccuracy = double.Parse(textBox_Detalizer_Accuracy.Text, CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        return;
                    }

                    IAlgorithm alg = new Algorithm(splitterAccuracy, detalizerAccuracy);
                    mid_surface_model = alg.Run(new SolverData(model));
                    RedrawMisSurface();
                    var rtb = new RenderTargetBitmap((int)mainCanvas.ActualWidth, (int)mainCanvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                    // needed otherwise the image output is black
                    mainCanvas.Measure(new Size((int)mainCanvas.ActualWidth, (int)mainCanvas.ActualHeight));
                    mainCanvas.Arrange(new Rect(new Size((int)mainCanvas.ActualWidth, (int)mainCanvas.ActualHeight)));
                    rtb.Render(mainCanvas);

                    PngBitmapEncoder BufferSave = new PngBitmapEncoder();
                    BufferSave.Frames.Add((BitmapFrame.Create(rtb)));
                    using (var fs = File.OpenWrite(path.Substring(0, path.Length - 4) + "res.png"))
                    {
                        BufferSave.Save(fs);
                    }
                }
            }
        }  
    }
}
