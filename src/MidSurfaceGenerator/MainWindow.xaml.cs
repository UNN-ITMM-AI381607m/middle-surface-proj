﻿using Microsoft.Win32;
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
using MidSurfaceNameSpace.Primitive;

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
        string filename;

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
                    filename = importDlg.FileName;
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
            mid_surface_model = null;
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
                double splitterAccuracy = double.Parse(textBox_Splitter_Accuracy.Text, CultureInfo.InvariantCulture);
                double detalizerAccuracy = double.Parse(textBox_Detalizer_Accuracy.Text, CultureInfo.InvariantCulture);             
                IAlgorithm alg = new Algorithm(splitterAccuracy, detalizerAccuracy);
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)mainCanvas.ActualWidth, (int)mainCanvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                foreach (string path in allFoundFiles)
                {
                    Component.Model model_temp = new Component.Model();
                    model_temp.Add(new Parser().ImportFile(path));
                    model = model_temp;
                    RedrawModel();                   
                    mid_surface_model = alg.Run(new SolverData(model));
                    RedrawMisSurface();
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

        private void Do_Enumeration(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog FBD = new System.Windows.Forms.FolderBrowserDialog();
            if (FBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SolverData SD = new SolverData(model);
                List<ISegment> segments = new List<ISegment>();
                foreach (var contour in SD.GetContours())
                {
                    segments.AddRange(contour.GetSegments());
                }
                bool[] is_used = new bool[segments.Count];
                for (int i = 0; i < is_used.Length; i++) is_used[i] = false;
                int k = 0, kolvo = 0, nom_file = 0;
                double splitterAccuracy = double.Parse(textBox_Splitter_Accuracy.Text, CultureInfo.InvariantCulture);
                double detalizerAccuracy = double.Parse(textBox_Detalizer_Accuracy.Text, CultureInfo.InvariantCulture);
                IAlgorithm alg = new Algorithm(splitterAccuracy, detalizerAccuracy);
                Html view = new Html(filename);
                Primitive.Contour tmp_count;
                Primitive.Figure tmp_fig;
                Component.Model tmp_model;
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)mainCanvas.ActualWidth, (int)mainCanvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                PngBitmapEncoder BufferSave;
                while (kolvo != is_used.Length)
                {
                    //контролирующий список

                    kolvo = 0;
                    k = 0;
                    while (is_used[k] == true) k++;
                    is_used[k] = true;
                    for (int i = 0; i < k; i++) is_used[i] = false;
                    foreach (bool b in is_used)
                        if (b) kolvo++;

                    //выбор сегментов по контролирующему списку

                    tmp_count = new Primitive.Contour();
                    for (int i = 0; i < is_used.Length; i++)
                        if (is_used[i]) tmp_count.Add(segments[i]);
                    tmp_fig = new Primitive.Figure();
                    tmp_fig.Add(tmp_count);
                    tmp_model = new Component.Model();
                    tmp_model.Add(tmp_fig);
                    model = tmp_model;
                    RedrawModel();

                    //построение текущей поверхности

                    mid_surface_model = alg.Run(new SolverData(model));
                    RedrawMisSurface();

                    // сохранение картинки

                    mainCanvas.Measure(new Size((int)mainCanvas.ActualWidth, (int)mainCanvas.ActualHeight));
                    mainCanvas.Arrange(new Rect(new Size((int)mainCanvas.ActualWidth, (int)mainCanvas.ActualHeight)));
                    rtb.Render(mainCanvas);
                    BufferSave = new PngBitmapEncoder();
                    BufferSave.Frames.Add((BitmapFrame.Create(rtb)));
                    using (var fs = File.OpenWrite(FBD.SelectedPath + "\\image" + nom_file + ".png"))
                    {
                        BufferSave.Save(fs);
                        fs.Close();
                    }

                    view.Add(FBD.SelectedPath + "\\image" + nom_file + ".png");
                    nom_file++;
                }
                System.IO.File.WriteAllText(FBD.SelectedPath + "\\show.html", view.Save());
            }
        }

        private void ShowNormal_Click(object sender, RoutedEventArgs e)
        {
            if (model == null)
            {
                MessageBox.Show("No model is imported");
                return;
            }

            string[] debugParams = textBox_Debug.Text.Split(';');
            int segmentNum = 0;
            double t = 0;
            double multiplier = 0;
            try
            {
                segmentNum = int.Parse(debugParams[0]);
                t = double.Parse(debugParams[1], CultureInfo.InvariantCulture);
                multiplier = double.Parse(debugParams[2], CultureInfo.InvariantCulture);
            }
            catch
            {
                MessageBox.Show("Wrong format in debug parameters.\nFormat is:\n    <segment number>;<t value>;<multiplier value>");
                return;
            }

            ISegment chosenSegment = model.GetCanvasData().ElementAt(segmentNum);
            Point pointOnSegment = chosenSegment.GetCurvePoint(t);
            Normal normalForSegment = chosenSegment.GetNormal(t);
            if (t == 0)
            {
                int prevSegmentNum = segmentNum == 0 ? model.GetCanvasData().Count() - 1 : segmentNum - 1;
                normalForSegment = normalForSegment.Combine(model.GetCanvasData().ElementAt(prevSegmentNum).GetNormal(1));
            }
            else if (t == 1)
            {
                int nextSegmentNum = segmentNum == model.GetCanvasData().Count() - 1 ? 0 : segmentNum + 1;
                normalForSegment = model.GetCanvasData().ElementAt(nextSegmentNum).GetNormal(0).Combine(normalForSegment);
            }
            ISegment normalSegment = new Segment(new BezierCurve(), new List<Point>()
            {
                pointOnSegment,
                new Point(pointOnSegment.X + multiplier * normalForSegment.Dx(), pointOnSegment.Y + multiplier * normalForSegment.Dy())
            });

            View.VisibleDataSettings settings = new View.VisibleDataSettings();
            settings.Brush = Brushes.Blue;
            settings.Thikness = 1;
            IMidSurface debugSurface = new MidSurface();
            debugSurface.Add(normalSegment);
            View.VisibleData visible_data = new View.VisibleData(debugSurface, settings);
            view.Paint(visible_data);
        }

        private void ClearDebug_Click(object sender, RoutedEventArgs e)
        {
            view.EnableIndices(false);
            RedrawModel();
            RedrawMisSurface();
        }

        private void ShowOnlyPoints_Click(object sender, RoutedEventArgs e)
        {
            if (mid_surface_model == null)
                return;

            var segments = mid_surface_model.GetData();
            List<ISegment> only_points = new List<ISegment>();
            foreach (var segment in segments)
            {
                ISegment point = new Segment(new BezierCurve(), new List<Point>()
                {
                    segment.GetPillar()[0],
                    Vector.Add(new Vector(1, 0), segment.GetPillar()[0])
                });
                only_points.Add(point);
            }

            IMidSurface points_surface = new MidSurface();
            foreach (var point in only_points)
            {
                points_surface.Add(point);
            }
            mainCanvas.Children.Clear();
            RedrawModel();
            View.VisibleDataSettings settings = new View.VisibleDataSettings();
            settings.Brush = Brushes.Red;
            settings.Thikness = 2;
            View.VisibleData visible_data = new View.VisibleData(points_surface, settings);
            view.Paint(visible_data);
        }

        private void ShowIndices_Click(object sender, RoutedEventArgs e)
        {
            view.EnableIndices(true);
            view.SetIndexFontSize(14);
            RedrawModel();
            view.SetIndexFontSize(10);
            RedrawMisSurface();
        }
        private void ShowOnlyModelIndices_Click(object sender, RoutedEventArgs e)
        {
            view.EnableIndices(true);
            view.SetIndexFontSize(14);
            RedrawModel();
            view.EnableIndices(false);
            RedrawMisSurface();
        }
    }
}
