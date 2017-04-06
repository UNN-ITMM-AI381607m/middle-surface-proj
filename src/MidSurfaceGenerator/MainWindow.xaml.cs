using Microsoft.Win32;
using MidSurface.IO;
using MidSurface.Solver;
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
        private MidSurface.IMidSurface mid_surface_model;
        private MidSurface.Component.IView view;
        System.Windows.Point origin, mouse_point;

        public MainWindow()
        {
            InitializeComponent();
            view = new MidSurface.Component.View(mainCanvas);
 
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
                    //TODO: some trick, maybe not good solution
                    model = model_temp;
                    RedrawModel();
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
            currentStatus.Content = "Generating...";
            MidSurface.Solver.IAlgorithm alg = new MidSurface.Solver.Algorithm();
            mid_surface_model = alg.Run(new SolverData(model));
            RedrawMisSurface();
            currentStatus.Content = "Ready for work";
        }

        private void RedrawModel()
        {
            if (model == null) return;
            origin = new System.Windows.Point(0, mainCanvas.ActualHeight - menu.ActualHeight);
            //TODO: continue connecting parameters
            mainCanvas.Children.Clear();
            View.VisibleDataSettings settings= new View.VisibleDataSettings();
            settings.Scale = Int32.Parse(textBox_Scale.Text);
            settings.Brush = Brushes.Black;
            settings.Offset_X = origin.X; 
            settings.Offset_Y = origin.Y;
            settings.Thikness = 2;
            View.VisibleData visible_data = new View.VisibleData(model,settings);
            view.Paint(visible_data);
        }
        private void RedrawMisSurface()
        {
            if (mid_surface_model == null) return;
            origin = new System.Windows.Point(0, mainCanvas.ActualHeight - menu.ActualHeight);
            //TODO: continue connecting parameters
            View.VisibleDataSettings settings = new View.VisibleDataSettings();
            settings.Scale = Int32.Parse(textBox_Scale.Text);
            settings.Brush = Brushes.Red;
            settings.Offset_X = origin.X;
            settings.Offset_Y = origin.Y;
            settings.Thikness = 1;
            View.VisibleData visible_data = new View.VisibleData(mid_surface_model, settings);
            view.Paint(visible_data);

        }

        private void textBox_Scale_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            RedrawModel();
            RedrawMisSurface();
        }

        private void ChangeOriginOfCanvas(object sender, MouseButtonEventArgs e)
        {
            foreach (UIElement el in mainCanvas.Children)
            {
                el.SetValue(Canvas.LeftProperty, e.GetPosition(null).X);
                el.SetValue(Canvas.TopProperty, e.GetPosition(null).Y-mainCanvas.ActualHeight);
            }
            mainCanvas.UpdateLayout();
        }

        private void mainCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Control c = sender as Control;
            Mouse.Capture(c);
            mouse_point = e.GetPosition(null);
        }

        private void mainCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            origin.X += (e.GetPosition(null).X - mouse_point.X);
            origin.Y += (e.GetPosition(null).Y - mouse_point.Y);
            foreach (UIElement el in mainCanvas.Children)
            {
                el.SetValue(Canvas.LeftProperty, origin.X);
                el.SetValue(Canvas.TopProperty, origin.Y-mainCanvas.ActualHeight);
            }
            mainCanvas.UpdateLayout();
        }
    }
}
