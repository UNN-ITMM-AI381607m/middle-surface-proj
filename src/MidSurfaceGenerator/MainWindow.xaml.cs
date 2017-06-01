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
using MidSurfaceNameSpace.Primitive;
using System.Diagnostics;

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
#if DEBUG
        private System.Windows.Point canvas_center;
        private System.Windows.Point diff_canvas_center;
        private double zoom_step = 2;
#endif
        public MainWindow()
        {
            InitializeComponent();
            view = new Component.View(mainCanvas);
#if RELEASE
            toolBar.Items.Remove(label_Debug);
            toolBar.Items.Remove(textBox_Debug);
            toolBar.Items.Remove(label_DebugNormalStep);
            toolBar.Items.Remove(textBox_DebugNormalStep);
#endif

#if DEBUG
            mainCanvas.MouseLeftButtonDown += CanvasDragBegin;
            mainCanvas.MouseLeftButtonUp += CanvasDragEnd;
            mainCanvas.MouseWheel += mainCanvas_MouseWheel;
            mainCanvas.SizeChanged += mainCanvas_SizeChanged;
            MenuItem debug = new MenuItem();
            List<RoutedEventHandler> debug_funcs = new List<RoutedEventHandler>();

            debug_funcs.Add(ShowNormal_Click);
            debug_funcs.Add(ClearDebug_Click);
            debug_funcs.Add(ShowOnlyPoints_Click);
            debug_funcs.Add(ShowIndices_Click);
            debug_funcs.Add(ShowOnlyModelIndices_Click);
            debug_funcs.Add(ShowSimplified_Click);
            debug_funcs.Add(ShowDetalizerNormals_Click);

            debug.SetValue(MenuItem.HeaderProperty ,"DEBUG");
            foreach(var item in debug_funcs)
            {
                MenuItem temp = new MenuItem();
                temp.Header = item.Method.ToString();
                temp.Click += item;
                debug.Items.Add(temp);
            }
            menu.Items.Add(debug);

#endif
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Dinar: think about changing statuses and implementation
            currentStatus.Content = "Импортирование модели.";
            currentStatus.UpdateLayout();

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
                    MessageBox.Show("Ошибка! Причина ошибки: " + ex.Message);
                    currentStatus.Content = "Готов к работе.";
                }
            }
            mid_surface_model = null;
            currentStatus.Content = "Импортирование завершено успешно. Готов к работе.";
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Dinar: prepare window with setting. Place for settings! 
            //MidSurfaceGenerator
            new Settings()
            {
                Owner = this
            }
            .ShowDialog();
            RedrawModel();
            RedrawMidSurface();
        }
        private void FlowSlice_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Dinar: prepare generating implementation
            if (model == null) return;
            RedrawModel();
            currentStatus.Content = "Генерация в процессе";
            currentStatus.UpdateLayout();
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

            IAlgorithm alg = new FlowAlgorithm(splitterAccuracy, detalizerAccuracy);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            mid_surface_model = alg.Run(new SolverData(model));
            sw.Stop();
            RedrawMidSurface();
            currentStatus.Content = "Генерация завершена успешно. Затраченное время: " + sw.Elapsed;
        }
        
        private void Generate(object sender, RoutedEventArgs e)
        {
            //TODO: Dinar: prepare generating implementation
            if (model == null) return;
            RedrawModel();
            currentStatus.Content = "Генерация в процессе";
            currentStatus.UpdateLayout();
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
            Stopwatch sw = new Stopwatch();
            sw.Start();
            mid_surface_model = alg.Run(new SolverData(model));
            sw.Stop();
            RedrawMidSurface();
            currentStatus.Content = "Генерация завершена успешно. Затраченное время: "+sw.Elapsed;
        }

        private void RedrawModel()
        {
            if (model == null) return;
            //TODO: Dinar: continue connecting parameters
            mainCanvas.Children.Clear();
            View.VisibleDataSettings settings = new View.VisibleDataSettings()
            {
                Brush = Settings.colofOfModel,
                Thikness = Settings.thicknessModel
            };
            View.VisibleData visible_data = new View.VisibleData(model, settings);
            view.Paint(visible_data);
        }
        private void RedrawMidSurface()
        {
            if (mid_surface_model == null) return;
            //TODO: Dinar: continue connecting parameters
            View.VisibleDataSettings settings = new View.VisibleDataSettings();
            settings.Brush = Settings.colofOfMidSurface;
            settings.Thikness = Settings.thicknessMidSurface;
            View.VisibleData visible_data = new View.VisibleData(mid_surface_model, settings);
            view.Paint(visible_data);
        }
        private void RedrawModel(Brush brush)
        {
            if (model == null) return;
            View.VisibleDataSettings settings = new View.VisibleDataSettings()
            {
                Brush = brush,
                Thikness = 10
            };
            View.VisibleData visible_data = new View.VisibleData(model, settings);
            view.Paint(visible_data);
        }
        private void RedrawMidSurface(Brush brush)
        {
            if (mid_surface_model == null) return;
            View.VisibleDataSettings settings = new View.VisibleDataSettings();
            settings.Brush = brush;
            settings.Thikness = 5;
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
                    RedrawMidSurface();
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
                Component.IModel current_model = model;
                Segments_iterator SI = new Segments_iterator(model);

                int nom_file = 0;

                double splitterAccuracy = double.Parse(textBox_Splitter_Accuracy.Text, CultureInfo.InvariantCulture);
                double detalizerAccuracy = double.Parse(textBox_Detalizer_Accuracy.Text, CultureInfo.InvariantCulture);
                IAlgorithm alg = new Algorithm(splitterAccuracy, detalizerAccuracy);

                Html view = new Html(filename);

                RenderTargetBitmap rtb = new RenderTargetBitmap((int)mainCanvas.ActualWidth, (int)mainCanvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                PngBitmapEncoder BufferSave;

                while (!SI.IsOver)
                {
                    model = current_model;
                    RedrawModel();
                    mid_surface_model = alg.Run(new SolverData(model));
                    RedrawMidSurface();

                    model = SI.Next;
                    RedrawModel(Brushes.Purple);

                    //построение текущей поверхности

                    mid_surface_model = alg.Run(new SolverData(model));
                    RedrawMidSurface(Brushes.Blue);

                    // сохранение картинки

                    mainCanvas.UpdateLayout();
                    //mainCanvas.Measure(new Size((int)mainCanvas.ActualWidth, (int)mainCanvas.ActualHeight));
                    //mainCanvas.Arrange(new Rect(new Size((int)mainCanvas.ActualWidth, (int)mainCanvas.ActualHeight)));
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
            RedrawMidSurface();
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
            RedrawMidSurface();
        }

        private void ShowOnlyModelIndices_Click(object sender, RoutedEventArgs e)
        {
            view.EnableIndices(true);
            view.SetIndexFontSize(14);
            RedrawModel();
            view.EnableIndices(false);
            RedrawMidSurface();
        }

        private void ShowSimplified_Click(object sender, RoutedEventArgs e)
        {
            if (model == null)
                return;
            mainCanvas.Children.Clear();
            View.VisibleDataSettings settings = new View.VisibleDataSettings()
            {
                Brush = Brushes.Black,
                Thikness = 2
            };
            View.VisibleData visible_data = new View.VisibleData(SimplifyModel(model), settings);
            view.EnableIndices(true);
            view.Paint(visible_data);
            RedrawMidSurface();
        }

        Component.Model SimplifyModel(Component.IModel model)
        {
            Component.Model simplified = new Component.Model();
            Primitive.Figure figure = new Primitive.Figure();
            Splitter splitter = new Splitter();

            var figures = model.GetData();
            foreach (var f in figures)
            {
                var contours = f.GetContours();
                var lines = splitter.Split(contours, double.Parse(textBox_Splitter_Accuracy.Text, CultureInfo.InvariantCulture));
                var new_segments = new List<ISegment>();
                foreach (var line in lines)
                {
                    new_segments.Add(JoinMSPoints.PointsToSegment(line.GetPoint1().GetPoint(),
                        line.GetPoint2().GetPoint()));
                }
                var contour = new Primitive.Contour();
                foreach (var new_s in new_segments)
                {
                    contour.Add(new_s);
                }
                figure.Add(contour);
            }
            simplified.Add(figure);
            return simplified;
        }

        private void ShowDetalizerNormals_Click(object sender, RoutedEventArgs e)
        {
            if (mid_surface_model == null) return;

            int step = 0;

            try
            {
                step = int.Parse(textBox_DebugNormalStep.Text);
            }
            catch
            {
                MessageBox.Show("Wrong format in debug parameters.\nFormat is:\n < step between points > ");
                return;
            }

            if (step <= 0)
            {
                MessageBox.Show("Enter positive number");
                return;
            }

            var segments = mid_surface_model.GetData();
            IMidSurface debugSurface = new MidSurface();
            for (int i = 0; i < segments.Count(); i++)
            {
                if (i % step != 0)
                    continue;

                var msseg = segments.ElementAt(i) as MSSegment;
                if (msseg == null) return;

                var normal = msseg.GetMSPillar()[0].GetNormal();
                var parent = msseg.GetMSPillar()[0].GetParent();
                var R = msseg.GetMSPillar()[0].GetRadius();

                ISegment normalSegment = new Segment(new BezierCurve(), new List<Point>()
                {
                    parent,
                    new Point(parent.X + R * normal.Dx(), parent.Y + R * normal.Dy())
                });

                debugSurface.Add(normalSegment);
            }

            View.VisibleDataSettings settings = new View.VisibleDataSettings();
            settings.Brush = Brushes.Blue;
            settings.Thikness = 1;
            View.VisibleData visible_data = new View.VisibleData(debugSurface, settings);
            view.Paint(visible_data);
        }

        private void OperatorGuide_Click(object sender, RoutedEventArgs e)
        {
            Process myProcess = new Process();
            try
            {
                // true is the default, but it is important not to set it to false
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = "https://github.com/sevoster/UNN_ITMM_AI381607m/blob/master/doc/%D0%A0%D1%83%D0%BA%D0%BE%D0%B2%D0%BE%D0%B4%D1%81%D1%82%D0%B2%D0%BE%20%D0%BE%D0%BF%D0%B5%D1%80%D0%B0%D1%82%D0%BE%D1%80%D0%B0_v.1.0.docx";
                myProcess.Start();
            }
            catch (Exception t)
            {
                Console.WriteLine(t.Message);
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //TODO: Dinar: think about changing statuses and implementation
            currentStatus.Content = "Экспортирование модели.";

            SaveFileDialog importDlg = new SaveFileDialog();
            importDlg.InitialDirectory = "C:\\";
            importDlg.Filter = "All files (*.*)|*.*|XML Models (*.xml)|*.xml";
            importDlg.FilterIndex = 2;
            importDlg.RestoreDirectory = true;
            if (importDlg.ShowDialog() != null)
            {
                try
                {
                    Component.Model model_temp = new Component.Model();
                    new Parser().ExportFile(mid_surface_model, importDlg.FileName);

                    //TODO: Dinar: some trick, maybe not good solution
                    model = null;
                    mid_surface_model = null;
                    RedrawModel();
                }
                catch (Exception ex)
                {
                    //TODO: Dinar: replace 
                    MessageBox.Show("Ошибка! Причина ошибки: " + ex.Message);
                    currentStatus.Content = "Готов к работе.";
                }
            }
            mid_surface_model = null;
            currentStatus.Content = "Экспортирование завершено успешно. Готов к работе.";
        }
#if DEBUG
        private void CanvasDragBegin(object sender, MouseButtonEventArgs e)
        {
            Control c = sender as Control;
            Mouse.Capture(c);
            diff_canvas_center = e.GetPosition(null);
        }
        private void CanvasDragEnd(object sender, MouseButtonEventArgs e)
        {

            canvas_center.X += (e.GetPosition(null).X - diff_canvas_center.X);
            canvas_center.Y += (e.GetPosition(null).Y - diff_canvas_center.Y);
            view.ChangeCenter(new Point(canvas_center.X, canvas_center.Y));
            RedrawModel();
            RedrawMidSurface();
            diff_canvas_center = new Point(0, 0);
        }

        private void mainCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvas_center = new Point(mainCanvas.ActualWidth / 2, mainCanvas.ActualHeight / 2);
        }
        private void mainCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0) view.ChangeZoom(zoom_step);
            else view.ChangeZoom(-zoom_step);
            RedrawModel();
            RedrawMidSurface();
        }
#endif
    }
}
